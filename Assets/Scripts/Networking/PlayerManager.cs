using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int playerCount;
    public Button Start;
    public Image sprite;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Start.interactable=false;
        try
        {
            sprite.color = Color.gray;
        }
        catch { }
        

    }

    private void Update()
    {
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        if (playerCount == 2)
        {
            if (PhotonNetwork.IsMasterClient&& !Start.interactable)
            {
                Start.interactable=true;
            }
            if (!PhotonNetwork.IsMasterClient&& Start.interactable) 
            {
                Start.interactable = false;
            }
            try
            {
                sprite.color = Color.white;
            }
            catch { }
        }
       
    }
    public void LoadNextScene()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            PhotonNetwork.LoadLevel("PVP");
        }

    }
    public void LoadResetScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
            PhotonNetwork.LoadLevel("Load");
        }

    }


}
