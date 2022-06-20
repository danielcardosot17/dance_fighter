using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;
    [SerializeField] private Image player1HealthBar;
    [SerializeField] private Image player2HealthBar;
    [SerializeField] private TMP_Text player1HpText;
    [SerializeField] private TMP_Text player2HpText;
    [SerializeField][Range(10.0f, 100.0f)] private float enemyInitialHealth;

    private List<Image> enemyHealthBar; // Change the order!
    private List<float> enemyCurrentHealth;
    private List<TMP_Text> enemyHealthText;

    private void Start() {
        enemyCurrentHealth = new List<float>();
        enemyCurrentHealth.Add(enemyInitialHealth);// enemy of player 1  = player 2
        enemyCurrentHealth.Add(enemyInitialHealth);// enemy of player 2  = player 1

        enemyHealthBar = new List<Image>();
        enemyHealthBar.Add(player2HealthBar); // enemy of player 1  = player 2
        enemyHealthBar.Add(player1HealthBar); // enemy of player 2  = player 1

        enemyHealthText = new List<TMP_Text>();
        enemyHealthText.Add(player2HpText); // enemy of player 1  = player 2
        enemyHealthText.Add(player1HpText); // enemy of player 2  = player 1
    }

    public void ResetPlayerHp(int playerId)
    {
        enemyCurrentHealth[playerId] = enemyInitialHealth;
        enemyHealthBar[playerId].fillAmount = (enemyCurrentHealth[playerId]/enemyInitialHealth);
        enemyHealthText[playerId].text = enemyCurrentHealth[playerId].ToString();
    }

    public void DealDamage(int playerId, float damage)
    {
        enemyCurrentHealth[playerId] -= damage;
        if(enemyCurrentHealth[playerId] <= 0.0f)
        {
            enemyCurrentHealth[playerId] = 0.0f;
            enemyHealthBar[playerId].fillAmount = 0.0f;
            enemyHealthText[playerId].text = enemyCurrentHealth[playerId].ToString();
            gameMaster.EndGame();
        }
        else
        {
            enemyHealthBar[playerId].fillAmount = (enemyCurrentHealth[playerId]/enemyInitialHealth);
            enemyHealthText[playerId].text = enemyCurrentHealth[playerId].ToString();
        }
    }
}
