using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; set; }

    [Header("UI")]
    public GameObject menuCanvas;
    public GameObject uiCanvas;
    public GameObject settingsMenu;
    public GameObject menu;
    public GameObject gameOverUI;

    public bool isMenuOpen;


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

    private void Update()
    {

        // SHOULD BE UPDATED TO ESCAPE FROM M
        if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen) { 


       
        
        uiCanvas.SetActive(false);
        menuCanvas.SetActive(true);

        isMenuOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        }
        else if(Input.GetKeyDown(KeyCode.M) && isMenuOpen)
        {

            settingsMenu.SetActive(false);
            menu.SetActive(true);

            uiCanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuOpen = false;


            


            if(CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false ) {


                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;


        }
                                                        
    }
}
