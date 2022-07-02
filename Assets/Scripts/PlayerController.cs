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
    private float resetDuration;

    // private bool mayResetAnimatorTransform = false;

    public int PlayerId { get => playerId; set => playerId = value; }
    public GameMaster GameMaster { get => gameMaster; set => gameMaster = value; }
    public BeatManager BeatManager { get => beatManager; set => beatManager = value; }
    public AttackManager AttackManager { get => attackManager; set => attackManager = value; }
    public float ResetDuration { get => resetDuration; set => resetDuration = value; }

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
        BeatManager.PlayerBeatAnimation(PlayerId);
        if(BeatManager.CheckIfPlayerBeatIsOnTime(PlayerId))
        {
            if(AttackManager.IsPlayerAbleToAttack(PlayerId))
            {
                var attackType = AttackManager.GetAttackType(PlayerId);
                var numberOfAnimations = gameMaster.GetNumberOfAttackAnimations(attackType);
                var randomInt = UnityEngine.Random.Range(0, numberOfAnimations);
                // Debug.Log("AttackAnimation ID");
                // Debug.Log(randomInt);
                
                PlayAttackAnimation(attackType, randomInt);
                AttackManager.PlayerAttack(PlayerId);
            }
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

    public void PlayLoserAnimation(int randomInt)
    {
        isAttacking = false;
        isEndgame = true;
        isWinner = false;
        playerAnimator.SetBool("isAttacking", isAttacking);
        playerAnimator.SetBool("isEndgame", isEndgame);
        playerAnimator.SetBool("isWinner", isWinner);
        playerAnimator.SetInteger("defeatNumber", randomInt);
        playerAnimator.SetTrigger("defeat");
    }

    public void PlayWinnerAnimation(int randomInt)
    {
        isAttacking = false;
        isEndgame = true;
        isWinner = true;
        playerAnimator.SetBool("isAttacking", isAttacking);
        playerAnimator.SetBool("isEndgame", isEndgame);
        playerAnimator.SetBool("isWinner", isWinner);
        playerAnimator.SetInteger("victoryNumber", randomInt);
        playerAnimator.SetTrigger("victory");
        
        // Debug.Log("Victory Animation ID");
        // Debug.Log(randomInt);
    }

    public void PlayAttackAnimation(string typeOfAttack, int randomInt)
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
        // mayResetAnimatorTransform = true;
        ResetAnimatorTransform();
    }

    // private void LateUpdate() {
    //     var distance = Vector3.Distance(Vector3.zero, playerAnimator.transform.localPosition);
    //     if(mayResetAnimatorTransform && distance > 0.1f)
    //     {
    //         ResetAnimatorTransform();
    //     }
    // }

    private void ResetAnimatorTransform()
    {
        // mayResetAnimatorTransform = false;
        // playerAnimator.transform.localPosition = Vector3.zero;
        // playerAnimator.transform.localRotation = Quaternion.identity;
        StartCoroutine(LerpPosition(Vector3.zero, ResetDuration));
        StartCoroutine(LerpFunction(Quaternion.Euler(Vector3.zero), ResetDuration));
    }

    private void ResetAnimatorVariables()
    {
        isAttacking = false;
        isEndgame = false;
        isWinner = false;
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = playerAnimator.transform.localPosition;
        while (time < duration)
        {
            playerAnimator.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        playerAnimator.transform.localPosition = targetPosition;
    }
    IEnumerator LerpFunction(Quaternion endValue, float duration)
    {
        float time = 0;
        Quaternion startValue = playerAnimator.transform.localRotation;
        while (time < duration)
        {
            playerAnimator.transform.localRotation = Quaternion.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        playerAnimator.transform.localRotation = endValue;
    }
}
