using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;


public class NPCDialog : MonoBehaviour
{

    public static NPCDialog Instance { get; set; }

    public bool playerInRange;
    public bool isTalkingWithPlayer;
    [SerializeField] private AudioSource npcLaugh;


    [SerializeField] private GameObject toActivate;


    [SerializeField] private ChatGPT chatGPT; // Reference to the ChatGPT dialogue system

    [SerializeField] private Animator npcAnimator;

    private Coroutine laughCoroutine; // Coroutine variable to keep track

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTalkingWithPlayer = true;
            playerInRange = true;
            toActivate.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            npcAnimator.SetBool("Talk",true);
            laughCoroutine = StartCoroutine(LaughEveryTwentySeconds()); // Start laughing coroutine
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isTalkingWithPlayer = false;

            toActivate.SetActive(false);

            if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            CloseChat(); // Optionally close the chat interface

            if (laughCoroutine != null)
            {
                StopCoroutine(laughCoroutine); // Stop laughing coroutine
                laughCoroutine = null;
            }
            npcAnimator.SetBool("Talk", false);
        }
    }

    private IEnumerator LaughEveryTwentySeconds()
    {
        while (true)
        {
            npcLaugh.Play(); // Play the laugh sound
            yield return new WaitForSeconds(20); // Wait for 20 seconds
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
        //toActivate.SetActive(false);
        // Optionally implement chat closure logic
        Debug.Log("Chat Closed");
    }
}

