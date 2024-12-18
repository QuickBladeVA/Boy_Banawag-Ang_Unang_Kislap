using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Move { Idle, LPunch, RPunch, LDodge, RDodge }

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public PlayerController player;       // Player 1 Controller
    public AIController enemy;            // Enemy AI Controller
    public PlayerController2 player2;     // Player 2 Controller (PvP)

    public GameObject playerObj;          // Player 1 GameObject
    public GameObject enemyObj;           // Player 2 or Enemy GameObject

    public Move playerMove;               // Player 1 Move
    public Move enemyMove;                // Player 2 Move or Enemy Move (AI)

    public bool isPvP;                    // Toggle for PvP Mode

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        // Determine if PvP or PvE
        if (GameObject.FindWithTag("Enemy") != null)
        {
            enemy = GetComponent<AIController>();
            isPvP = false;
        }
        if (GameObject.FindWithTag("Player 2") != null)
        {
            isPvP = true;
        }

        // Find Player 1 Controller
        if (isPvP)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        else 
        {
            player = GetComponent<PlayerController>();
        }



        // Find Player 1 GameObject
        if (playerObj == null)
        {
            try
            {
                playerObj = GameObject.FindWithTag("Player");
            }
            catch
            {
                Debug.Log("Objects with Tag:Player is Missing or More than 1");
            }
        }

        // Find Enemy or Player 2 GameObject
        if (enemyObj == null)
        {
            try
            {
                if (!isPvP)
                {
                    enemyObj = GameObject.FindWithTag("Enemy");
                }
                if (isPvP)
                {
                    enemyObj = GameObject.FindWithTag("Player 2");
                    player2 = enemyObj.GetComponent<PlayerController2>();
                }
            }
            catch
            {
                Debug.Log("Objects with Tag:Enemy is Missing or More than 1");
            }
        }

        // Set initial moves to Idle
        playerMove = Move.Idle;
        enemyMove = Move.Idle;

        // PvP Mode or PvE Mode
        if (isPvP)
        {
            StartCoroutine(Player(player2)); // Handle Player vs Player
        }
        else
        {
            StartCoroutine(Player(enemy));    // Handle Player vs AI
            StartCoroutine(Enemy(enemy));    // AI control logic
            List<Move> Idle = new List<Move>() { Move.Idle, Move.Idle, Move.Idle };
            enemy.ChangeMoveList(Idle);
        }

    }

    // AI Controller for the enemy (only used when not in PvP)
    IEnumerator Enemy(AIController enemy)
    {
        List<Move> Punch = new List<Move>() { Move.LPunch, Move.RPunch };

        while (!player.isKnockedOut && !enemy.isKnockedOut)
        {
            // Check if the player is tired
            if (player.isTired && enemy.moveList != Punch)
            {
                enemy.ChangeMoveList(Punch);
            }

            yield return new WaitForSeconds(0.8f);

            // Make the enemy's next move
            enemy.NextMove();
            enemyMove = enemy.move;

            if (enemy.isHit)
            {
                if (player.hasSuper)
                {
                    enemy.health -= player.attack * 4;
                    player.superPunch = 0;
                    player.hasSuper = false;
                }
                else
                {
                    enemy.health -= player.attack;
                }
                enemy.Hit();

                if (enemy.health <= 0)
                {
                    enemy.isKnockedOut = true;
                }
            }
        }
    }

    // Player 1 Logic for either PvP or PvE
    IEnumerator Player(AIController enemy)
    {
        while (!player.isKnockedOut && !enemy.isKnockedOut)
        {
            yield return new WaitForSeconds(0.01f);
            playerMove = player.move;
            if (player.isHit)
            {
                player.health -= enemy.attack;
                player.superPunch += enemy.attack;
                player.isHit = false;
                if (player.superPunch >= 80 && !player.hasSuperGained)
                {
                    player.hasSuper = true;
                    player.hasSuperGained = true;
                }

                if (player.health <= 0)
                {
                    player.isKnockedOut = true;
                }
            }
        }
    }

    // Player 1 Logic for PvP Mode (Handles Player 1 vs Player 2)
    IEnumerator Player(PlayerController2 player2)
    {
        while (!player.isKnockedOut && !player2.isKnockedOut)
        {
            yield return new WaitForSeconds(0.01f);

            // Handle Player 1 move
            playerMove = player.move;

            // Handle Player 2 move
            enemyMove = player2.move;

            // Player 1 takes damage if hit
            if (player.isHit)
            {
                if (player2.hasSuper)
                {
                    player.health -= player2.attack * 3;
                    player2.superPunch = 0;
                    player2.hasSuper = false;
                }
                else
                {
                    player.health -= player2.attack;
                }
                player.superPunch += player2.attack;
                player.isHit = false;
                if (player.superPunch >= 80 && !player.hasSuperGained)
                {
                    player.hasSuper = true;
                    player.hasSuperGained = true;
                }
                if (player.health <= 0)
                {
                    player.isKnockedOut = true;
                }
            }

            // Player 2 takes damage if hit
            if (player2.isHit)
            {
                if (player.hasSuper)
                {
                    player2.health -= player.attack * 3;
                    player.superPunch = 0;
                    player.hasSuper = false;
                }
                else
                {
                    player2.health -= player.attack;
                }
                player2.superPunch += player.attack;
                player2.isHit = false;
                if (player2.superPunch >= 80 && !player2.hasSuperGained)
                {
                    player2.hasSuper = true;
                    player2.hasSuperGained = true;
                }
                if (player2.health <= 0)
                {
                    player2.isKnockedOut = true;
                }
            }
        }
    }
}
