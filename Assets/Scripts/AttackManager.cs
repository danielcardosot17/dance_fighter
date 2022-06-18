using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private List<Image> player1AttackBars;
    [SerializeField] private List<Image> player2AttackBars;

    private List<List<Image>> playerAttackBarList;
    private List<bool> playerAttackIsReady;
    private List<int> playerBeatCounter;

    private void Start() {
        playerAttackBarList = new List<List<Image>>();
        playerAttackBarList.Add(player1AttackBars);
        playerAttackBarList.Add(player2AttackBars);

        playerAttackIsReady = new List<bool>();
        playerAttackIsReady.Add(false);
        playerAttackIsReady.Add(false);

        playerBeatCounter = new List<int>();
        playerBeatCounter.Add(0);
        playerBeatCounter.Add(0);
    }

    public void PlayerAttack(int playerId)
    {
        
    }

}
