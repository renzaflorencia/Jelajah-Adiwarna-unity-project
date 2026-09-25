using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TebangPohon : MonoBehaviour
{
    public bool playerInRange;
    public bool bisaTebang;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator animator;

    public float kaloriKeluar = 20; 

    private void Start()
    {
        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

        }
    }

    public void GetHit() {
        animator.SetTrigger("shake");
        treeHealth -= 1;
        PlayerState.Instance.currentCalories -= kaloriKeluar;


        if (treeHealth <= 0)
        {
            TreeIsDead();
        }
    }
    

    void TreeIsDead() { 
        Vector3 treePosisition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        bisaTebang = false;
        SelectionManager.instance.selectedTree = null;
        SelectionManager.instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosisition.x, treePosisition.y+1.5f, treePosisition.z+0.5f), Quaternion.Euler(0,0,0));

        brokenTree.transform.SetParent(transform.parent.transform.parent.transform.parent);

        //brokenTree.GetComponent<RegrowTree>.dayOfRegrowth = TimeManager.Instance.dayInGame + 2;
    }
    private void Update()
    {
        if (bisaTebang) { 
            GlobalState.instance.resourceHealth = treeHealth;
            GlobalState.instance.resourceHealthMax = treeMaxHealth;
        }
    }
}
