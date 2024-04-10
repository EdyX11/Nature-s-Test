using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }
    [Header("USER INTERFACE")]
    [SerializeField] public GameObject craftingScreenUI;
    [SerializeField] public GameObject toolsScreenUI,survivalScreenUI,refineScreenUI;

    [Header("ITEM LIST")]
    public List<string> inventoryItemList = new List<string>();
    [SerializeField] public bool isOpen;
    // category buttons : tools etc 
    Button toolsBTN,survivalBTN,refineBTN;

    // Craft Buttons
    Button craftAxeBTN,craftPlankBTN, craftIronSwordBTN;

    //Requirement text
    Text AxeReq1;
    Text AxeReq2;
    Text PlankReq1;

    Text IronSwordReq1;
    Text IronSwordReq2;


    

    //All Blueprints

    public Blueprint AxeBLP = new Blueprint("Axe", 1 , 2 , "Stone", 3 , "Stick" , 3 );
    public Blueprint PlankBLP = new Blueprint("Plank", 2 , 1, "Log", 1, "", 0);
    public Blueprint IronSwordBLP = new Blueprint("IronSword", 1, 2, "Iron", 3, "Plank", 3);




    private void Awake()
    {
  
        if(Instance != null && Instance != this)
        {
        
                Destroy(gameObject);

        }
        else 
        { 

        Instance = this;

        }
    }

        

// Start is called before the first frame update
    void Start()
    {

        isOpen = false;
        // crafting
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        //survival
        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

        //refine
        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        //Stone Axe

        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();
        

        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

        //PickAxe
        IronSwordReq1 = toolsScreenUI.transform.Find("IronSword").transform.Find("req1").GetComponent<Text>();
        IronSwordReq2 = toolsScreenUI.transform.Find("IronSword").transform.Find("req2").GetComponent<Text>();
        craftIronSwordBTN = toolsScreenUI.transform.Find("IronSword").transform.Find("Button").GetComponent<Button>();
        craftIronSwordBTN.onClick.AddListener(delegate { CraftAnyItem(IronSwordBLP); });

    }
    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
    }
    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);
    }
    void OpenRefineCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);

        refineScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft)
    {
        //add item into inventory
        //Debug.Log("Item added to invetory");
        //loop for how mmany items we need to produce according to blueprint

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

        for (var i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
        {

            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        }





        //remove resources from invetory when creating an item
        if (blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);



        }
        else if(blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }

        StartCoroutine(calculate());
       



    }
    public IEnumerator calculate()
    {

        yield return 0; // no delay
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();

    }

    // Update is called once per frame
    void Update()
    {
       // RefreshNeededItems();


        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {

            Debug.Log("C is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;


            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }


            isOpen = false;
        }
    }



   public  void  RefreshNeededItems()
    {
        int stone_count = 0;

        int stick_count = 0;

        int log_count = 0; 
        
        int iron_count = 0;

        int plank_count = 0;


        inventoryItemList = InventorySystem.Instance.itemList;


        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;

                case "Stick":
                    stick_count += 1;
                    break;

                case "Log":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
                case "Iron":
                    iron_count += 1;
                    break;


                    //  case "....":

                    // break;

            }

        }
        // ----FOR THE STONE AXE ----

        AxeReq1.text = "3 Stone["+stone_count+"]";
        AxeReq2.text = "3 Stick["+stick_count+"]";

        if(stone_count >=3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {

            craftAxeBTN.gameObject.SetActive(false);

        }


        // ----- WOOD PLANK ------
        PlankReq1.text = "1 Log[" + log_count + "]";
   

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {

            craftPlankBTN.gameObject.SetActive(false);

        }

        // ----FOR THE PICK AXE ----
        IronSwordReq1.text = "3 Iron[" + iron_count + "]";
        IronSwordReq2.text = "3 Plank[" + plank_count + "]";
        if (iron_count >= 3 && plank_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftIronSwordBTN.gameObject.SetActive(true);
        }
        else
        {

            craftIronSwordBTN.gameObject.SetActive(false);

        }
    }

}
