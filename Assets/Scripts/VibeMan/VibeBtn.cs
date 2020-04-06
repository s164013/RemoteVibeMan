using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibeBtn : Photon.MonoBehaviour {

    private Color ONCOLOR = new Color(1, 1, 1);
    private Color OFFCOLOR = new Color(0.3f, 0.3f, 0.3f);

    [SerializeField]
    private Image LampImage;
    [SerializeField]
    private Image LampImage_Btn;
    [SerializeField]
    private Image LampImage_8;
    [SerializeField]
    private Image LampImage_Btn_8;
    [SerializeField]
    private PhotonView m_photonView;
    [SerializeField]
    private AudioSource vibeSE;
    private bool vibe;
    

    // Use this for initialization
    void Start () {
        vibe = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!PhotonNetwork.isMasterClient)
        {
            switch (VibeManStates.WriteNumber)
            {
                case "9":
                    {
                        if (!vibe)
                        {
                            SerialManager.Write("9");
                            Debug.Log("Vibe!!");
                            vibe = true;
                        }
                        StartCoroutine("Delay_Client_Write9");
                        VibeManStates.WriteNumber = "";
                        m_photonView.RPC("ResetWriteNumber", PhotonTargets.MasterClient);
                        break;
                    }
                case "8":
                    {
                        if (!vibe)
                        {
                            SerialManager.Write("8");
                            Debug.Log("Vibe!!");
                            vibe = true;
                        }
                        StartCoroutine("Delay_Client_Write8");
                        VibeManStates.WriteNumber = "";
                        m_photonView.RPC("ResetWriteNumber", PhotonTargets.MasterClient);
                        break;
                    }
            }
        }
        /*else if(PhotonNetwork.isMasterClient)
        {
            if(Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                ClickedVibeBtn();
            }
        }*/
	}

    public void ClickedVibeBtn()
    {
        if(!vibe && PhotonNetwork.isMasterClient)
        {
            VibeManStates.WriteNumber = "9";
            StartCoroutine("Delay");
        }
    }

    public void ClickedVibeBtn8()
    {
        if(!vibe && PhotonNetwork.isMasterClient)
        {
            VibeManStates.WriteNumber = "8";
            StartCoroutine("Delay_Write8");
        }
    }

    IEnumerator Delay()
    {
        LampImage_Btn.color = ONCOLOR;
        vibeSE.Play();
        yield return new WaitForSeconds(0.5f);
        LampImage_Btn.color = OFFCOLOR;
        vibe = false;
    }
    IEnumerator Delay_Write8()
    {
        LampImage_Btn_8.color = ONCOLOR;
        vibeSE.Play();
        yield return new WaitForSeconds(0.5f);
        LampImage_Btn_8.color = OFFCOLOR;
        vibe = false;
    }
    IEnumerator Delay_Client_Write9()
    {
        LampImage.color = ONCOLOR;
        vibeSE.Play();
        yield return new WaitForSeconds(0.5f);
        LampImage.color = OFFCOLOR;
        vibe = false;

    }
    IEnumerator Delay_Client_Write8()
    {
        LampImage_8.color = ONCOLOR;
        vibeSE.Play();
        yield return new WaitForSeconds(0.5f);
        LampImage_8.color = OFFCOLOR;
        vibe = false;

    }

}
