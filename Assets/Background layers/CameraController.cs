using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player_pos;
    public float lookAhead;
    public float cameraFollowSpeed;
    float camerafocus;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") < -0.01)
            camerafocus = player_pos.position.x- lookAhead;
        else if(Input.GetAxis("Horizontal") > 0.01)
            camerafocus = player_pos.position.x+ lookAhead;
        else
            camerafocus = player_pos.position.x;

        if ((transform.position.x - camerafocus > 4.0f || transform.position.x - camerafocus < -1.0f)) // camera movement if player on edge of defined rectangle
        {
            transform.position = new Vector3(transform.position.x + cameraFollowSpeed * (-transform.position.x + camerafocus) / 4 * Time.deltaTime, transform.position.y, transform.position.z);

        }
    }
}
