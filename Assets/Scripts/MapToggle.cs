using UnityEngine;
using UnityEngine.InputSystem;

public class MapToggle : MonoBehaviour
{
    [SerializeField] GameObject mapImage;
    bool isMapVisible = false;

    void Start()
    {
        if (mapImage != null)
            mapImage.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            isMapVisible = !isMapVisible;
            mapImage.SetActive(isMapVisible);
        }
    }
}

