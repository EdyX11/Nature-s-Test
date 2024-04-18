using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;


public class NPCDialog : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;
    [SerializeField] private GameObject toActivate;


    [SerializeField] private ChatGPT chatGPT; // Reference to the ChatGPT dialogue system

    private void OnTriggerEnter(Collider other)
    {
        // Trigger chat possibility when the player enters the interaction range
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            toActivate.SetActive(true); 



           // ShowInteractionHint(true); // Visual feedback for interaction availability

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Disable chat possibility when the player leaves the interaction range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isTalkingWithPlayer = false; // Ensure we reset the talking flag



            //ShowInteractionHint(false); // Hide interaction hint
            toActivate.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CloseChat(); // Optionally close the chat interface
        }
    }

    public void OpenChat()
    {
        // Only open chat if the NPC is assigned and the player is in range
        if (chatGPT != null && playerInRange && !isTalkingWithPlayer)
        {
            chatGPT.SendReply(); // Initiate sending a reply via ChatGPT
            isTalkingWithPlayer = true;
            Debug.Log("Chat Opened");
        }
    }


    private void CloseChat()
    {
        // Optionally implement chat closure logic
        Debug.Log("Chat Closed");
    }
}

