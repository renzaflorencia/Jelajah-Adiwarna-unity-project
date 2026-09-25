using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] float currentHealt;
    [SerializeField] float maxHealth;

    [Header("Suara")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip AnimalScream;
    [SerializeField] AudioClip AnimalDie;
    [SerializeField] AudioClip animalAttack;

    private Animator animator;
    public bool isDead;

    public Slider healthBarSlider;

    enum AnimalType { 
        rabbit, 
        Bear
    }

    [SerializeField] AnimalType thisAnimalType;


    private void Start()
    {
        currentHealt = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage) {
        if (isDead == false) {
            currentHealt -= damage;
            healthBarSlider.value = currentHealt / maxHealth;

            if (currentHealt <= 0)
            {
                Debug.Log("Bear mati");
                PlayDyingSound();
                animator.SetTrigger("DIE");
                isDead = true;
            }
            else
            {
                PlayHitSound();
                animator.SetTrigger("HURT");

            }
        }
    }

    private void PlayDyingSound (){
        soundChannel.PlayOneShot(AnimalDie);
    }

    private void PlayHitSound() {

        soundChannel.PlayOneShot(AnimalScream);
    }

    public void playAttackShound()
    {

        soundChannel.PlayOneShot(animalAttack);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            healthBarSlider.gameObject.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            healthBarSlider.gameObject.SetActive(false);
        }
    }
}
