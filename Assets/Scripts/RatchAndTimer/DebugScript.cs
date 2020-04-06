using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

    private const int CREATENUM_PAR_SECOND = 2;

    [SerializeField]
    private GameObject testObject_obj;
    [SerializeField]
    private int createNum;
    [SerializeField]
    private Text debugText_txt;
    [SerializeField]
    private int settingTime;

    private bool time_bool;
    private float posY = 4.90f;
    private float time;
    private float nowTime;
    private GameObject lastObject_obj;
    private Rigidbody lastObject_rb;
    private float buffY;

    // Use this for initialization
    void Start () {
        time = 0.0f;
        nowTime = 0;
        time_bool = false;
        buffY = 0.5f * Mathf.Abs(Physics.gravity.y) * Mathf.Pow(settingTime, 2);
        //Debug.Log("buffY : " + buffY.ToString());
        createNum = settingTime * CREATENUM_PAR_SECOND;
        SphereHitJudge.CreateNum = createNum;
        float f = 0.3f;
        //GameObject obj = Instantiate(testObject_obj, new Vector3(0, buffY, 0), Quaternion.identity);
        for(int i = 0; i < createNum; i++)
        {
            float randX = 0;
            float randZ = 0;
            if (i < 73)
            {
                randX = Random.Range(-0.7f, 0.7f);
                randZ = Random.Range(-0.7f, 0.7f);
            }
            
            if(i > 73 && i < 100)
            {
                randX = Random.Range(-2f, 2f);
                randZ = Random.Range(-2f, 2f);
            }
            else if (i > 100)
            {
                randX = Random.Range(-4f, 4f);
                randZ = Random.Range(-4f, 4f);
            }
            GameObject obj = Instantiate(testObject_obj, new Vector3(randX, posY, randZ), Quaternion.identity);
            posY += f;
            if(i == createNum - 1)
            {
                lastObject_obj = obj;
                lastObject_rb = lastObject_obj.GetComponent<Rigidbody>();
            }
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        if(time_bool)
        {
            time += Time.deltaTime;
            debugText_txt.text = time.ToString() + "s";
            GameObject obj = Instantiate(testObject_obj, new Vector3(0, buffY, 0), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject obj = Instantiate(testObject_obj, new Vector3(0, posY, 0), Quaternion.identity);
            obj.transform.eulerAngles = new Vector3(90, 0, 0);
            posY += 0.25f;
        }
    }

    public void MaxDischarge()
    {
        time_bool = false;
    }

    public void StartShot()
    {
        time_bool = true;
    }
}
