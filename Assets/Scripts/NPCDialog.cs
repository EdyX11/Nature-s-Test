using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenAI;
using UnityEngine.Events;


public class NPCDialog : MonoBehaviour
{

    public static NPCDialog Instance { get; set; }
    [Header("Conditions")]
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    [Header("Components")]
    [SerializeField] private AudioSource npcLaugh;
    [SerializeField] private GameObject toActivate;
    [SerializeField] private ChatGPT chatGPT; // Reference to the ChatGPT dialogue system
    [SerializeField] private Animator npcAnimator;

    [Header("Events")]
    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    private Coroutine laughCoroutine; // Coroutine NPC sound

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

    private void Start()
    {
        // Subscribe to events
        onPlayerEnter.AddListener(HandlePlayerEnter);
        onPlayerExit.AddListener(HandlePlayerExit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            isTalkingWithPlayer = true;
            onPlayerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isTalkingWithPlayer = false;
            onPlayerExit.Invoke();
        }
    }
    private void HandlePlayerEnter()
    {
        Debug.Log("Chat Opened");
        toActivate.SetActive(true);
        npcAnimator.SetBool("Talk", true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SelectionManager.Instance.DisableSelection();
        laughCoroutine = StartLaughing();
        InventorySystem.Instance.canToggleInventory = false;
        CraftingSystem.Instance.canToggleCrafting = false;
    }

    private void HandlePlayerExit()
    {
        Debug.Log("Chat Closed");
        toActivate.SetActive(false);
        npcAnimator.SetBool("Talk", false);
        if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        SelectionManager.Instance.EnableSelection();
        StopLaughing();
        InventorySystem.Instance.canToggleInventory = true;
        CraftingSystem.Instance.canToggleCrafting = true;
    }

    private Coroutine StartLaughing()
    {
        return StartCoroutine(LaughEveryTwentySeconds());
    }

    private void StopLaughing()
    {
        if (laughCoroutine != null)
        {
            StopCoroutine(laughCoroutine);
            laughCoroutine = null;
        }
    }

    private IEnumerator LaughEveryTwentySeconds()
    {
        while (true)
        {
            npcLaugh.Play();
            yield return new WaitForSeconds(20);
        }
    }

    private void OnDestroy()
    {
        onPlayerEnter.RemoveListener(HandlePlayerEnter);
        onPlayerExit.RemoveListener(HandlePlayerExit);
    }

}

