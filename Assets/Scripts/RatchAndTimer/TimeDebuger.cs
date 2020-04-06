using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDebuger : MonoBehaviour {

    private DominoTimerManager dominoTimerManager;

	// Use this for initialization
	void Start () {
        dominoTimerManager = GameObject.Find("ScriptObject").GetComponent<DominoTimerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Domino")
        {
            dominoTimerManager.HitChangeColorDomino();
        }
    }
}
