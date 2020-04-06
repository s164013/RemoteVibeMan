using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHitJudge : MonoBehaviour {

    public static int CreateNum;

    [SerializeField]
    private DebugScript debugScript;

    private int hitNum;

	// Use this for initialization
	void Start () {
        hitNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sphere")
        {
            hitNum++;
            if(hitNum == CreateNum)
            {
                debugScript.MaxDischarge();
            }
        }
    }
}
