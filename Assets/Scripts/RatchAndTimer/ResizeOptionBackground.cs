using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeOptionBackground : MonoBehaviour
{
    [SerializeField]
    private RectTransform optionCanvas;
    private RectTransform thisRect;

    // Start is called before the first frame update
    void Start()
    {
        thisRect = GetComponent<RectTransform>();
        thisRect.sizeDelta = optionCanvas.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
