using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI;
    public Camera cameraToUse; // Assigned this in the inspector

    TextMeshProUGUI interaction_text;

    private void Start()
    {
        if (cameraToUse == null)
        {
            Debug.LogError("Main camera not found");
            return;
        }

        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
        if (interaction_text == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on interaction_Info_UI");
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
            var interactableObject = selectionTransform.GetComponent<InteractableObject>();
            if (interactableObject != null)
            {
                interaction_text.text = interactableObject.GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                interaction_Info_UI.SetActive(false);
            }
        }
    }
}
