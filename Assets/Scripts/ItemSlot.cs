using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }






    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
       // SoundManager.Instance.PlaySound(SoundManager.Instance.dropItemSound);
        //if there is not item already then set our item.
        if (!Item)
        {

            DragDrop.itemBeingDragged.transform.SetParent(transform);
            DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

            if (transform.CompareTag("QuickSlot")== false ) 
            {
                Debug.Log("ItemNot in quickslot");
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                InventorySystem.Instance.ReCalculateList();

            }
            if (transform.CompareTag("QuickSlot"))
            {
                Debug.Log("Item added to quick slot");

                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                InventorySystem.Instance.ReCalculateList();
            }



        }


    }




}
