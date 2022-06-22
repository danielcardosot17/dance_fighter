using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActionMap playerActionMap;
    private InputActionMap uiActionMap;
    private Animator playerAnimator;
    private GameMaster gameMaster;
    private BeatManager beatManager;
    private AttackManager attackManager;
    private bool isAttacking = false;
    private bool isEndgame = false;
    private bool isWinner = false;
    private int playerId;

    public int PlayerId { get => playerId; set => playerId = value; }
    public GameMaster GameMaster { get => gameMaster; set => gameMaster = value; }
    public BeatManager BeatManager { get => beatManager; set => beatManager = value; }
    public AttackManager AttackManager { get => attackManager; set => attackManager = value; }

    private void Awake() {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void BeatSync(InputAction.CallbackContext obj)
    {
        BeatManager.PlayerBeatAnimation(PlayerId);
        if(BeatManager.CheckIfPlayerBeatIsOnTime(PlayerId))
        {
            AttackManager.IncreaseBeatCounter(PlayerId);
        }
        else
        {
            AttackManager.ResetBeatCounter(PlayerId);
        }
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        isAttacking = !isAttacking;
        playerAnimator.SetBool("isAttacking", isAttacking);

        BeatManager.PlayerBeatAnimation(PlayerId);
        if(BeatManager.CheckIfPlayerBeatIsOnTime(PlayerId))
        {
            AttackManager.PlayerAttack(PlayerId);
        }
        else
        {
            AttackManager.ResetBeatCounter(PlayerId);
        }
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        GameMaster.PauseGame();
    }

    private void UnPauseGame(InputAction.CallbackContext obj)
    {
        GameMaster.UnPauseGame();
    }

    public void DisablePlayerInput()
    {
       playerActionMap.Disable();
    }

    public void EnablePlayerInput()
    {
        playerActionMap.Enable();
    }
    
    public void DisableUIInput()
    {
        uiActionMap.Disable();
    }

    public void EnableUIInput()
    {
        uiActionMap.Enable();
    }

    public void PauseAnimation()
    {
        playerAnimator.speed = 0;
    }
    public void UnPauseAnimation()
    {
        playerAnimator.speed = 1;
    }


    public void Initialize()
    {
        var playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponentInChildren<Animator>();

        playerActionMap = playerInput.actions.FindActionMap("Player");
        uiActionMap = playerInput.actions.FindActionMap("UI");
        
        playerActionMap.FindAction("Attack").performed += Attack;
        playerActionMap.FindAction("Beat").performed += BeatSync;
        playerActionMap.FindAction("PauseGame").performed += PauseGame;
        uiActionMap.FindAction("UnPauseGame").performed += UnPauseGame;
        DisableUIInput();
        DisablePlayerInput();

        ResetAnimatorVariables();
    }

    private void OnDisable() {
        playerActionMap.FindAction("Attack").performed -= Attack;
        playerActionMap.FindAction("Beat").performed -= BeatSync;
        playerActionMap.FindAction("PauseGame").performed -= PauseGame;
        uiActionMap.FindAction("UnPauseGame").performed -= UnPauseGame;
        DisableUIInput();
        DisablePlayerInput();
    }

    public void PlayRandomLoserAnimation()
    {
        var randomInt = UnityEngine.Random.Range(0, gameMaster.NumberOfDefeatAnimations);
        isAttacking = false;
        isEndgame = true;
        isWinner = false;
        playerAnimator.SetBool("isAttacking", isAttacking);
        playerAnimator.SetBool("isEndgame", isEndgame);
        playerAnimator.SetBool("isWinner", isWinner);
        playerAnimator.SetInteger("defeatNumber", randomInt);
        playerAnimator.SetTrigger("defeat");
    }

    public void PlayRandomWinnerAnimation()
    {
        var randomInt = UnityEngine.Random.Range(0, gameMaster.NumberOfVictoryAnimations);
        isAttacking = false;
        isEndgame = true;
        isWinner = true;
        playerAnimator.SetBool("isAttacking", isAttacking);
        playerAnimator.SetBool("isEndgame", isEndgame);
        playerAnimator.SetBool("isWinner", isWinner);
        playerAnimator.SetInteger("victoryNumber", randomInt);
        playerAnimator.SetTrigger("victory");
    }

    public void PlayRandomAttackAnimation(string typeOfAttack, int randomInt)
    {
        isAttacking = true;
        playerAnimator.SetBool("isAttacking", isAttacking);
        playerAnimator.SetInteger("attackNumber", randomInt);
        playerAnimator.SetTrigger(typeOfAttack);
    }

    public void AttackAnimationFinished()
    {
        isAttacking = false;
        playerAnimator.SetBool("isAttacking", isAttacking);
    }

    private void ResetAnimatorVariables()
    {
        isAttacking = false;
        isEndgame = false;
        isWinner = false;
    }
}
