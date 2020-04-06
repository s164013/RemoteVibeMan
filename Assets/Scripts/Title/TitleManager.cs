using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : Photon.MonoBehaviour
{
    public static bool JoinedLobby = false;

    private const int MAXPLAYERS = 2;

    [SerializeField]
    private GameObject dontDestroyObject;
    [SerializeField]
    private GameObject loginBtn;
    [SerializeField]
    private GameObject localBtn;
    [SerializeField]
    private GameObject logoutBtn;
    [SerializeField]
    private GameObject createBtn;
    [SerializeField]
    private GameObject joinBtn;
    [SerializeField]
    private GameObject[] roomBtnObjects;
    [SerializeField]
    private TMPro.TMP_Text errorText;
    [SerializeField]
    private LoadingScene loadingScene;

    private Button[] roomBtn;
    private Image[] roomBtnImages;
    private TMPro.TMP_Text[] roomBtnPlayerNumText;

    private Color emptyColor = Color.white;
    private Color waitColor = Color.yellow;
    private Color maxColor = Color.red;
    private string roomName;
    private AudioSource selectSE;

    // Start is called before the first frame update
    private void Awake()
    {
        /*if (!GameObject.Find("DontDestroyObject(Clone)"))
        {
            Instantiate(dontDestroyObject, Vector3.zero, Quaternion.identity);
        }*/
       
    }
    void Start()
    {
        if (!Screen.fullScreen)
        {
            Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
            Screen.fullScreen = true;
        }
        Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
        roomBtn = new Button[roomBtnObjects.Length];
        roomBtnImages = new Image[roomBtnObjects.Length];
        roomBtnPlayerNumText = new TMPro.TMP_Text[roomBtnObjects.Length];
        selectSE = GameObject.Find("SoundObjects/SEObject").GetComponent<AudioSource>();
        for (int i = 0; i < roomBtnObjects.Length; i++)
        {
            roomBtn[i] = roomBtnObjects[i].GetComponent<Button>();
            roomBtnImages[i] = roomBtnObjects[i].GetComponent<Image>();
            roomBtnPlayerNumText[i] = roomBtnObjects[i].transform.Find("PlayerNumText").GetComponent<TMPro.TMP_Text>();
        }
        roomName = "";
        if (JoinedLobby)
        {
            for (int i = 0; i < roomBtnObjects.Length; i++)
            {
                roomBtnObjects[i].SetActive(true);
            }
            if(loginBtn.activeSelf)
            {
                loginBtn.SetActive(false);
                localBtn.SetActive(false);
                logoutBtn.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void ClickedLoginBtn()
    {
        PhotonNetwork.ConnectUsingSettings("0.01"); // 前回 "0.02" 前々回 "0.1" なんで0.1にしたんや…
        //createBtn.SetActive(true);
        //joinBtn.SetActive(true);
        selectSE.Play();
        loginBtn.SetActive(false);
        localBtn.SetActive(false);
        logoutBtn.SetActive(true);
    }

    public void ClickedLocalBtn()
    {
        loadingScene.LoadRatchAndTimerScene();
    }

    public void ClickedJoinBtn()
    {
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            Debug.Log("RoomName : " + room.name.ToString());
            Debug.Log("RoomPlayer : " + room.playerCount.ToString());
            Debug.Log("RoomMaxPlayer : " + room.maxPlayers.ToString());

        }
        PhotonNetwork.JoinRoom(roomName);
        Debug.Log("Click!!");
    }

    /*public void InputField()
    {
        roomName = inputField.text;
    }*/

    public void ClickedLogoutBtn()
    {
        logoutBtn.SetActive(false);
        PhotonNetwork.Disconnect();
        selectSE.Play();
    }

    public void ClickedCreateBtn()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        roomOptions.MaxPlayers = MAXPLAYERS;
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);

    }

    public void ClickedRoomBtn(string BtnName)
    {
        selectSE.Play();
        List<string> roomName = new List<string>();
        Dictionary<string, int> roomInfo = new Dictionary<string, int>();
        bool createdRoom = false;
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            roomInfo.Add(room.Name, room.PlayerCount);
            Debug.Log("RoomName : " + room.name.ToString());
            Debug.Log("RoomPlayer : " + room.playerCount.ToString());
            Debug.Log("RoomMaxPlayer : " + room.maxPlayers.ToString());

        }
        foreach (string key in roomInfo.Keys)
        {
            if (key == BtnName)
            {
                createdRoom = true;
            }
        }
        if (createdRoom)
        {
            if (roomInfo[BtnName] != MAXPLAYERS)
            {
                PhotonNetwork.JoinRoom(BtnName);
            }
            else
            {
                return;
            }
        }
        else if (!createdRoom)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;
            roomOptions.MaxPlayers = MAXPLAYERS;
            PhotonNetwork.CreateRoom(BtnName, roomOptions, null);
        }
        /*switch (BtnName)
        {

            case "RoomBtn_Room01":
                {
                    
                    break;
                }
        }*/
    }

    void OnJoinedLobby()
    {
        Debug.Log("In?");
        for (int i = 0; i < roomBtnObjects.Length; i++)
        {
            roomBtnObjects[i].SetActive(true);
        }
        JoinedLobby = true;
    }

    void OnLeftLobby()
    {
        for (int i = 0; i < roomBtnObjects.Length; i++)
        {
            roomBtnObjects[i].SetActive(false);
        }
        JoinedLobby = false;
        loginBtn.SetActive(true);
        localBtn.SetActive(true);
        Debug.Log("LeftLobby");
    }

    void OnJoinedRoom() // Room参加時に呼び出し
    {
        Debug.Log("Join!!");
        PhotonNetwork.isMessageQueueRunning = false;
        loadingScene.LoadVibeManScene();
        JoinedLobby = true;
    }

    void OnFailedToConnectToPhoton() // 接続失敗時
    {
        errorText.text = "This PC cannot connect to EyeMoT server.";
    }

    void OnConnectionFail()
    {
        errorText.text = "This PC cannot connect to EyeMoT server.";
    }

    void OnReceivedRoomListUpdate() // Lobby内のRoomが更新されたときに呼び出し
    {
        Dictionary<string, int> roomInfo = new Dictionary<string, int>();
        List<Image> btnImages = new List<Image>(roomBtnImages);
        List<Button> btn = new List<Button>(roomBtn);
        List<int> playerNum = new List<int>();
        for (int i = 0; i < roomBtnObjects.Length; i++)
        {
            playerNum.Add(0);
        }
        foreach (RoomInfo room in PhotonNetwork.GetRoomList())
        {
            roomInfo.Add(room.Name, room.PlayerCount);
        }
        foreach (string key in roomInfo.Keys)
        {
            switch (key)
            {
                case "Room01":
                    {
                        if (roomInfo[key] == 1)
                        {
                            roomBtn[0].interactable = true;
                            roomBtnImages[0].color = waitColor;
                            playerNum[0] = 1;
                        }
                        else if (roomInfo[key] == 2)
                        {
                            roomBtn[0].interactable = false;
                            roomBtnImages[0].color = maxColor;
                            playerNum[0] = 2;
                        }
                        for (int i = 0; i < btn.Count; i++)
                        {
                            if (btn[i] == roomBtn[0])
                            {
                                btn.RemoveAt(i);
                                btnImages.RemoveAt(i);
                            }
                        }
                        break;
                    }
                case "Room02":
                    {
                        if (roomInfo[key] == 1)
                        {
                            roomBtn[1].interactable = true;
                            roomBtnImages[1].color = waitColor;
                            playerNum[1] = 1;
                        }
                        else if (roomInfo[key] == 2)
                        {
                            roomBtn[1].interactable = false;
                            roomBtnImages[1].color = maxColor;
                            playerNum[1] = 2;
                        }
                        for (int i = 0; i < btn.Count; i++)
                        {
                            if (btn[i] == roomBtn[1])
                            {
                                btn.RemoveAt(i);
                                btnImages.RemoveAt(i);
                            }
                        }
                        break;
                    }
                case "Room03":
                    {
                        if (roomInfo[key] == 1)
                        {
                            roomBtn[2].interactable = true;
                            roomBtnImages[2].color = waitColor;
                            playerNum[2] = 1;
                        }
                        else if (roomInfo[key] == 2)
                        {
                            roomBtn[2].interactable = false;
                            roomBtnImages[2].color = maxColor;
                            playerNum[2] = 2;
                        }
                        for (int i = 0; i < btn.Count; i++)
                        {
                            if (btn[i] == roomBtn[2])
                            {
                                btn.RemoveAt(i);
                                btnImages.RemoveAt(i);
                            }
                        }
                        break;
                    }
                case "Room04":
                    {
                        if (roomInfo[key] == 1)
                        {
                            roomBtn[3].interactable = true;
                            roomBtnImages[3].color = waitColor;
                            playerNum[3] = 1;
                        }
                        else if (roomInfo[key] == 2)
                        {
                            roomBtn[3].interactable = false;
                            roomBtnImages[3].color = maxColor;
                            playerNum[3] = 2;
                        }
                        for (int i = 0; i < btn.Count; i++)
                        {
                            if (btn[i] == roomBtn[3])
                            {
                                btn.RemoveAt(i);
                                btnImages.RemoveAt(i);
                            }
                        }
                        break;
                    }
                case "Room05":
                    {
                        if (roomInfo[key] == 1)
                        {
                            roomBtn[4].interactable = true;
                            roomBtnImages[4].color = waitColor;
                            playerNum[4] = 1;
                        }
                        else if (roomInfo[key] == 2)
                        {
                            roomBtn[4].interactable = false;
                            roomBtnImages[4].color = maxColor;
                            playerNum[4] = 2;
                        }
                        for (int i = 0; i < btn.Count; i++)
                        {
                            if (btn[i] == roomBtn[4])
                            {
                                btn.RemoveAt(i);
                                btnImages.RemoveAt(i);
                            }
                        }
                        break;
                    }
                case "Room06":
                    {
                        if (roomInfo[key] == 1)
                        {
                            roomBtn[5].interactable = true;
                            roomBtnImages[5].color = waitColor;
                            playerNum[5] = 1;
                        }
                        else if (roomInfo[key] == 2)
                        {
                            roomBtn[5].interactable = false;
                            roomBtnImages[5].color = maxColor;
                            playerNum[5] = 2;
                        }
                        for (int i = 0; i < btn.Count; i++)
                        {
                            if (btn[i] == roomBtn[5])
                            {
                                btn.RemoveAt(i);
                                btnImages.RemoveAt(i);
                            }
                        }
                        break;
                    }

            }
        }

        for (int i = 0; i < btn.Count; i++)
        {
            btn[i].interactable = true;
            btnImages[i].color = emptyColor;
        }
        for (int i = 0; i < roomBtnPlayerNumText.Length; i++)
        {
            roomBtnPlayerNumText[i].text = playerNum[i].ToString() + "/2";
        }
    }
}
