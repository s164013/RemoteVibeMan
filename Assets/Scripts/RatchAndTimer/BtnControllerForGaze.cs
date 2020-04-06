using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BtnControllerForGaze : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image gazeCircle;

    private float time;
    private float circleScale;
    private Animator anim;
    private Button thisBtn;
    private bool mouseOver_bool;
    private bool delayFlag_bool;
    private Vector3 gazeCircleScale;
    // Use this for initialization
    void Start () {
        circleScale = 0;
        thisBtn = GetComponent<Button>();
        anim = GetComponent<Animator>();
        mouseOver_bool = false;
        delayFlag_bool = false;
        thisBtn.onClick.AddListener(Clicked);
        gazeCircleScale = gazeCircle.transform.localScale;
    }

    // Update is called once per frame
    void Update () {
		if(mouseOver_bool && !delayFlag_bool)
        {
            if (!gazeCircle.enabled)
            {
                gazeCircle.enabled = true;
            }
            time += Time.deltaTime;
            circleScale = gazeCircleScale.x - (time * gazeCircleScale.x);
            gazeCircle.transform.localScale = new Vector3(circleScale, circleScale);
            if (time >= StaticVariableField.GazeTime)
            {
                anim.SetTrigger("Pressed");
                thisBtn.onClick.Invoke();
                StartCoroutine("Delay_AnimClecked");
            }
        }
	}
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver_bool = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver_bool = false;
        gazeCircle.transform.localScale = gazeCircleScale;
        gazeCircle.enabled = false;
        if (gazeCircle.enabled)
        {
            gazeCircle.enabled = false;
        }
        time = 0;
    }

    void Clicked()
    {
        time = 0;
    }

    IEnumerator Delay_AnimClecked()
    {
        time = 0;
        delayFlag_bool = true;
        yield return new WaitForSeconds(0.2f);
        anim.SetTrigger("Highlighted");
        yield return new WaitForSeconds(0.8f);
        delayFlag_bool = false;
    }
}
