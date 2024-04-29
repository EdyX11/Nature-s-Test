using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{

    //singleton design pattern used to be more efficient
    public static SelectionManager Instance { get; set; }
    public bool onTarget;


    public GameObject selectedObject;


    public GameObject interaction_Info_UI;
    public Camera cameraToUse; // Assigned this in the inspector

    TextMeshProUGUI interaction_text;

    public Image centerDotImage;
    public Image handIcon;
    public bool handIsVisible;


    public GameObject selectedTree;
    public GameObject chopHolder;




    private void Start()
    {
        //camera check for missing
        if (cameraToUse == null)
        {
            Debug.LogError("Main camera not found");
            return;
        }



        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
        if (interaction_text == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on interaction_Info_UI");
        }
    }

    private void Awake()
    {

        if(Instance != null&& Instance != this) {
            Destroy(gameObject);
            }

        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (cameraToUse == null) return;

        Ray ray = cameraToUse.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.transform.name);  // Check what is being hit
            HandleChoppableTreeInteraction(hit);
            HandleInteractableObjectInteraction(hit);
            HandleInteractableNPCInteraction(hit);
            HandleAnimalInteraction(hit);
            HandleEnemyNPCInteraction(hit);
            HandleBigBearEnemyNPCInteraction(hit);
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            interaction_text.text = "";
            handIcon.gameObject.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
        }
    }

    private void HandleInteractableObjectInteraction(RaycastHit hit)
    {
        InteractableObject interactable = hit.transform.GetComponent<InteractableObject>();
        if (interactable && interactable.playerInRange)
        {
            Debug.Log("Interactable Object: " + interactable.GetItemName());  // Confirm item name is fetched
            onTarget = true;
            selectedObject = interactable.gameObject;

            interaction_Info_UI.SetActive(true);
            interaction_text.text = interactable.GetItemName();

            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(true);
            handIsVisible = true;
        }
        else
        {
            Debug.Log("No interactable object or not in range");
            handIsVisible = false;
            handIcon.gameObject.SetActive(false);
        }
    }

    private void HandleChoppableTreeInteraction(RaycastHit hit)
    {
        ChoppableTree choppableTree = hit.transform.GetComponent<ChoppableTree>();
        if (choppableTree && choppableTree.playerInRange)
        {
            choppableTree.canBeChopped = true;
            selectedTree = choppableTree.gameObject;
            chopHolder.gameObject.SetActive(true);
        }
        else
        {
            if (selectedTree != null)
            {
                selectedTree.GetComponent<ChoppableTree>().canBeChopped = false;
                selectedTree = null;
                chopHolder.gameObject.SetActive(false);
            }
        }
    }
    private void HandleInteractableNPCInteraction(RaycastHit hit)
    {
        NPCDialog npc = hit.transform.GetComponent<NPCDialog>();

        if (npc && npc.playerInRange)
        {
            // Interaction logic can be minimized here since most actions are triggered by OnTriggerEnter
            // Here, we just handle cursor visibility if necessary
            ManageCursor(npc.isTalkingWithPlayer);
        }
        else
        {
            // Optionally reset UI or other interactions if the player is not in range or interacting
            ClearInteractionUI();
        }
    }

    private void ManageCursor(bool isTalking)
    {
        if (isTalking)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            // Optional: Reset cursor if the NPC interaction ends and the chat closes
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ClearInteractionUI()
    {
        // Clears any interaction-related UI elements, if previously used
        interaction_text.text = "";
        interaction_Info_UI.SetActive(false);
    }


    private void HandleAnimalInteraction(RaycastHit hit)
    {
        Animal animal = hit.transform.GetComponent<Animal>();
        if (animal != null && animal.playerInRange)
        {
            if (animal.isDead)
            {
                interaction_text.text = "Loot";

                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;


                if (Input.GetMouseButtonDown(0))
                {

                   Lootable lootable = animal.GetComponent<Lootable>();
                   Loot(lootable); // method pass var
                }

            }

            else { 
            interaction_text.text = animal.animalName;
            interaction_Info_UI.SetActive(true);

            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);

            handIsVisible = false;
             
            }
            if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
            {
                if (!animal.IsAlreadyBeingAttacked)  //state from Animal script
                {
                    StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }
        }
      
       
    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        animal.IsAlreadyBeingAttacked = true; // Lock to prevent re-entrancy.
        yield return new WaitForSeconds(delay);

        if (animal != null && animal.playerInRange && !animal.isDead)
        {
            animal.TakeDamage(damage);
        }

        animal.IsAlreadyBeingAttacked = false; // Unlock after operation.
    }
    private void HandleEnemyNPCInteraction(RaycastHit hit)
    {
        EnemyNPC enemy = hit.transform.GetComponent<EnemyNPC>();
        if (enemy != null && enemy.playerInRange)
        {
            if (enemy.isDead)
            {
                interaction_text.text = "Loot";

                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;


                if (Input.GetMouseButtonDown(0))
                {

                    Lootable lootable = enemy.GetComponent<Lootable>();
                    Loot(lootable); // method pass var
                }

            }

            else
            {
                interaction_text.text = enemy.enemyName;
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

                handIsVisible = false;

            }
            if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
            {
                if (!enemy.IsAlreadyBeingAttacked)  //state from Animal script
                {
                    StartCoroutine(DealDamageToEnemy(enemy, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }
        }
        IEnumerator DealDamageToEnemy(EnemyNPC enemy, float delay, int damage)
        {
            enemy.IsAlreadyBeingAttacked = true; // Lock to prevent re-entrancy.
            yield return new WaitForSeconds(delay);

            if (enemy != null && enemy.playerInRange && !enemy.isDead)
            {
                enemy.TakeDamage(damage);
            }

            enemy.IsAlreadyBeingAttacked = false; // Unlock after operation.
        }

    }
    private void HandleBigBearEnemyNPCInteraction(RaycastHit hit)
    {
        BigBearEnemy bigBear = hit.transform.GetComponent<BigBearEnemy>();
        if (bigBear != null && bigBear.playerInRange)
        {
            if (bigBear.isDead)
            {
                interaction_text.text = "Loot";

                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);

                handIsVisible = true;


                if (Input.GetMouseButtonDown(0))
                {

                    Lootable lootable = bigBear.GetComponent<Lootable>();
                    Loot(lootable); // method pass var
                }

            }

            else
            {
                interaction_text.text = bigBear.enemyName;
                interaction_Info_UI.SetActive(true);

                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

                handIsVisible = false;

            }
            if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
            {
                if (!bigBear.IsAlreadyBeingAttacked)  //state from Animal script
                {
                    StartCoroutine(DealDamageToEnemyBigBear(bigBear, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
            }
        }
        IEnumerator DealDamageToEnemyBigBear(BigBearEnemy bigBear, float delay, int damage)
        {
            bigBear.IsAlreadyBeingAttacked = true; // Lock to prevent re-entrancy.
            yield return new WaitForSeconds(delay);

            if (bigBear != null && bigBear.playerInRange && !bigBear.isDead)
            {
                bigBear.TakeDamageBear(damage);
            }

            bigBear.IsAlreadyBeingAttacked = false; // Unlock after operation.
        }

    }
    private void Loot(Lootable lootable) 
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootReceived> receivedLoot = new List<LootReceived>();// empty list

            foreach (LootPossibility loot in lootable.possibleLoot)// loop over the posible loot
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax+1);// rand no , exclusive max amount
                if(lootAmount != 0)
                {
                    LootReceived lt = new LootReceived();

                    lt.item = loot.item;
                    lt.amount = lootAmount;

                    receivedLoot.Add(lt);   


                }

            }


            lootable.finalLoot = receivedLoot;
            lootable.wasLootCalculated = true;


        }
        //spawn loot, same method as for wood
        Vector3 lootSpawnPos = lootable.gameObject.transform.position; ;

        foreach (LootReceived lootReceived in lootable.finalLoot)
        {
            for(int i = 0;i<lootReceived.amount; i++)
            {

                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootReceived.item.name + "_Model"),
                    new Vector3(lootSpawnPos.x, lootSpawnPos.y + 0.2f, lootSpawnPos.z),
                    Quaternion.Euler(0,0,0));
            }

        }
        //can modify in order for the bloodpuddle to remain for some time
        if (lootable.GetComponent<Animal>())
        {

            lootable.GetComponent<Animal>().bloodPuddle.transform.SetParent(lootable.transform.parent);
           
        }


        //destroy looted body 
        Destroy(lootable.gameObject);


    
    }

   

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;  
        interaction_Info_UI.SetActive(false) ;

        selectedObject = null;

    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);


    }


}
