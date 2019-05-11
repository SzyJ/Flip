using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public PlayerChar playerPrefab;

    [HideInInspector]
    public PlayerChar localPlayer;


    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Main_Menu");
            return;
        }
    }

    void start()
    {
        PlayerChar.refreshInstance(ref localPlayer, playerPrefab);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        PlayerChar.refreshInstance(ref localPlayer, playerPrefab);
    }
}