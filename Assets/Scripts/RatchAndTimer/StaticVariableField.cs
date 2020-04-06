using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVariableField : MonoBehaviour
{
    [SerializeField]
    private LoadingScene loadingScene;

    public static int MinuteTimer;
    public static int SecondTimer;
    public static List<GameObject> Domino_objList;
    public static float GazeTime = 1.0f;

    public static bool StartVibeFlag_bool;

    // Use this for initialization
    void Start()
    {
        MinuteTimer = 0;
        SecondTimer = 0;
        Domino_objList = new List<GameObject>();
        StartVibeFlag_bool = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            loadingScene.LoadTitleScene();
        }
    }
}