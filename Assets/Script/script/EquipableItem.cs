using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;
    internal bool swingwait = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            InventorySystem.Instance.isOpen == false &&
            CraftingSystem.instance.isOpen == false &&
            SelectionManager.instance.handIsVisible == false &&
            swingwait == false) { //tombol kiri mouse

            swingwait = true;
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");
            StartCoroutine(NewSwingDelay());
        }
    }

    public void GetHit() {
        GameObject selectedTree = SelectionManager.instance.selectedTree;
        if (selectedTree != null)
        {
            SoundSystem.instance.PlaySound(SoundSystem.instance.chopSound);
            selectedTree.GetComponent<TebangPohon>().GetHit();
        }
    }

    IEnumerator SwingSoundDelay() {
        yield return new WaitForSeconds(0.2f);
        SoundSystem.instance.PlaySound(SoundSystem.instance.toolsSound);
    }

    IEnumerator NewSwingDelay() {
        yield return new WaitForSeconds(1f);
        swingwait = false;
    
    }
    
}
