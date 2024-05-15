using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class InventorySystem : MonoBehaviour
{



    public GameObject ItemInfoUI;

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool canToggleInventory = true;
    public bool isOpen;

    //public bool isFull;

    //PickUp PopUp
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;


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


    void Start()
    {
        isOpen = false;
        //isFull = false;

        PopulateSlotList();
        Cursor.visible = false;

    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {

                slotList.Add(child.gameObject);
            }
        }
    }




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && canToggleInventory)
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryScreenUI.SetActive(isOpen);
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        // Handling component enabling based on inventory state
        if (isOpen)
        {
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.enabled = false;
        }
        else if (!CraftingSystem.Instance.isOpen)
        {
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.enabled = true;
        }
    }


    public void AddToInventory(string itemName)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
        whatSlotToEquip = FindNextEmptySlot();

            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation); // items , position , rotation we instatiate at
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            itemList.Add(itemName);
        //Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);






        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        
    }



    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);    
        pickupName.text= ("Picked up: " + itemName);
        pickupImage.sprite = itemSprite;
        StartCoroutine(HidePickupAlertAfterDelay(2f));
    }
    private IEnumerator HidePickupAlertAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);


        pickupAlert.SetActive(false);
    }





    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {

            if (slot.transform.childCount == 0) // if slot has no children -> slot is available to be filled 
            {

                return slot;
            }
        }

        return new GameObject();

    }

    public bool CheckSlotsAvailable(int emptyNeeded)
    {
        int emptySlot = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount <= 0)
            {

                emptySlot += 1;

            }
        }

        if (emptySlot >= emptyNeeded)
        {
            return true;
        }
        else
        {

            return false;
        }

    }

   public void  RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;
        for (var i = slotList.Count - 1; i >= 0; i--)//parse the list from end to start, backwards
        {
            if (slotList[i].transform.childCount > 0 ) // from index 20, if slot has child
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0 )
                  {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;
                  } 
            }
        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
    // getting the updated list 
    public void ReCalculateList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)// slot not empty 
            {

                string name = slot.transform.GetChild(0).name; //Stone (Clone) 

                string str2 = "(Clone)";
                //in order to remove string (Clone) which is generated when picking up an item

                string result = name.Replace(str2,"");
                Debug.Log("name changed to original,printiing result to be added");
                Debug.Log(result);
                itemList.Add(result);  // add the name to the new list 

            }
        }
    }




}