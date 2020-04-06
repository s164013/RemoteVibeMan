using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateDominoFollowObject : MonoBehaviour {

    [SerializeField]
    private Transform target_tf;

    private RectTransform rectTransform;
	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target_tf.position);
    }
}
