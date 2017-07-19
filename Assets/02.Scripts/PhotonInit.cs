using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviour {

    public string version = "v1.0";

    public InputField userId;

    public InputField roomName;

    public GameObject scrollContetns;

    public GameObject roomItem;

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 4;

    private void Awake()
    {
        if (!PhotonNetwork.connected)
        {
            // 포톤 클라우드에 접속
            PhotonNetwork.ConnectUsingSettings(version);
        }

        userId.text = GetUserId();

        roomName.text = "ROOM_" + Random.Range(0, 999).ToString("000");
    }



    // 포톤 클라우드에 정상적으로 접속한 후 로비에 입장하면 호출되는 콜백함수
    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby !");
        userId.text = GetUserId();

        //PhotonNetwork.JoinRandomRoom();
    }

    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");

        if(string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        }

        return userId;
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No rooms !");

        // 룸 생성
        //PhotonNetwork.CreateRoom("MyRoom", true, true, 20);
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions() { maxPlayers = MaxPlayersPerRoom }, null);
    }

    void OnJoinedRoom()
    {
        Debug.Log("Enter Room");

        // 탱크를 생성하는 함수 호출
        //CreateTank();

        StartCoroutine(this.LoadBattleField());
    }

    IEnumerator LoadBattleField()
    {
        PhotonNetwork.isMessageQueueRunning = false;

        AsyncOperation ao = Application.LoadLevelAsync("scBattleField");

        yield return ao;
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.player.name = userId.text;

        PlayerPrefs.SetString("USER_ID", userId.text);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickCreateRoom()
    {
        string _roomNmae = roomName.text;

        if(string.IsNullOrEmpty(roomName.text))
        {
            _roomNmae = "ROOM_" + Random.Range(0, 999).ToString("000");
        }

        PhotonNetwork.player.name = userId.text;

        PlayerPrefs.SetString("USER_ID", userId.text);

        // 생성할 룸의 조건 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.isOpen = true;
        roomOptions.isVisible = true;
        roomOptions.maxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomNmae, roomOptions, TypedLobby.Default);
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Create Room Failed = " + codeAndMsg[1]);
    }

    void OnReceivedRoomListUpdate()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM"))
        {
            Destroy(obj);
        }

        int rowCount = 0;
        // 스크롤 영역 초기화 
        scrollContetns.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            Debug.Log(_room.name);

            GameObject room = (GameObject)Instantiate(roomItem);

            room.transform.SetParent(scrollContetns.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.name;
            roomData.connectPlayer = _room.playerCount;
            roomData.maxPlayers = _room.maxPlayers;
            roomData.DispRoomData();



            // RoomItem의 Button 컴포넌트에 클릭 이벤트를 동적으로 연결 
            roomData.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
                {
                    OnClickRoomItem(roomData.roomName);
                }
                                                                              );


             
            // Grid Layout Group 컴포넌트의 Constraint Count 값을 증가시킴 
            scrollContetns.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;

            // 스크롤 영역의 높이를 증가시킴
            scrollContetns.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }
    }

    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.player.name = userId.text;

        PlayerPrefs.SetString("USER_ID", userId.text);

        PhotonNetwork.JoinRoom(roomName);
    }

    private void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}
