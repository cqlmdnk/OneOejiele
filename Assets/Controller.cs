using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0.0f);
        cam.transform.position = new Vector3(cam.transform.position.x + 3 * move.x * Time.deltaTime, cam.transform.position.y, cam.transform.position.z);
    }
}
