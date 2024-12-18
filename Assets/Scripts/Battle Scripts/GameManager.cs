using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { InGame,Pause, Win, Lose, Wait}

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;

    public bool Local;

    public BattleManager bManager = BattleManager.instance;

    public GameState gameState = GameState.InGame;

    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject waitUI;

    public int playerCount;
    public int playerNumber;

    KeyCode pause = KeyCode.Escape;

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
        Camera.main.aspect = 16f / 9f;

        try
        {
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        }
        catch 
        {
        
        }


    
    }
    // Update is called once per frame


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(gameState);

        }
        else
        {
            // Network player, receive data
            this.gameState = (GameState)stream.ReceiveNext();

        }

    }

    private void Start()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
    }

    void Win() 
    {
        if (!bManager.isPvP)
        {
            if (bManager.enemy.isKnockedOut)
            {
                gameState = GameState.Win;
            }
        }
        else if (bManager.isPvP)
        {
            if (bManager.player2.isKnockedOut)
            {
                gameState = GameState.Win;
            }
        }
    }

    void Update()
    {



        if (playerCount == 2 || !bManager.isPvP||Local)
        {
            try
            {
                playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            }
            catch { }


            if (gameState == GameState.Wait) 
            {
                gameState = GameState.InGame;
                waitUI.SetActive(false);
                Time.timeScale = 1;
            }

            switch (gameState)
            {
                case GameState.InGame:
                    if (bManager.player.isKnockedOut)
                    {
                        gameState = GameState.Lose;
                    }

                    Win();

                    if (Input.GetKeyUp(pause))
                    {
                        gameState = GameState.Pause;
                    }
                    break;

                case GameState.Pause:
                    Time.timeScale = 0;
                    pauseUI.SetActive(true);

                    if (Input.GetKeyUp(pause))
                    {
                        Time.timeScale = 1;
                        pauseUI.SetActive(false);
                        gameState = GameState.InGame;
                    }
                    break;

                case GameState.Win:
                    winUI.SetActive(true);
                    StopGame();
                    if (!BattleManager.instance.isPvP)
                    {
                        if (PlayerPrefs.GetInt("Level") <= SceneManager.GetActiveScene().buildIndex + 2)
                        {
                            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 2);
                        }
                    }

                    break;

                case GameState.Lose:
                    loseUI.SetActive(true);
                    StopGame();
                    break;
            }
        }
        else 
        {
            gameState=GameState.Wait;
            waitUI.SetActive(true);
            Time.timeScale = 0;

        }

    }

    void StopGame() 
    {
        if (!bManager.isPvP) 
        {
            bManager.player.move = Move.Idle;

            bManager.player.hasSuper = false;
            bManager.player.isTired = false;
            bManager.player.stamina = 5;

            bManager.enemy.move = Move.Idle;
        }

    }
}
