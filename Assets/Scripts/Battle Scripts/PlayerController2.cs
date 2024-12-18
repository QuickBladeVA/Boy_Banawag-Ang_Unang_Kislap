using Photon.Pun;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;



public class PlayerController2 : MonoBehaviourPunCallbacks,IPunObservable
{
    BattleManager bManager;

    public Character character;
    public Move move = Move.Idle;

    public string characterName;
    public int health, attack, speed, stamina, superPunch;

    public bool isKnockedOut = false;
    public bool isHit;
    public bool isTired = false;
    public bool hasSuper = false;
    public bool hasSuperGained = false;


    KeyCode LPunchKey = KeyCode.Q;
    KeyCode RPunchKey = KeyCode.W;
    KeyCode LDodgeKey = KeyCode.I;
    KeyCode RDodgeKey = KeyCode.P;

    bool isNotMoving = true;


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(move);
            stream.SendNext(isKnockedOut);
            stream.SendNext(isHit);
            stream.SendNext(isTired);
            stream.SendNext(isNotMoving);
            stream.SendNext(hasSuper);
            stream.SendNext(hasSuperGained);
        }
        else
        {
            // Network player, receive data
            this.move = (Move)stream.ReceiveNext();
            this.isKnockedOut = (bool)stream.ReceiveNext();
            this.isHit = (bool)stream.ReceiveNext();
            this.isTired = (bool)stream.ReceiveNext();
            this.isNotMoving = (bool)stream.ReceiveNext();
            this.hasSuper = (bool)stream.ReceiveNext();
            this.hasSuperGained = (bool)stream.ReceiveNext();
        }

    }

    // Start is called before the first frame update
    void Awake()
    {
        bManager = BattleManager.instance;

        if (character == null)
        {
            Debug.Log("Player Character Script is missing: Check Character Folder for reference");
        }
        else
        {
            characterName = character.name;
            health = character.hp;
            superPunch = 0;
            attack = character.atk;
            speed = character.spd;
            stamina = 5;
        }
        if (!GameManager.instance.Local)
        {
            photonView.TransferOwnership(PhotonNetwork.CurrentRoom.Players[2]);
        }

        


    }

    private void Start()
    {


        if (GameManager.instance.playerNumber == 2)
        {
            if (GameManager.instance.Local)
            {
                LPunchKey = KeyCode.Q;
                RPunchKey = KeyCode.W;
                LDodgeKey = KeyCode.I;
                RDodgeKey = KeyCode.P;
            }
            else
            {
                LPunchKey = KeyCode.Z;
                RPunchKey = KeyCode.X;
                LDodgeKey = KeyCode.LeftArrow;
                RDodgeKey = KeyCode.RightArrow;
            }



        }
        StartCoroutine(Action());
        StartCoroutine(StaminaRegen());
    }
    IEnumerator StaminaRegen()
    {
        while (true)
        {

            if (stamina < 5 && !isTired)
            {
                yield return new WaitForSeconds(3); // Wait for 5 seconds
                stamina +=2;
            }
            else
            {
                yield return null; // If stamina is full, just wait for the next frame
            }
            
        }
    }


    IEnumerator Action()
    {
        while (true) // Continuously check for input
        {
            if (!GameManager.instance.Local)
            {
                if (!photonView.IsMine)
                {
                    yield return null;
                }
            }
            if (!isTired)
            {
                if (isNotMoving)
                {
                    move = GetMoveInput();
                    if (move != Move.Idle)
                    {
                        isNotMoving = false;
                        if (move == Move.LPunch || move == Move.RPunch)
                        {
                            stamina-=1;
                        }
                        yield return new WaitForSeconds(0.5f);
                        isNotMoving = true;
                    }
                }
                yield return null; // Wait for next frame
            }

            if (stamina <= 0) 
            {
                isTired = true;
                move = Move.Idle;
                yield return new WaitForSeconds(3f);
                isTired = false;
            }

        }
    }

    private Move GetMoveInput()
    {
        if (Input.GetKeyDown(LPunchKey))
        {
            return Move.LPunch;
        }
        if (Input.GetKeyDown(RPunchKey))
        {
            return Move.RPunch;
        }
        if (Input.GetKeyDown(LDodgeKey))
        {
            return Move.LDodge;
        }
        if (Input.GetKeyDown(RDodgeKey))
        {
            return Move.RDodge;
        }

        return Move.Idle; // If no keys are pressed, return Idle
    }
}
