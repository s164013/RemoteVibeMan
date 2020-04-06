using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;


public class PortOnTitle : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text VibeManEnabledText;

    //static Dropdown Maindrop;
    SerialPort port = new SerialPort();
    public string[] ports;
    //private static int dropdownNum;
    private string COMName;
    private int StringCoumt; //","の数
    private bool ThreadEndFlg = false;//スレッド処理が終わったらtrue
    private bool portDownFlag = false;//COMの自動決定後，ドロップダウンの表示が変わったら再びfalse
    private int vibeFoundFlag;
    //public GameObject ArduinoText;
    Thread thread;

    [SerializeField]
    private GameObject serialManager;

    //(1)
    private int number = 0;
    private int number_save = 0;
    string _portname;
    private int allnumber;
    //(1)

    void Awake()
    {
        thread = new Thread(COMSearch);
        thread.Start();
        VibeManEnabledText.text = "Searching...";
        COMName = "";
        vibeFoundFlag = 0;
        if(VibeManStates.vibeEnabled)
        {
            vibeFoundFlag = 1;
        }
        //ArduinoText = GameObject.Find("Canvas/ArduinoMessage");
    }

    void Start()
    {
        portDownFlag = true;
        //Maindrop = GetComponent<Dropdown>();
        // Fill ports array with COM's Name available
        ports = SerialPort.GetPortNames();
        //clear/remove all option item
        // Maindrop.options.Clear();
        //fill the dropdown menu OptionData with all COM's Name in ports[]
        //Maindrop.options.Add(new Dropdown.OptionData() { text = "Not Found" });
        /*( foreach (string c in ports)
        {
            Maindrop.options.Add(new Dropdown.OptionData() { text = c });
        }*/
        /*if (!SerialManage.COMSetflag) {
            try {
                Maindrop.value = 1;
                Maindrop.value = 0;
            }
            catch (Exception) {
                throw;
            }
        }
        else {
            if (dropdownNum == 0) {
                Maindrop.value = 1;
                Maindrop.value = 0;
            }
            Maindrop.value = dropdownNum;
        }*/
    }

    void FixedUpdate()
    {
        /*if (SerialManage.COMSetflag == false && ThreadEndFlg == false) {
            ArduinoText.GetComponent<Text>().text = "Arduinoを検索中です...";
        }
        else if (SerialManage.COMSetflag == true && ThreadEndFlg == true) {
            ArduinoText.GetComponent<Text>().text = "Arduinoが見つかりました";
        }
        else {
            ArduinoText.GetComponent<Text>().text = "Arduinoが見つかりません\n[F]キーで再検索";
        }*/
        //Portdownの表示を変える処理
        if (SerialManager.COMSetflag && portDownFlag && ThreadEndFlg)
        {
            //Maindrop.options.Clear();
            /*Maindrop.captionText.text = SerialManager.Decport.PortName.Substring(
                SerialManager.Decport.PortName.Length - 4, 4);
            Maindrop.options.Add(new Dropdown.OptionData() { text = Maindrop.captionText.text });*/
            COMName = SerialManager.Decport.PortName.Substring(
                SerialManager.Decport.PortName.Length - 4, 4);
            portDownFlag = false;
            if(!GameObject.Find("SerialObject(Clone)"))
            {
                Instantiate(serialManager, Vector3.zero, Quaternion.identity);
            }
        }
        if(vibeFoundFlag == 1)
        {
            VibeManEnabledText.text = "Found.";
            VibeManStates.vibeEnabled = true;
        }
        else if (vibeFoundFlag == 2 && !VibeManStates.vibeEnabled)
        {
            VibeManEnabledText.text = "Not Found.";
            VibeManStates.vibeEnabled = false;
        }
    }

    void Update()
    {
        //Debug.Log("vibefoundFlag : " + vibeFoundFlag.ToString());
        if (Input.GetKeyDown(KeyCode.F))
        {
            thread = null;
            vibeFoundFlag = 0;
            VibeManEnabledText.text = "Searching...";
            VibeManStates.vibeEnabled = false;
            thread = new Thread(COMSearch);
            ThreadEndFlg = false;
            SerialManager.COMSetflag = false;
            SerialManager.Decport = new SerialPort();
            thread.Start();
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            //デバッグ用
            Debug.Log("Debug_try");

            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                Debug.Log("port" + port);
            }

        }

        if (number_save != number)
        {
            //全体の何番目まで認識したか--(1)
            number_save = number;
            VibeManEnabledText.text = "Searching..." + _portname + " (" + number + "/" + allnumber + ")";
            //--(1)
        }
    }

    public void OnValueChanged()
    {
        if (ThreadEndFlg == true && SerialManager.COMSetflag == false)
        {
            //dropdownNum = Maindrop.value;
            SerialManager.comPortStr = COMName; // Maindrop.captionText.text;
            //Debug.Log(dropdownNum);
            SerialManager.Decport = new SerialPort("\\\\.\\" + COMName/*Maindrop.captionText.text*/, 9600);
            //Debug.Log(Maindrop.captionText.text);
            SerialManager.COMSetflag = true;
        }
    }

    void COMSearch()
    {
        bool getVibe = false;
        //ポートの全取得
        string[] _portList = SerialPort.GetPortNames();
        allnumber = _portList.Length;
        number = 0;

        if (port.IsOpen)
        {
            port.Close();
        }
        foreach (string _portName in _portList)
        {
            _portname = _portName;
            number++;

            if (SerialManager.COMSetflag == false)
            {

                try
                {
                    port = new SerialPort("\\\\.\\" + _portName, 9600, Parity.None, 8, StopBits.One);
                    //port = new SerialPort("\\\\.\\" + _portName, 9600);
                    port.Handshake = Handshake.None;
                    port.ReadTimeout = 2000;
                    port.RtsEnable = true;
                    port.DtrEnable = true;
                    StringCoumt = 0;
                    port.Open();

                    for (int i = 0; i < 255; i++)
                    {
                        if ((char)port.ReadByte() == ',')
                        {
                            StringCoumt++;
                        }
                        if (StringCoumt == 5)
                        {
                            Debug.Log("Arduino Find");
                            SerialManager.Decport = port;
                            SerialManager.Decport.ReadTimeout = 1;
                            SerialManager.COMSetflag = true;
                            vibeFoundFlag = 1;
                            SerialManager.Decport.DiscardInBuffer();
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    //print(e);
                    Debug.Log("Exception" + e);
                    Debug.Log("Arduino Not Find");
                    //vibeFoundFlag = 2;
                }
                finally
                {
                    port.Close();
                }
            }

            Debug.Log("_portName" + _portName);
            Debug.Log("ComsetFlag" + SerialManager.COMSetflag);

        }
        if(vibeFoundFlag != 1)
        {
            vibeFoundFlag = 2;
        }
        ThreadEndFlg = true;
        Debug.Log("ThreadEnd!");
    }

}