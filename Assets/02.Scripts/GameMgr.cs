using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour {

    public Text txtConnect;

    // 접속 로그를 표시할 Text UI 항목변수
    public Text txtLogMsg;

    // RPC 호출을 위한 PhotonView
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        CreateTank();

        PhotonNetwork.isMessageQueueRunning = true;

        GetConnectPlayerCount();
    }

    IEnumerator Start()
    {
        string msg = "\n<color=#00ff00>[" + PhotonNetwork.player.name + "] Connected</color>";
        pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);

        // 룸에 있는 네트워크 객체 간의 통신이 완료될 때까지 잠시 대기
        yield return new WaitForSeconds(1.0f);

        SetConnectPlayerScore();
    }

    // 모든 탱크의 스코어 UI에 점수를 표시하는 함수를 호출
    void SetConnectPlayerScore()
    {
        PhotonPlayer[] players = PhotonNetwork.playerList;
        foreach(PhotonPlayer _player in players)
        {
            Debug.Log("[" + _player.ID + "]" + _player.name + " " + _player.GetScore() + " kill");
        }

        // 모든 Tank 프리팹을 배열에 저장
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("TANK");

        foreach(GameObject tank in tanks)
        {
            int currKillCount = tank.GetComponent<PhotonView>().owner.GetScore();
            tank.GetComponent<TankDamage>().txtKillCount.text = currKillCount.ToString();
        }
    }

    void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }

    // 룸 접속자 정보를 조회하는 함수
    void GetConnectPlayerCount()
    {
        // 현재 입장한 룸 정보를 받아옴
        Room currRoom = PhotonNetwork.room;

        txtConnect.text = currRoom.playerCount.ToString() + "/" + currRoom.maxPlayers.ToString();
    }

    // 네트워크 플레이어가 접속했을 때 호출되는 함수
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        GetConnectPlayerCount();
    }

    // 네트워크 플레이어가 룸을 나가거나 접속이 끊어졌을 때 호출되는 함수
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        GetConnectPlayerCount();
    }

    [PunRPC]
    void LogMsg(string msg)
    {
        txtLogMsg.text = txtLogMsg.text + msg;    
    }

    public void OnClickExitRoom()
    {
        PhotonNetwork.LeaveRoom();

		string msg = "\n<color=#ff0000>[" + PhotonNetwork.player.name + "] DisConnected</color>";
		pv.RPC("LogMsg", PhotonTargets.AllBuffered, msg);
    }

    void OnLeftRoom()
    {
        Application.LoadLevel("scLobby");
    }

}
