using UnityEngine;

public class IntroUIController : MonoBehaviour
{
    public GameObject introUI; // Assign your Canvas or Panel here

    void Start()
    {
        if (introUI != null)
        {
            introUI.SetActive(true); // Show intro UI at start
        }
    }

    void Update()
    {
        if (introUI != null && introUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            introUI.SetActive(false); // Hide intro UI when E is pressed
        }
    }
}