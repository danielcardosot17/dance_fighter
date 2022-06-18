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
    private int playerId;
    public int PlayerId { get => playerId; set => playerId = value; }
    public GameMaster GameMaster { get => gameMaster; set => gameMaster = value; }
    public BeatManager BeatManager { get => beatManager; set => beatManager = value; }
    public AttackManager AttackManager { get => attackManager; set => attackManager = value; }

    private void Awake() {
        // playerInputActions = new PlayerInputActions();
        playerAnimator = GetComponentInChildren<Animator>();
    }
    
    // private void OnEnable() {
    //     playerInputActions.Player.Attack.performed += Attack;
    //     playerInputActions.Player.Beat.performed += BeatSync;
    //     playerInputActions.Player.PauseGame.performed += PauseGame;
    //     playerInputActions.UI.UnPauseGame.performed += UnPauseGame;
    // }

    private void BeatSync(InputAction.CallbackContext obj)
    {
        BeatManager.CheckIfPlayerBeatIsOnTime(PlayerId);
        BeatManager.PlayerBeatAnimation(PlayerId);
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        isAttacking = !isAttacking;
        playerAnimator.SetBool("isAttacking", isAttacking);

        AttackManager.PlayerAttack(PlayerId);
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
    }

    private void OnDisable() {
        playerActionMap.FindAction("Attack").performed -= Attack;
        playerActionMap.FindAction("Beat").performed -= BeatSync;
        playerActionMap.FindAction("PauseGame").performed -= PauseGame;
        uiActionMap.FindAction("UnPauseGame").performed -= UnPauseGame;
        DisableUIInput();
        DisablePlayerInput();
    }
}
