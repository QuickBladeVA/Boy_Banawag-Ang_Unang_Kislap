using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ResetLevel : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1.0f;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(WaitAndResetLevel());
        }
    }

    private IEnumerator WaitAndResetLevel()
    {
        yield return new WaitForSeconds(0.3f);

        // Load the level
        PhotonNetwork.LoadLevel("PVP");
    }
}
