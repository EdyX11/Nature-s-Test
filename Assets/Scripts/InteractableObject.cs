using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;


    public string ItemName;

    public string GetItemName() { 
        
        return ItemName;
    
    }

     void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget)
        {
            //if invetory is not full , we add , else alert is full
            if (!InventorySystem.Instance.CheckIfFull())
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

