using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public Character character;

    public Move move;
 
    public string characterName;
    public int health,maxHealth, attack, speed;
    public bool isKnockedOut = false;
    public bool isHit = false;


    public List<Sequence> moveSequence;// The List of all move List Sequence
    /* Ex.
     * set 1 = Punch, Punch, Dodge
     * set 2 = Punch, Dodge, Dodge
     * set 3 = Punch, Punch, Dodge
     */

    public string moveListName;
    public List<Move> moveList;// The List of all move List 
    /* Ex.
     * Punch
     * Dodge
     * Punch
     */

    int random;
    int selection = 0;


    private void Awake()
    {
        if (character == null)
        {
            Debug.Log("AI Character Script is missing: Check Character Folder for reference");
        }
        else
        {
            characterName = character.name;
            health = character.hp;
            maxHealth = health;
            attack = character.atk;
            speed = character.spd;
        }

        if (moveSequence.Count > 0)
        {
            random = Random.Range(0, moveSequence.Count);

            moveList = moveSequence[random].moveList;
            moveListName = moveSequence[random].name;
            if (moveList.Count > 0)
            {
                move = moveList[0];
            }
            else
            {
                Debug.Log(moveListName+" is Empty: Check Sequence Folder for reference");
            }
        }
        else 
        {
            Debug.Log("AI Move Sequence is Empty: Check Sequence Folder for reference");
        }
    }

    public void NextMove()
    {
        if (selection == moveList.Count)
        {
            ChangeMoveList();
        }
        else 
        {
            selection += 1;
            move = moveList[selection-1];
        } 
    }

    public void Hit() 
    {
        ChangeMoveList();
        isHit = false;
    }

    public void ChangeMoveList() 
    {
        move = Move.Idle;
        selection = 0;

        random = Random.Range(0, moveSequence.Count);

        moveList = moveSequence[random].moveList;
        moveListName = moveSequence[random].name;
    }

    public void ChangeMoveList(List<Move> moveSequence)
    {
        move = Move.Idle;
        selection = 0;

        moveList = moveSequence;
        moveListName = moveSequence.ToString();
    }

}
