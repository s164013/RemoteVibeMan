using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoCollisionController : MonoBehaviour {

    private DominoFollowObject dominoFollowObject;
    [SerializeField]
    private AudioSource hitDominoSE;
    [SerializeField]
    private AudioClip[] hitDominoSE_clip;

    // Use this for initialization
    void Start () {
        dominoFollowObject = GameObject.Find("DominoFollowObject").GetComponent<DominoFollowObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Domino")
        {
            //int rand = Random.Range(1, hitDominoSE_clip.Length);
            hitDominoSE.PlayOneShot(hitDominoSE_clip[hitDominoSE_clip.Length - 1]);
            dominoFollowObject.FollowDominoObjectUpdate(transform.position);
        }
    }
}
