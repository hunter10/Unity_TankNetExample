using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : MonoBehaviour {

    public string version = "v1.0";

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 4;

    private void Awake()
    {
        // 포톤 클라우드에 접속
        PhotonNetwork.ConnectUsingSettings(version);
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    // 포톤 클라우드에 정상적으로 접속한 후 로비에 입장하면 호출되는 콜백함수
    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby !");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms !");

        // 룸 생성
        //PhotonNetwork.CreateRoom("MyRoom", true, true, 20);
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = MaxPlayersPerRoom }, null);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");

        // 탱크를 생성하는 함수 호출
        CreateTank();
    }

    void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }
}
