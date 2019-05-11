﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Map_Ctrl : MonoBehaviourPunCallbacks
{

    public Transform[] Mountain1;
    public Transform[] Cloud;
    public Transform[] Mountain2;
    public Transform[] plane;
    public Transform[] RailRoad;

    public playerListController_minj playerListCtrl;

    // 초기 포지션
    Vector3 init_mountain1;
    Vector3 init_mountain2;
    Vector3 init_cloud;
    Vector3 init_plane;
    Vector3 init_railroad;

    List<Transform> map_object = new List<Transform>();

    float map_speed; // 맵이 움직이는 스피드

    int[] on_objectindex;
    Vector3[] Object_InitPosition; // 중요! 순서는 저장된 순서대로임!

    void Start()
    {
        // 빈 오브젝트 아래에 있는 움직이는 오브젝트들을 연결
        // 찾은 오브젝트들을 리스트에 넣기
        MapObject_AddList();
        // 찾은 오브젝트들 중 1번째 (기차가 달려가는 방향쪽에 있는 게 1번) 오브젝트들의 초기 위치값 찾기
        //MapObject_InitPosition();

        on_objectindex = new int[map_object.Count / 2]; // 둘중에 어떤게 true 상태인지
    }


    void MapObject_InitPosition()
    {
        Object_InitPosition = new Vector3[map_object.Count];

        Object_InitPosition[0] = Mountain1[0].position;
        Object_InitPosition[1] = Mountain2[0].position;
        Object_InitPosition[2] = Cloud[0].position;
        Object_InitPosition[3] = plane[0].position;
        Object_InitPosition[4] = RailRoad[0].position;

    }
    void MapObject_AddList()
    {
        // 저장된 순서 중요.
        // 현재 카메라로부터 먼거리 순서대로 저장해놨음s
        map_object.Add(Mountain1[0]);
        map_object.Add(Mountain1[1]);
        map_object.Add(Cloud[0]);
        map_object.Add(Cloud[1]);
        map_object.Add(Mountain2[0]);
        map_object.Add(Mountain2[1]);
        map_object.Add(plane[0]);
        map_object.Add(plane[1]);
        map_object.Add(RailRoad[0]);
        map_object.Add(RailRoad[1]);
    }
    void MapObject_PositionChange()
    {
        for (int i = 0; i < map_object.Count; i += 2)
        {
            if (on_objectindex[i / 2].Equals(0))
            {
                // 첫번째꺼가 true
                if (map_object[i].localPosition.z >= 800.0f)
                {
                    map_object[i + 1].localPosition = new Vector3(map_object[i + 1].localPosition.x,
                        map_object[i + 1].localPosition.y, 0);
                    map_object[i + 1].gameObject.SetActive(true);
                    map_object[i].gameObject.SetActive(false);
                    on_objectindex[i / 2] = 1;
                }


            }
            else if (on_objectindex[i / 2].Equals(1))
            {
                // 두번째꺼가 true인 상태에서
                if (map_object[i + 1].localPosition.z >= 800.0f)
                {
                    map_object[i].localPosition = new Vector3(map_object[i].localPosition.x,
                        map_object[i].localPosition.y, 0);
                    map_object[i].gameObject.SetActive(true);
                    map_object[i + 1].gameObject.SetActive(false);
                    on_objectindex[i / 2] = 0;
                }

            }
        }

    }

    void Update()
    {
        if (playerListCtrl.playerList[PhotonNetwork.LocalPlayer.ActorNumber - 1].player.Where_Floor != 4)
        {
            // 맵의 스피드는 항상 기차의 스피드를 받아오고 있다
            map_speed = TrainGameManager.instance.Speed * TrainGameManager.instance.Speed_stat;

            for (int i = 0; i < map_object.Count; i += 2)
            {
                // Mountain1이 제일 멀리 있으니까 제일 느리게 움직여야 해.

                // 그리고 기차 멈출 때 속도 자연스럽게 감소하는것도 하고싶은데
                // 그럴거면 mathf 써야 되는데
                if (on_objectindex[i / 2].Equals(0)) // 첫번째꺼가 true
                {
                    map_object[i].localPosition += new Vector3(0, 0, (map_speed / 20.0f) * ((i + 1) * Time.deltaTime * 2.0f));
                }
                else if (on_objectindex[i / 2].Equals(1))  // 두번째꺼가 true
                {
                    map_object[i + 1].localPosition += new Vector3(0, 0, (map_speed / 20.0f) * ((i + 1) * Time.deltaTime * 2.0f));
                }
            }

            MapObject_PositionChange();


            // 휠애니메이션의 속도 
            // 이거 아예 trainctrl로 옮길거임
            // TrainCtrl.Wheel_Animation_Speed();
        }
    }
}

// iTween이 과부하가 심하대서 cloud에 iTWeen 뺐음 4/1