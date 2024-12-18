using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    BattleManager bManager;
    PlayerController player;
    PlayerController2 player2;
    AIController enemy;

    void Start()
    {
        bManager = BattleManager.instance;
        player = bManager.player;
        player2 = bManager.player2;
        enemy = bManager.enemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.CompareTag("Player"))
        {
            if (other.CompareTag("Enemy"))
            {
                Hit(player, enemy, Move.LPunch, Move.RDodge);
                Hit(player, enemy, Move.RPunch, Move.LDodge);
            }
            if (other.CompareTag("Player 2"))
            {
                Hit(player, player2, Move.LPunch, Move.RDodge);
                Hit(player, player2, Move.RPunch, Move.LDodge);
            }

        }

        if (this.CompareTag("Enemy"))
        {

            if (other.CompareTag("Player"))
            {
                Hit(enemy, player, Move.LPunch, Move.RDodge);
                Hit(enemy, player, Move.RPunch, Move.LDodge);
            }
        }

        if (this.CompareTag("Player 2"))
        {

            if (other.CompareTag("Player"))
            {
                Hit(player2, player, Move.LPunch, Move.RDodge);
                Hit(player2, player, Move.RPunch, Move.LDodge);
            }
        }

    }

    //PVE
    void Hit(PlayerController self, AIController target, Move punch, Move dodge)
    {
        // Check if self uses punch and target does not use dodge or any punch move
        if (self.move == punch)
        {
            if (target.move == Move.LPunch || target.move == Move.RPunch)
            {
                bManager.player.stamina -= 2;
            }
            else if (!(target.move == dodge || target.move == Move.LPunch || target.move == Move.RPunch))
            {
                // Target does not dodge or punch back, apply damage to the target only
                target.isHit = true;
            }
        }
    }

    //EvP
    void Hit(AIController self, PlayerController target, Move punch, Move dodge)
    {
        // Check if self uses punch
        if (self.move == punch)
        {

            if (!(target.move == dodge || target.move == Move.LPunch || target.move == Move.RPunch))
            {
                // Target does not dodge or punch back, apply damage to the target only
                target.isHit = true;
            }
        }
    }

    //PVP2
    void Hit(PlayerController self, PlayerController2 target, Move punch, Move dodge)
    {
        // Check if self uses punch and target does not use dodge or any punch move
        if (self.move == punch)
        {
            if (target.move == Move.LPunch || target.move == Move.RPunch)
            {
                bManager.player.stamina -= 2;
            }
            else if (!(target.move == dodge || target.move == Move.LPunch || target.move == Move.RPunch))
            {
                // Target does not dodge or punch back, apply damage to the target only
                target.isHit = true;
            }
        }
    }

    //P2vP
    void Hit(PlayerController2 self, PlayerController target, Move punch, Move dodge)
    {
        // Check if self uses punch
        if (self.move == punch)
        {

            if (target.move == Move.LPunch || target.move == Move.RPunch)
            {
                bManager.player2.stamina -= 2;
            }
            else if (!(target.move == dodge || target.move == Move.LPunch || target.move == Move.RPunch))
            {
                // Target does not dodge or punch back, apply damage to the target only
                target.isHit = true;
            }
        }
    }

}
