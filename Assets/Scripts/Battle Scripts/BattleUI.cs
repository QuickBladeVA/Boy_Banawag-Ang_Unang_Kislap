using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    private BattleManager bManager;
    private PlayerController player;
    private PlayerController2 player2;
    private AIController enemy;

    public Slider playerHealthSlider;
    public Slider playerStaminaSlider;
    public Slider playerSuperPunchSlider;
    public Slider enemyHealthSlider;

    // PvP
    public Slider enemyStaminaSlider;
    public Slider enemySuperPunchSlider;

    private const int MaxStamina = 5;
    private const int MaxSuperPunch = 75;

    void Start()
    {
        bManager = BattleManager.instance;

        player = bManager.player;
        player2 = bManager.player2;
        enemy = bManager.enemy;

        InitializePlayerSliders(player.health,playerHealthSlider, playerStaminaSlider, playerSuperPunchSlider);

        if (bManager.isPvP)
        {
            InitializePlayerSliders(player2.health, enemyHealthSlider, enemyStaminaSlider, enemySuperPunchSlider);
        }
        if (!bManager.isPvP)
        {
            InitializeEnemySlider();
        }
    }

    private void InitializePlayerSliders(int maxHealth,Slider healthSlider, Slider staminaSlider, Slider superPunchSlider)
    {
        healthSlider.maxValue = maxHealth;

        staminaSlider.maxValue = MaxStamina;

        superPunchSlider.maxValue = MaxSuperPunch;
    }

    private void InitializeEnemySlider()
    {
        if (!bManager.isPvP)
        {
            enemyHealthSlider.maxValue = bManager.enemy.health ;
        }
    }

    void Update()
    {
        UpdatePlayerSliders(player.health, player.stamina, player.superPunch, playerHealthSlider, playerStaminaSlider, playerSuperPunchSlider);

        if (bManager.isPvP)
        {
            UpdatePlayerSliders(player2.health, player2.stamina, player2.superPunch, enemyHealthSlider, enemyStaminaSlider, enemySuperPunchSlider);
        }
        else
        {
            if (enemy != null)
            {
                enemyHealthSlider.value = enemy.health;
            }
        }
    }

    private void UpdatePlayerSliders(int health, int stamina, int superPunch, Slider healthSlider, Slider staminaSlider, Slider superPunchSlider)
    {
        healthSlider.value = health;
        staminaSlider.value = stamina;
        superPunchSlider.value = superPunch;
    }
}
