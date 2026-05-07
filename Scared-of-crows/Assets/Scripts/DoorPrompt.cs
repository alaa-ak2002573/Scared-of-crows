using UnityEngine;
using TMPro;

public class DoorPrompt : MonoBehaviour
{
    public GameObject promptCanvas;
    public TextMeshProUGUI promptText;

    void Start()
    {
        promptText.text = "The barn door is shut.";
        promptCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            promptCanvas.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            promptCanvas.SetActive(false);
    }
}