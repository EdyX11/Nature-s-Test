using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    // Singleton design pattern used to be more efficient
    public static DialogSystem Instance { get; private set; }

    public Button sendButton;
    public Text dialogText;
    public InputField inputField;
    public Canvas dialogCanvas; // Your dialog UI Canvas

    // You mentioned 'writeText', which seems like it should be the InputField
    // where the player writes their messages, so I've included 'inputField' here.
    // Replace this with your actual InputField in the inspector.

    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep this UI persistent across scenes
        }
    }

    private void Start()
    {
        // Initially, hide the dialog canvas
        dialogCanvas.enabled = false;

        // Add a listener to your send button here if needed
        // For example:
        // sendButton.onClick.AddListener(SendMessage);

        // You would define SendMessage() to process and display the message
        // and then call the API or whatever functionality you need.
    }

    public void OpenDialog()
    {
        // Set the dialog canvas to be visible and interactable
        dialogCanvas.enabled = true;

        // Optional: Clear the text from the last message
        inputField.text = "";
        dialogText.text = "";

        // Optional: Focus the input field so the player can start typing right away
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void CloseDialog()
    {
        // Set the dialog canvas to be hidden
        dialogCanvas.enabled = false;
    }

    // Call this function when the send button is pressed
    public void SendMessage()
    {
        string message = inputField.text;
        // Here, you can add the message to the dialog area
        // and call your API or any other functionality as needed.
    }

    // You may also want to create functions to append messages to the dialogText,
    // both from the player and from the NPC (or ChatGPT responses).
}
