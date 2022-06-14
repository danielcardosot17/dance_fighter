using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameEventSO pauseGameEvent;
    [SerializeField] private GameEventSO unPauseGameEvent;
    [SerializeField] private GameEventSO beatSyncEvent;
    private PlayerInputActions playerInputActions;
    private Animator playerAnimator;
    private bool isAttacking = false;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerAnimator = GetComponentInChildren<Animator>();
    }
    
    private void OnEnable() {
        playerInputActions.Player.Attack.performed += Attack;
        playerInputActions.Player.Beat.performed += BeatSync;
        playerInputActions.Player.PauseGame.performed += PauseGame;
        // playerInputActions.Player.Enable();
        playerInputActions.UI.UnPauseGame.performed += UnPauseGame;
        // playerInputActions.UI.Disable();
    }

    private void BeatSync(InputAction.CallbackContext obj)
    {
        beatSyncEvent.Raise();
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        pauseGameEvent.Raise();
    }
    private void UnPauseGame(InputAction.CallbackContext obj)
    {
        unPauseGameEvent.Raise();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
