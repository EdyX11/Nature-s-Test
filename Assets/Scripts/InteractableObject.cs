using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [Header("Conditions")]
    [SerializeField] public bool playerInRange;
    [SerializeField] public string ItemName;

    public string GetItemName() { 
        
        return ItemName;
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            HandleItemInteraction();
        }
    }


    private void HandleItemInteraction()
    {
        if (InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            InventorySystem.Instance.AddToInventory(ItemName);
            Debug.Log("Item added to inventory");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full");
        }
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
}

