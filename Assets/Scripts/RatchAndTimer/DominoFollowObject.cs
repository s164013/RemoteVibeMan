using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoFollowObject : MonoBehaviour {

    private Vector3 thisPos;
    private bool updatePos_bool;

	// Use this for initialization
	void Start () {
        updatePos_bool = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(updatePos_bool)
        {
            transform.position = thisPos;
            updatePos_bool = false;
        }
	}

    public void FollowDominoObjectUpdate(Vector3 dominoPos)
    {
        thisPos = dominoPos;
        thisPos.y = 3.0f;
        updatePos_bool = true;
    }
}
