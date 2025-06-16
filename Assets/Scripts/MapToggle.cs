using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// Handles map visibility toggle and unlocking logic when player collects the guidance crystal.
public class MapToggle : MonoBehaviour
{
    /// Reference to the GameObject representing the map image.
    [SerializeField] GameObject mapImage;

    /// Reference to the GameObject that shows the map hint text (e.g., "Press G").
    [SerializeField] GameObject mapHintText;

    /// Tracks whether the map is currently visible.
    bool isMapVisible = false;

    /// Tracks whether the map has been unlocked.
    bool mapUnlocked = false;

    /// Hides the map and hint text at the start of the game.
    void Start()
    {
        if (mapImage != null)
            mapImage.SetActive(false);

        if (mapHintText != null)
            mapHintText.SetActive(false);
    }

    /// Listens for the G key to toggle the map if it has been unlocked.
    void Update()
    {
        if (mapUnlocked && Keyboard.current.gKey.wasPressedThisFrame)
        {
            isMapVisible = !isMapVisible;
            mapImage.SetActive(isMapVisible);
        }
    }

    /// Unlocks the map and displays the hint text when the guidance crystal is collected.
    public void UnlockMap()
    {
        mapUnlocked = true;

        if (mapHintText != null)
            mapHintText.SetActive(true); // show the "Press G" text
    }
}
