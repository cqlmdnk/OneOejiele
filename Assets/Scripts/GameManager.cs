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
    public GameObject zombie, tankZombie;
    public Transform zombieLeftSpawn, zombieRightSpawn;
    public Button archerButton, banditButton, wizardButton;
    HealthController kingHealthController;
    private float enemySpawnTime = 3f;
    [SerializeField]
    private bool enemySpawnCooledDown = true;
    void Awake()
    {

        Time.timeScale = 1;
        kingHealthController = GameObject.FindGameObjectWithTag("King").GetComponent<HealthController>();
        archerButton.onClick.AddListener(delegate { InstantiateArcher(); });
        banditButton.onClick.AddListener(delegate { InstantiateBandit(); });
        wizardButton.onClick.AddListener(delegate { InstantiateWizard(); });
        paused = true;
    }

    // Update is called once per frame
    void Update()
    {
        
         if(IsSpawnCooledDown())
            HandleNewEnemies();
         
        IsGameOver();
        HandleUI();
        if (!paused)
        {
            GameObject.Find("CoinCount").GetComponent<CoinCounter>().enabled = true;
        }
    
    }

    private void HandleUI()
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
        else if (Input.GetKeyDown(KeyCode.Tab))
        {


            if (!paused)
            {


                DestroyImmediate(GameObject.FindGameObjectWithTag("Player"));
                characterSelection.SetActive(true);
                Time.timeScale = 0;

                paused = true;
            }
        }
    }

    private void IsGameOver()
    {
        if (kingHealthController.GetHealth() < 0f)
        {
            if (!paused)
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

    void HandleNewEnemies()
    {
        int creationChance = Random.Range(0, 100);
        if(creationChance >= 95)
        {
            int direction = Random.Range(0, 2);

            if(direction == 0)
            {
                InstantiateNewEnemy(zombieLeftSpawn);
            }
            else
            {
                InstantiateNewEnemy(zombieRightSpawn);
            }
        }
    }
    void InstantiateNewEnemy(Transform spawnPoint)
    {

        int kind = Random.Range(0, 2);
        if (kind == 0)
        {
            Instantiate(zombie, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {

            Instantiate(tankZombie, spawnPoint.position, spawnPoint.rotation);
        }
        enemySpawnCooledDown = false;
        StartCoroutine(HandleSpawnTimer());
    }

    IEnumerator HandleSpawnTimer()
    {
        yield return new WaitForSeconds(enemySpawnTime);
        enemySpawnCooledDown = true;
        yield break;
    }

   bool IsSpawnCooledDown()
    {
        return enemySpawnCooledDown;
    }
}
