using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDomino : MonoBehaviour
{
    private const float BUFF_Z = -4f;

    [SerializeField]
    private GameObject domino_obj;
    [SerializeField]
    private GameObject turnDomino_objs;
    [SerializeField]
    private float createstartPos_X;
    [SerializeField]
    private Material dominoMaterial;
    [SerializeField]
    private int colorChangeNum;
    [SerializeField]
    private float createSpace;
    [SerializeField]
    private int createRowNum;
    [SerializeField]
    private GameObject dominoFloor;
    [SerializeField]
    private GameObject dominoFollow_obj;
    [SerializeField]
    private GameObject dominoFollowCamera_obj;
    [SerializeField]
    private AllDominoCameraController allDominoCameraController;

    private int createNum;
    private int loopCount;
    private int floorCreateCount;
    private int settingTime;
    private int inverse_int;
    private int lastCreateNum;
    private int notCreateTrunCount;
    private float startPos_X;
    private float startPos_Z;
    private float minX;
    private float minZ;
    private float maxX;
    private float maxZ;
    private Color createColor;
    private bool colorChange_bool;
    private bool loop_bool;
    private Vector3 floorCreatePos;

    // Use this for initialization
    void Start()
    {
        floorCreateCount = 1;
        floorCreatePos = new Vector3(700.0f, -0.05f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Create(int time)
    {
        inverse_int = 1;
        
        notCreateTrunCount = 0;
        settingTime = time;
        createColor = Color.white;
        startPos_X = createstartPos_X;
        startPos_Z = 0;
        minX = 0;
        minZ = 0;
        maxX = 0;
        maxZ = 0;
        lastCreateNum = 0;
        loopCount = 1;
        loop_bool = false;
        //allDominoCamera.SetCameraPos(startPos_X, startPos_X + ((settingTime - 1) * 5 * createSpace));
        dominoFollow_obj.transform.position = new Vector3(startPos_X, 3.0f, startPos_Z);//Instantiate(dominoFollow_obj, new Vector3(startPos_X, 3.0f, startPos_Z), Quaternion.identity);
        dominoFollowCamera_obj.transform.position = new Vector3(startPos_X, 3.0f, startPos_Z);//Instantiate(dominoFollow_obj, new Vector3(startPos_X, 3.0f, startPos_Z), Quaternion.identity);
        CreateDomino_Horizontal();
    }

    void CreateDomino_Vartical()
    {

    }

    void CreateDomino_Horizontal() // 60秒区切りで作成している  最初だけ2分作成  1秒 = ドミノ5個
    {
        int judgeNum = settingTime / (60 * loopCount); //　残り時間が60秒以上か判定
        notCreateTrunCount = 0;
        if (judgeNum > 0) // 60秒以上のとき、60秒分のドミノを作成
        {
            createNum = 60 * 5;
            loop_bool = true;
        }

        else // 60秒以下のとき、残り秒数分のドミノを作成
        {
            createNum = (settingTime - (60 * (loopCount - 1))) * 5;
            loop_bool = false;
        }

        for (int i = 0; i < createNum; i++)
        {
            float createPosX = startPos_X + (lastCreateNum * createSpace);
            GameObject obj = Instantiate(domino_obj, new Vector3(createPosX, 1.25f, startPos_Z), Quaternion.identity);
            if(minX > createPosX)
            {
                minX = createPosX;
            }
            if(maxX < createPosX)
            {
                maxX = createPosX;
            }
            if (i % colorChangeNum == 0)
            {
                createColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }
            else if (i % colorChangeNum == colorChangeNum - 1)
            {
                //obj.AddComponent<DominoHitManager>(); // デバッグ用スクリプトを付加していた
            }
            obj.GetComponent<Renderer>().material = dominoMaterial;
            obj.GetComponent<Renderer>().material.color = createColor;
            if (i == 0 && loopCount == 1)
            {
                obj.AddComponent<StartDomino>();
            }
            if (i == createNum - 1)
            {
                obj.AddComponent<LastDomino>();
                obj.AddComponent<TimeDebuger>();

                //startPos_X = startPos_X + ((lastCreateNum + inverse_int) * createSpace);
            }
            if (i % 15 == 14 && i != 0 && i < createNum - 25) // 1秒分生成後　かつ　残り10秒以上生成する余裕がある
            {
                int rand = Random.Range(0, 5);
                if(rand == 0 || notCreateTrunCount == 3)
                {
                    startPos_Z += BUFF_Z;
                    GameObject turnObj = Instantiate(turnDomino_objs, new Vector3(startPos_X + ((lastCreateNum) * createSpace), 1.25f, startPos_Z), Quaternion.identity);
                    if (inverse_int < 0)
                    {
                        turnObj.transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                    StaticVariableField.Domino_objList.Add(turnObj);
                    startPos_Z += BUFF_Z;
                    lastCreateNum += inverse_int;
                    inverse_int *= -1;
                    createNum -= 16;
                    notCreateTrunCount = 0;
                    maxZ = startPos_Z;
                }
                else
                {
                    notCreateTrunCount++;
                }
            }
            lastCreateNum += inverse_int;
            StaticVariableField.Domino_objList.Add(obj);
        }
        if (/*loopCount == 1 && */loop_bool)
        {
            loopCount++;
            CreateDomino_Horizontal();
        }
        else
        {
            allDominoCameraController.SetCameraPos(minX, minZ, maxX, maxZ);
        }
    }


    public void NextCreate()
    {
        loopCount++;
        floorCreateCount++;
        if (floorCreateCount == 2)
        {
            CreateFloor();
            floorCreateCount = 0;
        }
        CreateDomino_Horizontal();
    }

    void CreateFloor()
    {
        Instantiate(dominoFloor, floorCreatePos, Quaternion.identity);
        floorCreatePos.x += 500.0f;
    }
    //// いつかは作りたい模様のようなドミノの形
    /*
    void DominoEvent_Square(int startNum)
    {
        int createLineNum = 1;
        for (int i = startNum; i < startNum + createRowNum; i++)
        {
            GameObject obj;
            for (int j = 0; j < createLineNum; i++)
            {
                obj = Instantiate(domino_obj, new Vector3(createstartPos_X_X + (i * createSpace), 1.25f, 0), Quaternion.identity);
            }
            obj = Instantiate(domino_obj, new Vector3(createstartPos_X_X + (i * createSpace), 1.25f, 0), Quaternion.identity);
            if (i % colorChangeNum == 0)
            {
                createColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }
            else if (i % colorChangeNum == colorChangeNum - 1)
            {
                obj.AddComponent<DominoHitManager>();
            }
            obj.GetComponent<Renderer>().material = dominoMaterial;
            obj.GetComponent<Renderer>().material.color = createColor;
            if (i == 0)
            {
                obj.AddComponent<DominoStart>();
            }
            if (i == createNum - 1)
            {
                obj.AddComponent<LastDomino>();
            }
        }
    }
    */
}
