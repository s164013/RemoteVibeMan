using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominoTimerManager : MonoBehaviour
{

    [SerializeField]
    private TMPro.TMP_Text timer_txt;
    [SerializeField]
    private AudioSource countDownSE;
    [SerializeField]
    private AudioSource finishSE;

    private float time;
    private float debugTime;
    private float ratchTime;
    private float nowTime;
    private int shotSECounter;
    private bool timeCount_bool;

    // Use this for initialization
    void Start()
    {
        time = 0.1f;
        debugTime = time; 
        timeCount_bool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCount_bool)
        {
            TimeCount();
        }
    }

    void TimeCount()
    {
        time += Time.deltaTime;
        nowTime = ratchTime - time;
        int minute = (int)(nowTime) / 60;
        int second = (int)Mathf.Ceil((nowTime) % 60);
        if (second == 60)
        {
            minute += 1;
            second = 0;
        }

        timer_txt.text = minute.ToString("00") + ":" + second.ToString("00");
        if (nowTime <= 0)
        {
            nowTime = 0;
            timeCount_bool = false;
            timer_txt.text = "00:00";
            PlayFinishSE();
        }
        else
        {
            if (shotSECounter != second)
            {
                PlayCountSE();
                shotSECounter = second;
            }
        }
    }

    public void HitChangeColorDomino()
    {
        Debug.Log("Time : " + time.ToString());
    }
    public void EndDomino()
    {
        //cameraMove.CameraStop();
    }

    public void StartDomino(int settingTime)
    {
        timeCount_bool = true;
        ratchTime = settingTime;
        shotSECounter = (int)Mathf.Ceil((ratchTime) % 60);
        Debug.Log("RatchTime : " + ratchTime.ToString());
        if(!StaticVariableField.StartVibeFlag_bool)
        {
            StaticVariableField.StartVibeFlag_bool = true;
            Debug.Log("SerialPort_SendMessege_StartAction");
            //ドミノ開始時にバイブマンに送る信号
            SerialManager.Write("7");
        }
    }

    public void ResetTime()
    {
        time = 0.1f;
        timeCount_bool = false;
    }

    public void PlayCountSE()
    {
        countDownSE.Play();
    }

    public void PlayFinishSE()
    {
        finishSE.Play();
        if(StaticVariableField.StartVibeFlag_bool)
        {
            Debug.Log("SerialPort_SendMessege_EndAction");
            //ドミノ終了時にバイブマンに送る信号
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
    }
}
