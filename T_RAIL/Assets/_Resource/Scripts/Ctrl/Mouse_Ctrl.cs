﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Mouse_Ctrl : MonoBehaviourPunCallbacks
{

    // 마우스 컨트롤.
    // 말그대로 마우스로 클릭해서 하는 것들 관리
    // UI 제외

  //  float ScreenWidth;
  //  float ScreenHeight;

    public GameObject Inventory;
    public GameObject ChoiceButton;

    public GameObject StateControllerCam;
    public GameObject ExitStateController; // 일단 ㅇ녀기다가 갖다놨음 



    //   layerMask = (1 << LayerMask.NameToLayer("Furniture")); 

    private void Start()
    {
        
       // ScreenWidth = Screen.width;
       // ScreenHeight = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        // 근데 maincamera에 tag가 maincamera로 안달려있어서
        // 계속 null 오류가 떴었음

        // 그러면 결국 maincamera에 태그 달아줬는데
        // 이 ray가 카메라를 태그로 인식하는거면
        // 얘도 결국 태그연산 아니야?

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 만약에 마우스 클릭이 안된다?
            // max distance 200.0f을 mathf.infinity로 바꿔볼것
            //if (Physics.Raycast(ray, out hit))
            //{
            //   // Debug.Log(hit.collider.gameObject.layer);
            //}
            if (Physics.Raycast(ray, out hit))
            {
                // 아... Equals("12") 라고 해서 계속 안됐던거임 흑흑 젠장
                if (hit.collider.gameObject.layer.Equals(GameValue.itembox_layer))
                {
                    // 상자일 경우!
                    Inventory.SetActive(true);
                    // Vector3 m_Position = Input.mousePosition;

                    Inventory.transform.position = Input.mousePosition;
                    //new Vector3(m_Position.x, m_Position.y, m_Position.z);
                }

                else if (hit.collider.gameObject.layer.Equals(GameValue.passenger_layer))
                {
                    // 승객일 경우 
                }
                else if (hit.collider.gameObject.layer.Equals(GameValue.choice_layer))
                {
                    ChoiceButton.SetActive(true);
                    ChoiceButton.transform.position = Input.mousePosition;
                    //ChoiceButton.GetComponent<UI_ChoiceButton>().GetHitObject(hit.collider.gameObject);
                    Debug.Log(hit.collider.gameObject.name + "  dkdkdkkdkdk   ");// + hit.collider.transform.root.gameObject.name);
                    photonView.RPC("getHitObjectRPC", RpcTarget.AllBuffered, hit.collider.gameObject.GetPhotonView().ViewID);
                }

                else if (hit.collider.gameObject.layer.Equals(GameValue.sofa_layer))
                {
                    // 소파레이어
                    //ChoiceButton.SetActive(true);
                   // ChoiceButton.transform.position = Input.mousePosition;
                   // ChoiceButton.GetComponent<UI_ChoiceButton>().GetHitObject(hit.collider.gameObject);

                }

                else if (hit.collider.CompareTag("state"))
                {
                    // headtrain의 state판을 클릭하면
                    Debug.Log("ㅑㅜ");
                    this.GetComponent<Camera>().enabled = false;
                    StateControllerCam.GetComponent<Camera>().enabled = true;
                    ExitStateController.SetActive(true);
                }

                
            }
        }
    }

    [PunRPC]
    public void getHitObjectRPC(int hit_object_viewID)
    {
        ChoiceButton.transform.parent.GetComponent<UI_ChoiceButton>().GetHitObject(PhotonView.Find(hit_object_viewID).gameObject);
    }


    public void Exit_StateController()
    {
        this.GetComponent<Camera>().enabled = true;
        StateControllerCam.GetComponent<Camera>().enabled = false;
        ExitStateController.SetActive(false);
    }

}
