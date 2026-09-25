using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
   
    public static PlayerState Instance { get; set; }

    // --- palyer health --- // 
    public float currentHealth;
    public float maxHealth;

    // --- player calories ---- //
    public float currentCalories;
    public float maxCalories;


    // --- player hydration --- // 
    public float currentHydration;
    public float maxHydration;

    public bool isPlayerDead;
    public RespawnLocation registerRespawnLocation;
    public event Action onRespawnRegistered;

    public bool isHydrationActive; //bisa dihapus 

    public AudioSource playerAudioSource;
    public AudioClip playerPain;
    public AudioClip playerDeath;

    private float hurtSoundDelay = 2f;
    private float hurtTime = 0f;

    float distanceTravelled = 0;
    Vector3 lastPosition; 

    public GameObject playerBody;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
    }

    public void SetCalories(float value)
    {
        currentCalories = Mathf.Clamp(value, 0, maxCalories);
    }

    public void SetHydration(float value)
    {
        currentHydration = Mathf.Clamp(value, 0, maxHydration);
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydration = maxHydration;

        StartCoroutine(decreaseHydration());
    }

    IEnumerator decreaseHydration() {
        while (true) {
            currentHydration -= 1;
            yield return new WaitForSeconds(10);
        }
    
    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if (distanceTravelled >= 5) { 
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        if (Input.GetKeyDown(KeyCode.N)) { //test darah 
            currentHealth -= 10 ;
            
        }
    }

    public void menerimaDMG(int damage) { 
        currentHealth -= damage;

        if (currentHealth <= 0 && !isPlayerDead )
        {
            Debug.Log("Player mati");
            playerDead();
        }
        else {
            if (currentHealth > 0 && Time.time >= hurtTime) {
                playerAudioSource.PlayOneShot(playerPain);
                Debug.Log("Player terluka");

                hurtTime = Time.time + hurtSoundDelay;
            }
        }
    }

    public void playerDead() {
        isPlayerDead = true;
        playerAudioSource.PlayOneShot(playerDeath);
        respawn();
    }

    public void respawn() {

        StartCoroutine(respawnCoroutine());
    }


    public IEnumerator respawnCoroutine() { 

        playerBody.GetComponent<PlayerMovement>().enabled = false;
        playerBody.GetComponent<CameraMovement>().enabled = false;

        if (registerRespawnLocation != null) {
            Vector3 posisi = registerRespawnLocation.transform.position;

            posisi.y += 5f;// diatas
            posisi.z += 5f; //disamping

            playerBody.transform.position = posisi; //respawn player
            currentHealth = maxHealth;
        }

        yield return new WaitForSeconds(0.2f);
        isPlayerDead = false;

        playerBody.GetComponent<PlayerMovement>().enabled = true;
        playerBody.GetComponent<CameraMovement>().enabled = true;
    }

    internal void SetRegisteredLocation(RespawnLocation respawnLocation)
    {
        registerRespawnLocation = respawnLocation;
        onRespawnRegistered?.Invoke();
    }
}
