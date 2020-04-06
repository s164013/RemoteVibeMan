using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDomino : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
       /* if (collision.gameObject.tag == "Domino")
        {
            GameObject obj = GameObject.Find("ScriptObject");
            //obj.GetComponent<DominoTimeManager>().EndDomino();
            obj.GetComponent<CreateDomino>().NextCreate();
        }*/

    }
}
