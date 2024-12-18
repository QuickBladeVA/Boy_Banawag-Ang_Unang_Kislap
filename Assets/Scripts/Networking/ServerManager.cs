using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public Button create;
    public Button join;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        create.interactable = false;
        join.interactable = false;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        base.OnConnectedToMaster();
        create.interactable = true;
        join.interactable = true;

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        base.OnJoinedLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room Successfully");
        PhotonNetwork.LoadLevel("WRS");
        base.OnJoinedRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room has been created");
        base.OnCreatedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room, Room cannot be found");
        base.OnJoinRandomFailed(returnCode, message);
    }
}
