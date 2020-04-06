using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V))
        {
                SerialManager.Write("9");
                Debug.Log("9");
        }
    }
}
