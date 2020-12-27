﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player_pos;
    public float lookAhead;
    public float cameraFollowSpeed;
    float camerafocus;
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_pos == null && GameObject.FindGameObjectWithTag("Player") != null)
            player_pos = GameObject.FindGameObjectWithTag("Player").transform;
        else if(GameObject.FindGameObjectWithTag("Player") == null)
            return;
        if (Input.GetAxis("Horizontal") < -0.01)
            camerafocus = player_pos.position.x- lookAhead;
        else if(Input.GetAxis("Horizontal") > 0.01)
            camerafocus = player_pos.position.x+ lookAhead;
        else
            camerafocus = player_pos.position.x;

        if ((transform.position.x - camerafocus > 1.0f || transform.position.x - camerafocus < -1.0f)) // camera movement if player on edge of defined rectangle
        {
            transform.position = new Vector3(transform.position.x + cameraFollowSpeed * (-transform.position.x + camerafocus) / 4 * Time.deltaTime, transform.position.y, transform.position.z);

        }
    }
}
