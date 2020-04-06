using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDominoCameraController : MonoBehaviour {

    private Camera dominoCamera;

    private float radius;
    private const float MARGIN = 4.0f;

    // Use this for initialization
    void Start () {
        dominoCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetCameraPos(float minX, float minZ, float maxX, float maxZ)
    {
        radius = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(minX) + Mathf.Abs(maxX), 2) + Mathf.Pow(Mathf.Abs(minZ) + Mathf.Abs(maxZ), 2)) * 0.5f; //(endPos - startPos) * 0.5f;
        float distance = ((radius + MARGIN) / Mathf.Sin(dominoCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        float halfX = (Mathf.Abs(maxX) - Mathf.Abs(minX)) * 0.5f;
        float halfZ = (Mathf.Abs(minZ) - Mathf.Abs(maxZ)) * 0.5f;
        Debug.Log("Rad : " + Mathf.Sin(dominoCamera.fieldOfView * 0.5f * Mathf.Deg2Rad).ToString());
        Debug.Log("radius : " + radius.ToString());
        Debug.Log("distance : " + distance.ToString());
        Debug.Log("min : (" + minX.ToString() + ", " + minZ.ToString() + ")");
        Debug.Log("max : (" + maxX.ToString() + ", " + maxZ.ToString() + ")");
        float posX = radius;
        transform.position = new Vector3(halfX + 4, distance, halfZ - (distance * 0.5f));
    }
}
