using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private InputActionAsset controls; 
    [SerializeField] private List<GameEventSO> playerBeatEvents;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private RuntimeAnimatorController animatorController;

    [SerializeField] private GameEventSO pauseGameEvent;
    [SerializeField] private GameEventSO unPauseGameEvent;
    [SerializeField] private GameEventSO disablePlayerInputEvent;
    [SerializeField] private GameEventSO enablePlayerInputEvent;
    [SerializeField] private GameEventSO disableUIInputEvent;
    [SerializeField] private GameEventSO enableUIInputEvent;
    [SerializeField] private GameEventSO unPausePlayerAnimationEvent;
    [SerializeField] private GameEventSO pausePlayerAnimationEvent;
    private List<string> controlSchemeList;
    private List<PlayerController> players;

    private void Awake() {
        players = new List<PlayerController>();
        controlSchemeList = new List<string>();
        foreach(var control in controls.controlSchemes)
        {
            controlSchemeList.Add(control.name);
        }
    }

    public void SpawnPlayers()
    {
        for(int i = 0; i < 2; i++)
        {
            int randomInt = UnityEngine.Random.Range(0, playerPrefabs.Count);
            var newPlayerGO = PlayerInput.Instantiate(playerPrefabs[randomInt], controlScheme: controlSchemeList[i], pairWithDevice: Keyboard.current).gameObject;
            newPlayerGO.transform.position = spawnPoints[i].position;
            newPlayerGO.transform.rotation = spawnPoints[i].rotation;
            AddPlayerController(newPlayerGO, i);
            AddAnimatorController(newPlayerGO);
            AddHealthController(newPlayerGO);
            AddGameEventListeners(newPlayerGO);
        }
    }

    private void AddAnimatorController(GameObject newPlayerGO)
    {
        var animator = newPlayerGO.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = animatorController;
    }

    private void AddGameEventListeners(GameObject newPlayerGO)
    {
        AddDisablePlayerInputListener(newPlayerGO);
        AddEnablePlayerInputListener(newPlayerGO);
        AddDisableUIInputListener(newPlayerGO);
        AddEnableUIInputListener(newPlayerGO);
        AddUnPausePlayerAnimationListener(newPlayerGO);
        AddPausePlayerAnimationListener(newPlayerGO);
    }

    private void AddPausePlayerAnimationListener(GameObject newPlayerGO)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = pausePlayerAnimationEvent;
        pausePlayerAnimationEvent.RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().PauseAnimation);
    }

    private void AddUnPausePlayerAnimationListener(GameObject newPlayerGO)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = unPausePlayerAnimationEvent;
        unPausePlayerAnimationEvent.RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().UnPauseAnimation);
    }

    private void AddEnableUIInputListener(GameObject newPlayerGO)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = enableUIInputEvent;
        enableUIInputEvent.RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().EnableUIInput);
    }

    private void AddDisableUIInputListener(GameObject newPlayerGO)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = disableUIInputEvent;
        disableUIInputEvent.RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().DisableUIInput);
    }

    private void AddEnablePlayerInputListener(GameObject newPlayerGO)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = enablePlayerInputEvent;
        enablePlayerInputEvent.RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().EnablePlayerInput);
    }

    private void AddDisablePlayerInputListener(GameObject newPlayerGO)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = disablePlayerInputEvent;
        disablePlayerInputEvent.RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().DisablePlayerInput);
    }

    private void AddHealthController(GameObject newPlayerGO)
    {
        var healthController = newPlayerGO.AddComponent<HealthController>();
    }

    private void AddPlayerController(GameObject newPlayerGO, int playerId)
    {
        var newPlayer = newPlayerGO.AddComponent<PlayerController>();
        newPlayer.PauseGameEvent = pauseGameEvent;
        newPlayer.UnPauseGameEvent = unPauseGameEvent;
        newPlayer.PlayerId = playerId;
        newPlayer.BeatSyncEvent = playerBeatEvents[playerId];
        newPlayer.Initialize();
    }
}
