using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VibeManStates : MonoBehaviour
{
    [SerializeField]
    private PhotonView m_photonView;
    [SerializeField]
    private TMPro.TMP_Text mineStates;
    /*[SerializeField]
    private TMPro.TMP_Text WriteNumberText;*/
    [SerializeField]
    private GameObject vibeManBtn;
    [SerializeField]
    private GameObject vibeManBtn_Write8;
    [SerializeField]
    private LoadingScene loadingScene;

    public static string WriteNumber;
    public static bool vibeEnabled;

    private string s;

    private bool master_is_me;
    // Start is called before the first frame update
    private void Awake()
    {
        /*
        WriteNumber = "";
        s = "";
        if (PhotonNetwork.isMasterClient)
        {
            mineStates.text = "- Master -";
            vibeManBtn.SetActive(true);
        }
        else
        {
            mineStates.text = "- Client -";
            vibeManBtn.SetActive(false);
        }*/
    }
    void Start()
    {
        PhotonNetwork.isMessageQueueRunning = true;
        WriteNumber = "";
        s = "";
        if (PhotonNetwork.isMasterClient)
        {
            mineStates.text = "- Master -";
            vibeManBtn.SetActive(true);
            vibeManBtn_Write8.SetActive(true);
            master_is_me = true;
        }
        else
        {
            mineStates.text = "- Client -";
            vibeManBtn.SetActive(false);
            vibeManBtn_Write8.SetActive(false);
            master_is_me = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //WriteNumberText.text = WriteNumber;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.T))
        {
            PhotonNetwork.LeaveRoom();
            loadingScene.LoadTitleScene();
        }
        if((!PhotonNetwork.isMasterClient && vibeManBtn.activeSelf) || (PhotonNetwork.isMasterClient && !vibeManBtn.activeSelf))
        {
            Start();
        }

        if(master_is_me != PhotonNetwork.isMasterClient)
        {
            master_is_me = PhotonNetwork.isMasterClient;
            if (master_is_me)
            {
                mineStates.text = "- Master -";
                vibeManBtn.SetActive(true);
                vibeManBtn_Write8.SetActive(true);
                master_is_me = true;
            }
            else
            {
                mineStates.text = "- Client -";
                vibeManBtn.SetActive(false);
                vibeManBtn_Write8.SetActive(false);
                master_is_me = false;
            }
        }


    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(WriteNumber);
            Debug.Log("Writing.");
        }
        else
        {
            WriteNumber = (string)stream.ReceiveNext();
            Debug.Log("Receiving.");
        }
    }
    [PunRPC]
    void ResetWriteNumber()
    {
        WriteNumber = "";
    }

    void OnPhotonPlayerDisconnected()
    {
        Start();
    }
    void OnPhotonPlayerConnected(PhotonPlayer otherPlayer)
    {
        if(PhotonNetwork.isMasterClient)
        {
            if(vibeEnabled)
            {
                PhotonNetwork.SetMasterClient(otherPlayer);
            }
        }
        Start();
    }

    //マスターとクライアントの入れ替えを行うメソッド
    public void MasterAccount_Swich()
    {
        if (PhotonNetwork.isMasterClient)
        {
            //マスターの場合
            //プレイヤーのリストを取得
            List<PhotonPlayer> players = new List<PhotonPlayer>();
            try
            {
                //自分以外のアカウントを配列へ格納
                foreach (var player in PhotonNetwork.playerList)
                {
                    if (player != PhotonNetwork.player)
                    {
                        players.Add(player);
                    }
                }
            }
            finally
            {
                foreach (var player in players)
                {
                    Debug.Log("player_ID" + player.ID);
                }
            }

            //アカウント移行先が一つのみ
            if (players.Count == 1)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (vibeEnabled)
                    {
                        //マスター権利移行
                        PhotonNetwork.SetMasterClient(players[0]);
                    }
                }
            }
            else//移行先が一つ以上
            {

            }
        }
        else
        {
            //クライアントの場合
            //マスターに権限の意向を申請する
            PhotonNetwork.SetMasterClient(PhotonNetwork.player);
        }

    }

}

