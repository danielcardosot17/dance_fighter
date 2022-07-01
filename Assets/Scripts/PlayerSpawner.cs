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
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private List<GameEventSO> animationFinishedEvents;
    [SerializeField][Range(0.1f, 1.0f)] private float resetDuration = 0.5f;
    
    private List<string> controlSchemeList;
    private List<int> playerPrefabIndexList;
    private List<GameObject> playerPrefabList;
    private List<GameObject> players;

    private void Awake() {
        controlSchemeList = new List<string>();
        foreach(var control in controls.controlSchemes)
        {
            controlSchemeList.Add(control.name);
        }
        playerPrefabIndexList = new List<int>();
        playerPrefabList = new List<GameObject>();
        players = new List<GameObject>();
        playerPrefabIndexList.Add(0);
        playerPrefabIndexList.Add(0);
        playerPrefabList.Add(new GameObject("Preview Player 1"));
        playerPrefabList.Add(new GameObject("Preview Player 2"));
    }

    public void SpawnPlayers()
    {
        for(int i = 0; i < 2; i++)
        {
            var newPlayerGO = PlayerInput.Instantiate(playerPrefabs[playerPrefabIndexList[i]], controlScheme: controlSchemeList[i], pairWithDevice: Keyboard.current).gameObject;
            newPlayerGO.transform.position = spawnPoints[i].position;
            newPlayerGO.transform.rotation = spawnPoints[i].rotation;
            AddPlayerController(newPlayerGO, i);
            AddAnimatorController(newPlayerGO, i);
            AddAnimationFinishedEventListener(newPlayerGO, i);
            players.Add(newPlayerGO);
            gameMaster.PlayerList.Add(newPlayerGO.GetComponent<PlayerController>());
        }
    }

    private void AddAnimationFinishedEventListener(GameObject newPlayerGO, int playerId)
    {
        var listener = newPlayerGO.AddComponent<GameEventListener>();
        listener.Event = animationFinishedEvents[playerId];
        animationFinishedEvents[playerId].RegisterListener(listener);
        listener.Response = new UnityEvent();
        listener.Response.AddListener(newPlayerGO.GetComponent<PlayerController>().AttackAnimationFinished);
    }

    public List<PlayerController> GetPlayerControllerList()
    {
        var playerControllerList = new List<PlayerController>();
        foreach(var player in players)
        {
            playerControllerList.Add(player.GetComponent<PlayerController>());
        }
        return playerControllerList;
    }

    private void AddAnimatorController(GameObject newPlayerGO, int playerId)
    {
        var animator = newPlayerGO.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = animatorController;
        var controller = animator.gameObject.AddComponent<AnimatorPositionController>();
        controller.AnimationFinishedEvent = animationFinishedEvents[playerId];
    }

    private void AddPlayerController(GameObject newPlayerGO, int playerId)
    {
        var newPlayer = newPlayerGO.AddComponent<PlayerController>();
        newPlayer.PlayerId = playerId;
        newPlayer.BeatManager = beatManager;
        newPlayer.GameMaster = gameMaster;
        newPlayer.AttackManager = attackManager;
        newPlayer.ResetDuration = resetDuration;
        newPlayer.Initialize();
    }

    public void PreviewModel(int playerIndex, int modelIndex)
    {
        Destroy(playerPrefabList[playerIndex]);
        var preview = Instantiate(playerPrefabs[modelIndex], spawnPoints[playerIndex].position, spawnPoints[playerIndex].rotation);
        AddAnimatorController(preview, playerIndex);
        playerPrefabList[playerIndex] = preview;
    }

    public void NextAvatar(int playerIndex)
    {
        var modelIndex = playerPrefabIndexList[playerIndex] + 1;
        if(modelIndex >= playerPrefabs.Count)
        {
            modelIndex = 0;
        }
        playerPrefabIndexList[playerIndex] = modelIndex;
        PreviewModel(playerIndex, modelIndex);
    }

    public void DestroyAllPreviewModels()
    {
        foreach(var gameObject in playerPrefabList)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyPlayers()
    {
        if(players.Count > 0)
        {
            for(int i = 1; i >=0; i--)
            {
                var playerToDestroy = players[i];
                players.RemoveAt(i);
                Destroy(playerToDestroy);
            }
            players = new List<GameObject>();
        }
    }
}
