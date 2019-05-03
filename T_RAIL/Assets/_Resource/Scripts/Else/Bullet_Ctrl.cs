﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Ctrl : MonoBehaviour {

    [SerializeField]
    Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();

    }

    public void CallMoveCoroutin()
    {
        StartCoroutine(MoveBullet());
    }

	IEnumerator MoveBullet()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;

            if (timer > 2)
                break;
            rig.AddForce(transform.forward *Time.deltaTime* GameValue.bullet_speed, ForceMode.Impulse);

            yield return null;

        }
        gameObject.SetActive(false);
    }
}
