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
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable  && interactable.playerInRange)
            {

                onTarget = true;
                selectedObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                //if to change hand to dot
                if (interactable.CompareTag("pickable"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;
                }
                else
                {
                    handIcon.gameObject.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                    handIsVisible = false;
                }
            }
            else//if there is a hit but no interactable script
            {   
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                handIcon.gameObject.SetActive(false);
                centerDotImage.gameObject.SetActive(true);

                handIsVisible = false;  
            }
        }
        else // if the is no hit at all 
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            handIcon.gameObject.SetActive(false);
            centerDotImage.gameObject.SetActive(true);

            handIsVisible = false;
        }
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
