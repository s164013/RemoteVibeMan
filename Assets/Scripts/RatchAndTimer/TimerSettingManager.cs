using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSettingManager : MonoBehaviour
{

    private const int MINUTE = 1;
    private const int SECOND = 1;
    private const int F_SECOND = 5;
    private const int MAXMINUTETIME = 10;
    private const int MINMINUTETIME = 0;
    private const int MAXSECONDTIME = 59;
    private const int MINSECONDTIME = 0;

    [SerializeField]
    private CreateDomino createDomino;
    [SerializeField]
    private TMPro.TMP_Text timer_txt;
    [SerializeField]
    private TMPro.TMP_Text mode_txt;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private DominoTimerManager dominoTimerManager;
    [SerializeField]
    private GameObject serialObj;
    [SerializeField]
    private AudioSource selectSE;
    [SerializeField]
    private AudioSource finishSE;

    private int settingTime_int;

    // Use this for initialization
    void Start()
    {
        Instantiate(serialObj, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickedMinuteBtn_Up()
    {
        if (StaticVariableField.MinuteTimer < MAXMINUTETIME)
        {
            StaticVariableField.MinuteTimer += MINUTE;
        }
        else
        {
            StaticVariableField.MinuteTimer = MINMINUTETIME;
        }
        if(StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        TimeTextChange();
        SelectSE_Play();
    }
    public void ClickedMinuteBtn_Down()
    {
        if (StaticVariableField.MinuteTimer > MINMINUTETIME)
        {
            StaticVariableField.MinuteTimer -= MINUTE;
        }
        else
        {
            StaticVariableField.MinuteTimer = MAXMINUTETIME;
        }
        if (StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        TimeTextChange();
        SelectSE_Play();
    }
    public void ClickedSecondBtn_Up()
    {
        if (StaticVariableField.SecondTimer < MAXSECONDTIME)
        {
            StaticVariableField.SecondTimer += SECOND;
        }
        else
        {
            StaticVariableField.SecondTimer = MINSECONDTIME;
        }
        if (StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        TimeTextChange();
        SelectSE_Play();
    }
    public void ClickedSecondBtn_Down()
    {
        if (StaticVariableField.SecondTimer > MINSECONDTIME)
        {
            StaticVariableField.SecondTimer -= SECOND;
        }
        else
        {
            StaticVariableField.SecondTimer = MAXSECONDTIME;
        }
        if (StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        TimeTextChange();
        SelectSE_Play();
    }
    public void Clicked5SecondBtn_Up()
    {
        if (StaticVariableField.SecondTimer <= MAXSECONDTIME - 5)
        {
            StaticVariableField.SecondTimer += F_SECOND;
        }
        else if (StaticVariableField.SecondTimer < MAXMINUTETIME)
        {
            StaticVariableField.SecondTimer = MAXSECONDTIME;
            Debug.Log("TimeUpdate.");

        }
        else
        {
            StaticVariableField.SecondTimer = MINSECONDTIME;
        }
        if (StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        TimeTextChange();
        SelectSE_Play();
    }
    public void Clicked5SecondBtn_Down()
    {
        if (StaticVariableField.SecondTimer >= MINSECONDTIME + 5)
        {
            StaticVariableField.SecondTimer -= F_SECOND;
        }
        else if (StaticVariableField.SecondTimer > MINSECONDTIME)
        {
            StaticVariableField.SecondTimer  = MINSECONDTIME;
            Debug.Log("TimeUpdate.");
        }
        else
        {
            StaticVariableField.SecondTimer = MAXSECONDTIME;
        }
        if (StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        TimeTextChange();
        SelectSE_Play();
    }

    public void ClickedResetBtn()
    {
        StaticVariableField.MinuteTimer = 0;
        StaticVariableField.SecondTimer = 0;
        TimeTextChange();
        SelectSE_Play();
    }

    public void ClickedStartBtn()
    {
        int timer = (StaticVariableField.MinuteTimer * 60) + (StaticVariableField.SecondTimer);
        if (timer > 0)
        {
            createDomino.Create(timer);
            dominoTimerManager.StartDomino(timer);
            //cameraMove.CameraStart();
            mainCanvas.enabled = false;
           
        }
        else
        {
            if(!StaticVariableField.StartVibeFlag_bool)
            {
                SerialManager.Write("7");
                StaticVariableField.StartVibeFlag_bool = true;
            }
            else
            {
                SerialManager.Write("8");
                StaticVariableField.StartVibeFlag_bool = false;
            }
        }
        SelectSE_Play();
    }

    public void ClickedBackBtn()
    {
        mainCanvas.enabled = true;
        foreach (GameObject obj in StaticVariableField.Domino_objList)
        {
            Destroy(obj);
        }
        StaticVariableField.Domino_objList.Clear();
        StaticVariableField.Domino_objList = new List<GameObject>();
        //cameraMove.ResetCameraPos();
        dominoTimerManager.ResetTime();
        if(StaticVariableField.StartVibeFlag_bool)
        {
            SerialManager.Write("8");
            StaticVariableField.StartVibeFlag_bool = false;
        }
        SelectSE_Play();
        FinishSE_Stop();
    }

    public void ClickedTest1Btn()
    {
        SerialManager.Write("5,1000");
        SelectSE_Play();
    }

    public void ClickedTest2Btn()
    {
        SerialManager.Write("5,1000\n");
        SelectSE_Play();
    }

    void TimeTextChange()
    {
        int timer = (StaticVariableField.MinuteTimer * 60) + (StaticVariableField.SecondTimer);
        timer_txt.text = StaticVariableField.MinuteTimer.ToString("00") + ":" + StaticVariableField.SecondTimer.ToString("00");
        if (timer == 0)
        {
            mode_txt.text = "ラッチ";
        }
        else
        {
            mode_txt.text = "タイマー";
        }
    }

    void SelectSE_Play()
    {
        selectSE.Play();
    }

    void FinishSE_Stop()
    {
        if(finishSE.isPlaying)
        {
            finishSE.Stop();
        }
    }
}
