using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private bool paused;
    public GameObject inventory;
    public GameObject characterSelection;
    public GameObject archer, bandit, wizard;
    public Button archerButton, banditButton, wizardButton;
    void Start()
    {
        Time.timeScale = 1;
        archerButton.onClick.AddListener(delegate { InstantiateArcher(); });
        banditButton.onClick.AddListener(delegate { InstantiateBandit(); });
        wizardButton.onClick.AddListener(delegate { InstantiateWizard(); });
        paused = true;
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
                
            }
            else 
            { 
                Time.timeScale = 0;
                inventory.SetActive(true);
                paused = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {

            
            if(!paused)
            {
                

                DestroyImmediate(GameObject.FindGameObjectWithTag("Player"));
                characterSelection.SetActive(true);
                Time.timeScale = 0;
                
                paused = true;
            }
        }
    }
    void InstantiateArcher()
    {
        Instantiate(archer,new Vector3(0, 0, 2), Quaternion.identity);
        characterSelection.SetActive(false);
        characterSelection.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
    void InstantiateBandit()
    {
        Instantiate(bandit, new Vector3(0, 0, 2), Quaternion.identity);
        characterSelection.SetActive(false);
        characterSelection.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
    void InstantiateWizard()
    {
        Instantiate(wizard, new Vector3(0, 0, 2), Quaternion.identity);
        characterSelection.SetActive(false);
        characterSelection.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }
}
