using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class MapToggle : MonoBehaviour
{
    [SerializeField] GameObject mapImage;
    [SerializeField] GameObject mapHintText;

    bool isMapVisible = false;
    bool mapUnlocked = false;

    void Start()
    {
        if (mapImage != null)
            mapImage.SetActive(false);

        if (mapHintText != null)
            mapHintText.SetActive(false);
    }

    void Update()
    {
        if (mapUnlocked && Keyboard.current.gKey.wasPressedThisFrame)
        {
            isMapVisible = !isMapVisible;
            mapImage.SetActive(isMapVisible);
        }
    }

    // Call this when the player collects the guidance crystal
    public void UnlockMap()
    {
        mapUnlocked = true;

        if (mapHintText != null)
            mapHintText.SetActive(true); // show the "Press G" text
    }
}
