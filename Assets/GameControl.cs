using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Start is called before the first frame update
    private bool paused = false;
    public GameObject inventory; 
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (paused)
            {
                Time.timeScale = 1;
                inventory.SetActive(false);
                paused = false;
            }
            else 
            { 
                Time.timeScale = 0;
                inventory.SetActive(true);
                paused = true;
            }
        }
    }
}
