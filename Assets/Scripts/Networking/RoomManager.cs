using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomNumberInput;

    #region buttonOnClick

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom(roomNumberInput.text, roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomNumberInput.text);

    }

    #endregion
}
