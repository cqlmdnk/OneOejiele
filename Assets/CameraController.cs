using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player_pos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x - player_pos.position.x > 4.0f || transform.position.x - player_pos.position.x < -1.0f)) // camera movement if player on edge of defined rectangle
        {
            transform.position = new Vector3(transform.position.x + 5 * (-transform.position.x + player_pos.position.x) / 4 * Time.deltaTime, transform.position.y, transform.position.z);

        }
    }
}
