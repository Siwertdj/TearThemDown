using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LogicManager : MonoBehaviour
{
    [SerializeField] EntitySpawner spawner;
    [SerializeField] int entitiesToSpawn;
    public int entitiesToWin;
    [SerializeField] TextMeshProUGUI popCounter;

    [SerializeField] Camera _camera;

    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] GameObject player;
    private CharacterController playerController;
    private AudioSource[] _screamers;
    private CrowdManager _crowdManager;
    public int currentPopulation;

    private float screamTimer;
    private float screamDelay = 1.0f;
    private int lastPopulation;

    private void Awake()
    {
        _crowdManager = FindObjectOfType<CrowdManager>();
        _crowdManager.startNumber = entitiesToSpawn;
    }

    void Start()
    {
        spawner.entitiesToSpawn = entitiesToSpawn;
        spawner.Spawn();
        
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        

        playerController = player.GetComponent<CharacterController>();
        _screamers = player.transform.GetChild(0).gameObject.GetComponents<AudioSource>();
        screamTimer = 0;

        // TODO: Set player health and speed here somewhere
    }

    // Update is called once per frame
    void Update()
    {
        // find no. of entities
        currentPopulation = _crowdManager.number;
        
        popCounter.text = currentPopulation.ToString();
        
        // if anyone died in the last (currently set to) 1 second(s), Scream.
        if (screamTimer >= screamDelay)
        {
            int amountDied = lastPopulation - currentPopulation;
            if (amountDied > 0)
            {
                Scream(amountDied);
            }
            
            // update the last-tracked population size and reset timer
            screamTimer = 0.0f;
            lastPopulation = currentPopulation;
        }
        
        if (currentPopulation <= 0)
        {
            GameOver();
        }
        
        
        UpdateCamera();
        HandleInput();

        screamTimer += Time.deltaTime;
    }

    
    void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        playerController.enabled = false;
    }

    public void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        playerController.enabled = true;
    }

    void GameOver()
    {
        // Disable controls
        playerController.enabled = false;

        // set inactive othermenu
        winMenu.SetActive(false);
        pauseMenu.SetActive(false);

        // set active losemenu
        loseMenu.SetActive(true);

    }

    public void GameWon()
    {

        // Disable controls
        playerController.enabled = false;

        // set inactive othermenu
        loseMenu.SetActive(false);
        pauseMenu.SetActive(false);

        // set active winmenu
        winMenu.SetActive(true);
    }
    
    void UpdateCamera()
    {

        float offsetY = 0; // this will offset the cameraposition

        switch (currentPopulation)
        {
            case var expression when (currentPopulation <= 300):
                offsetY = 45;
                break;
            case var expression when (currentPopulation > 300 && currentPopulation <= 500):
                offsetY = 55;
                break;
            case var expression when (currentPopulation > 500 && currentPopulation <= 1000):
                offsetY = 70;
                break;
            case var expression when (currentPopulation > 1000):
                offsetY = 80;
                break;
        }

        _camera.GetComponent<CameraBehavior>().offset.y = offsetY + 0;
    }

    void Scream(int amountDied)
    {
        // we choose an amount of randomly-selected scream-sounds to play,
        // based on the amount of people that died since the last interval.
        
        int amountToScream = 0;
        
        switch (amountDied)
        {
            case int n when (n >= 40):
                amountToScream = 5;
                break;
            case int n when (n >= 30):
                amountToScream = 4;
                break;
            case int n when (n < 30 && n >= 20):
                amountToScream = 3;
                break;
            case int n when (n < 20 && n >= 10):
                amountToScream = 2;
                break;
            case int n when (n < 10 && n >= 5):
                amountToScream = 1;
                break;
            default:
                // amountToScream = 0;
                break;
        }

        // add indices 0 .. 4 to a list of ints
        List<int> screamsNotSelected = new List<int>();
        screamsNotSelected.Add(0);
        screamsNotSelected.Add(1);
        screamsNotSelected.Add(2);
        screamsNotSelected.Add(3);
        screamsNotSelected.Add(4);

        // we choose a random amount of screams,  equal to the amountToScream-value
        for (int i = 0; i < amountToScream; i++)
        {
            // the index, after selection, is removed from the screamsNotSelected list,
            // so that it cannot be selected again.
            int index = Random.Range(0, screamsNotSelected.Count);
            // we play the selected sound immediately.
            _screamers[index].Play();
            screamsNotSelected.Remove(index);
        }
        
    }

}
