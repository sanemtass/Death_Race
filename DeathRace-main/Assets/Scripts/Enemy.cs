using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public List<SlotTransforms> slotTransformsList;
    public List<Gun> weaponsList;
    public List<GunBehaviour> createdWeaponsList;
    public GunBehaviour selectedGunToUpgrade;
    public Transform selectedGunTransform;

    [SerializeField] private Gun activeGuns;

    private UIManager uIManager;

    private void OnEnable()
    {
        UIManager.onBuyTurret += CreateWeapon;
        UIManager.onAddSlot += AddSlot;
    }

    private void OnDisable()
    {
        UIManager.onBuyTurret -= CreateWeapon;
        UIManager.onAddSlot -= AddSlot;
    }

    private void Start()
    {
        activeGuns = weaponsList[0];
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        SelectWeapon();
    }

    public void CreateWeapon()
    {
        bool isCreated = false;

        foreach (var t in slotTransformsList)
        {
            if (!t.IsEmpty() && t.isActive && !isCreated)
            {
                // Check if there is a selected gun to upgrade
                if (selectedGunTransform != null && t.slotTransforms == selectedGunTransform.parent)
                {
                    t.isEmpty = false;
                }

                if (activeGuns != null && activeGuns.gunPrefab != null)
                {
                    GameObject weapon = Instantiate(activeGuns.gunPrefab, t.slotTransforms);
                    weapon.transform.parent = t.slotTransforms;
                    float yOffset = 0.20f; // silahların yukarıda olacağı mesafe
                    weapon.transform.localPosition = Vector3.up * yOffset;
                    t.isEmpty = true;
                    isCreated = true;
                    createdWeaponsList.Add(weapon.GetComponent<GunBehaviour>());

                    // Clear the selected gun transform reference
                    selectedGunTransform = null;
                }
            }
            if (isCreated)
            {
                break;
            }
        }
    }

    private void SelectWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Gun"))
                {
                    Debug.Log("Silah seçildi");
                    uIManager.CreateUpgradeButton(hit.collider.gameObject);

                    // Set the selected gun to upgrade and its transform in the Enemy script
                    selectedGunToUpgrade = hit.collider.GetComponent<GunBehaviour>();
                    selectedGunTransform = hit.collider.transform;
                    break;
                }
            }
        }
    }

    public void Upgrade(GameObject currentGun)
    {
        GunBehaviour currentGunBehaviour = currentGun.GetComponent<GunBehaviour>();

        if (currentGunBehaviour.nextGun != null)
        {
            CreateWeaponAt(currentGun.transform, currentGunBehaviour.nextGun);

            // Remove the current gun's GunBehaviour component from the list
            createdWeaponsList.Remove(currentGunBehaviour);

            // Destroy the current gun from the scene
            Destroy(currentGun);

            // Upgrade işleminden sonra butonu kapat
            FindObjectOfType<UIManager>().CreateUpgradeButton(null);
        }
    }

    private void CreateWeaponAt(Transform gunTransform, Gun nextGun)
    {
        if (nextGun != null && nextGun.gunPrefab != null)
        {
            GameObject weapon = Instantiate(nextGun.gunPrefab, gunTransform.position, gunTransform.rotation);
            weapon.transform.parent = gunTransform.parent;
            float yOffset = 0.20f; // silahların yukarıda olacağı mesafe
            weapon.transform.localPosition = Vector3.up * yOffset;
            GunBehaviour newGunBehaviour = weapon.GetComponent<GunBehaviour>();
            createdWeaponsList.Add(newGunBehaviour);

            // Update the new gun's nextGun reference
            newGunBehaviour.nextGun = nextGun.nextGun;

            // Update the isEmpty property of the corresponding slot
            foreach (var slot in slotTransformsList)
            {
                if (slot.slotTransforms == gunTransform.parent)
                {
                    slot.isEmpty = true;
                    break;
                }
            }
        }
    }

    public void AddSlot()
    {
        foreach (var item in slotTransformsList)
        {
            if (!item.isActive)
            {
                item.isActive = true;
                item.slotTransforms.gameObject.SetActive(true);
                break;
            }
        }
    }
}

[System.Serializable]
public class SlotTransforms
{
    public Transform slotTransforms;

    public bool isEmpty;
    public bool isActive;

    public bool IsEmpty()
    {
        return isEmpty;
    }

}