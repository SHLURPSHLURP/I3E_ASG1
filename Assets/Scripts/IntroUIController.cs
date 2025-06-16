using UnityEngine;

/// Controls the visibility of the intro UI and hides it when the player presses E.
public class IntroUIController : MonoBehaviour
{
    /// Reference to the intro UI GameObject (Canvas or Panel).
    public GameObject introUI; // Assign your Canvas or Panel here

    /// Called on the first frame. Enables the intro UI if assigned.
    void Start()
    {
        if (introUI != null)
        {
            introUI.SetActive(true); // Show intro UI at start
        }
    }

    /// Checks every frame for the E key to hide the intro UI.
    void Update()
    {
        if (introUI != null && introUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            introUI.SetActive(false); // Hide intro UI when E is pressed
        }
    }
}
