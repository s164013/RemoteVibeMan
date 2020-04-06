using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System;
using UnityEngine.UI;
using UnityEngine.Profiling;
//using Object = UnityEngine.Object;

public class SerialManager: MonoBehaviour
{


    public SerialPort[] serial = new SerialPort[255];

    public static SerialPort Decport;//decided comport
    public static string comPortStr = "";
    //Object sync = new Object();

    private GameObject sample;
    private Color def_color;

    public static bool COMSetflag = false;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (COMSetflag)
        {
            SerialOpen();
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void SerialOpen()
    {
        try
        {
            if (!Decport.IsOpen)
            {//シリアルポートOpen
                Decport.Open();
            }
        }
        catch (Exception)
        {
            //Debug.LogError(ex.Message);
            return;
        }
    }

    void SerialClose()
    {
        try
        {
            if (Decport.IsOpen)
            {
                Decport.Close();
                Decport.Dispose();
            }
        }
        catch (Exception)
        {
            return;
        }
    }

    public static void Write(string message)
    {
        try
        {
            Decport.Write(message);
        }
        catch (System.Exception)
        {
            //      Debug.LogWarning(e.Message);
            Debug.Log("Error");
        }
    }
}