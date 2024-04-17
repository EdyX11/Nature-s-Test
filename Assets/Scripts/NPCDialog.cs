using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;

public class NPCDialog : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    [SerializeField] private ChatGPT chatGPT; // Reference to the ChatGPT script

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

    public void OpenChat()
    {
        if (chatGPT != null && playerInRange) // Check if ChatGPT is assigned and player is in range
        {
            chatGPT.SendReply(); // Call the method to send a reply via ChatGPT
            isTalkingWithPlayer = true;
            Debug.Log("Chat Opened");
        }
    }
}
