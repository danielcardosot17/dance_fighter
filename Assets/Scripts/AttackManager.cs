using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private BeatManager beatManager;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private List<Image> player1AttackBars;
    [SerializeField] private List<Image> player2AttackBars;
    [SerializeField] private Color originalBarColor;
    [SerializeField] private Color readyBarColor;

    private List<List<Image>> playerAttackBarList;
    private List<int> playerBeatCounter;

    private void Start() {
        playerAttackBarList = new List<List<Image>>();
        playerAttackBarList.Add(player1AttackBars);
        playerAttackBarList.Add(player2AttackBars);

        playerBeatCounter = new List<int>();
        playerBeatCounter.Add(0);
        playerBeatCounter.Add(0);
    }

    public bool IsPlayerAbleToAttack(int playerId)
    {
        return playerBeatCounter[playerId] > 0;
    }

    public string GetAttackType(int playerId)
    {
        if(playerBeatCounter[playerId] == 3)
        {
            return "attackHeavy";
        }
        else if(playerBeatCounter[playerId] == 2)
        {
            return "attackMedium";
        }
        else if(playerBeatCounter[playerId] == 1)
        {
            return "attackLight";
        }
        else
        {
            return "";
        }
    }

    public void PlayerAttack(int playerId)
    {
        if(playerBeatCounter[playerId] > 0)
        {
            var damage = Mathf.Pow(2, playerBeatCounter[playerId] - 1); // damage = 1, 2, 4
            healthManager.DealDamage(playerId, damage);
            Debug.Log("Player " + playerId.ToString() +  " deals " + damage.ToString() + " damage");
            ResetBeatCounter(playerId);
        }
    }

    public void IncreaseBeatCounter(int playerId)
    {
        if(playerBeatCounter[playerId] < 3){
            playerBeatCounter[playerId]++;
        }
        ChangeAttackBarColor(playerId);
    }

    public void ChangeAttackBarColor(int playerId)
    {
        playerAttackBarList[playerId][playerBeatCounter[playerId]-1].color = readyBarColor;
    }

    public void ResetAttackBarColors(int playerId)
    {
        foreach(var bar in playerAttackBarList[playerId])
        {
            bar.color = originalBarColor;
        }
    }

    public void ResetBeatCounter(int playerId)
    {
        playerBeatCounter[playerId] = 0;
        ResetAttackBarColors(playerId);
    }

}
