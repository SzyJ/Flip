using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkConnectionManager : MonoBehaviourPunCallbacks
{

    public Button hostButton;
    public Button joinButton;

    public bool joiningHost = false;
    public bool startingHost = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        joiningHost = false;
        startingHost = false;

        hostButton.gameObject.SetActive(true);
        joinButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onJoinButtonClick()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "Test";
        PhotonNetwork.GameVersion = "v1";

        joiningHost = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void onHostButtonClick()
    {
        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.NickName = "Test";
        PhotonNetwork.GameVersion = "v1";

        startingHost = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void onExitButtonClick()
    {
        Application.Quit();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        joiningHost = false;
        startingHost = false;

        Debug.Log(cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Master");

        if (startingHost)
        {
            startingHost = false;
            Debug.Log("Creating Demo Room");
            PhotonNetwork.CreateRoom("Demo", new RoomOptions { MaxPlayers = 2 });
        }
        else if (joiningHost)
        {
            joiningHost = false;
            Debug.Log("Joining Demo Room");
            PhotonNetwork.JoinRoom("Demo");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        SceneManager.LoadScene("Battle_Map");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("Creating Demo Room");
        PhotonNetwork.CreateRoom("Demo", new RoomOptions { MaxPlayers = 2 });
    }
}
