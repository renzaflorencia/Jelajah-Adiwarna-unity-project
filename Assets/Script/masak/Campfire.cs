using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{

    public bool playerInRange;
    public bool isCooking;
    public float waktuMasak;
    public CookableFood bahan; // food being cooked
    public string makananJadi;
    public GameObject fire;
    
    private void Update()
    {
        float jarak = Vector3.Distance(PlayerState.Instance.playerBody.transform.position, transform.position);

        if (jarak < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (isCooking)
        {
            waktuMasak -= Time.deltaTime;
            fire.SetActive(true);
        }
        else { 
            fire.SetActive(false);
        }
        if (waktuMasak <= 0 && isCooking)
        {
            isCooking = false;
            makananJadi = mendapatkanMakanan(bahan);
        }
    }

    private string mendapatkanMakanan(CookableFood makanan)
    {
        return makanan.namaMasakan;
    }

    public void OpenUI() {
        CampfireManager.instance.OpenUi();
        CampfireManager.instance.selectedCampfire = this;
        if (makananJadi != "") {
            GameObject rf = Instantiate(Resources.Load<GameObject>(makananJadi),
                CampfireManager.instance.makananaSlot.transform.position,
                CampfireManager.instance.makananaSlot.transform.rotation);

            rf.transform.SetParent(CampfireManager.instance.makananaSlot.transform);

            rf.transform.localScale = new Vector3(1f, 1f, 1f);
            makananJadi = "";
        }
    }

    public void mulaiMasak(InventoryItem makanan) {
        bahan = ConvertIntoCookable(makanan);
        isCooking = true;
        waktuMasak = waktuMemasakMakanan(bahan);
    
    }

    private CookableFood ConvertIntoCookable(InventoryItem makanan)
    {
        foreach (CookableFood cookable in CampfireManager.instance.cookingData.makananValid) {
            if (cookable.name == makanan.thisName) { 
                return cookable;
            }
        }
        return new CookableFood();
    }

    private float waktuMemasakMakanan(CookableFood makanan)
    {
        return makanan.waktuUntukMemasak;
    }
}
