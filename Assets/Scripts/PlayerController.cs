using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameEventSO pauseGameEvent;
    public GameEventSO PauseGameEvent { get => pauseGameEvent; set => pauseGameEvent = value; }
    private GameEventSO unPauseGameEvent;
    public GameEventSO UnPauseGameEvent { get => unPauseGameEvent; set => unPauseGameEvent = value; }
    private PlayerInputActions playerInputActions;
    private Animator playerAnimator;
    private bool isAttacking = false;
    private int playerId;
    public int PlayerId { get => playerId; set => playerId = value; }
    private GameEventSO beatSyncEvent;
    public GameEventSO BeatSyncEvent { get => beatSyncEvent; set => beatSyncEvent = value; }

    // private void Awake() {
    //     playerInputActions = new PlayerInputActions();
    //     playerAnimator = GetComponentInChildren<Animator>();
    // }
    
    // private void OnEnable() {
    //     playerInputActions.Player.Attack.performed += Attack;
    //     playerInputActions.Player.Beat.performed += BeatSync;
    //     playerInputActions.Player.PauseGame.performed += PauseGame;
    //     playerInputActions.UI.UnPauseGame.performed += UnPauseGame;
    // }

    private void BeatSync(InputAction.CallbackContext obj)
    {
        BeatSyncEvent.Raise();
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        PauseGameEvent.Raise();
    }
    private void UnPauseGame(InputAction.CallbackContext obj)
    {
        UnPauseGameEvent.Raise();
    }

    public void DisablePlayerInput()
    {
        playerInputActions.Player.Disable();
    }

    public void EnablePlayerInput()
    {
        playerInputActions.Player.Enable();
    }
    
    public void DisableUIInput()
    {
        playerInputActions.UI.Disable();
    }

    public void EnableUIInput()
    {
        playerInputActions.UI.Enable();
    }

    public void PauseAnimation()
    {
        playerAnimator.speed = 0;
    }
    public void UnPauseAnimation()
    {
        playerAnimator.speed = 1;
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        isAttacking = !isAttacking;
        playerAnimator.SetBool("isAttacking", isAttacking);
    }

    public void Initialize()
    {
        playerAnimator = GetComponentInChildren<Animator>();
        var playerInput = GetComponent<PlayerInput>();
        Debug.Log("playerInput.inputIsActive");
        Debug.Log(playerInput.inputIsActive);
        
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Attack.performed += Attack;
        playerInputActions.Player.Beat.performed += BeatSync;
        playerInputActions.Player.PauseGame.performed += PauseGame;
        playerInputActions.UI.UnPauseGame.performed += UnPauseGame;
    }
}
