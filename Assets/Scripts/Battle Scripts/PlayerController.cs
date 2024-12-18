using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviourPunCallbacks,IPunObservable
{
    public Character character;
    public Move move = Move.Idle;

    public string characterName;
    public int health, attack, speed, stamina, superPunch;

    public bool isKnockedOut = false;
    public bool isHit;
    public bool isTired = false;
    public bool hasSuper = false;
    public bool hasSuperGained = false;

    public KeyCode LPunchKey;
    public KeyCode RPunchKey; 
    public KeyCode LDodgeKey;
    public KeyCode RDodgeKey;

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

    }
    private void Start()
    {
        if (BattleManager.instance.isPvP && !GameManager.instance.Local) {
            if (photonView.IsMine && GameManager.instance.playerNumber == 1)
            {

                LPunchKey = KeyCode.Z;
                RPunchKey = KeyCode.X;
                LDodgeKey = KeyCode.LeftArrow;
                RDodgeKey = KeyCode.RightArrow;
            }
        }
        if (!BattleManager.instance.isPvP || GameManager.instance.Local) 
        {
            LPunchKey = KeyCode.Z;
            RPunchKey = KeyCode.X;
            LDodgeKey = KeyCode.LeftArrow;
            RDodgeKey = KeyCode.RightArrow;
        }
        StartCoroutine(Action());
        StartCoroutine(StaminaRegen());
    }

    IEnumerator StaminaRegen()
    {
        while (true)
        {

            if (BattleManager.instance.isPvP)
            {
                if (stamina < 5 && !isTired)
                {
                    yield return new WaitForSeconds(3); // Wait for 5 seconds
                    stamina += 2;
                }
                else
                {
                    yield return null; // If stamina is full, just wait for the next frame
                }
            }
            else
            {
                if (stamina < 5 && !isTired)
                {
                    yield return new WaitForSeconds(5); // Wait for 5 seconds
                    stamina += 3;
                }
                else
                {
                    yield return null; // If stamina is full, just wait for the next frame
                }
            }
        }
    }


    IEnumerator Action()
    {
        while (true) // Continuously check for input
        {
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
                if (BattleManager.instance.isPvP)
                {
                    yield return new WaitForSeconds(3f);
                }
                else
                {
                    yield return new WaitForSeconds(5f);
                }
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
