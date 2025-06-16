using UnityEngine;

/// Controls the opening and closing rotation of a door.
public class DoorBehaviour : MonoBehaviour
{
    /// Rotates the door 90 degrees on the Y axis to open it.
    public void Open()
    {
        Vector3 doorRotation = transform.eulerAngles;
        doorRotation.y += 90f;
        transform.eulerAngles = doorRotation;
    }

    /// Rotates the door -90 degrees on the Y axis to close it.
    public void Close()
    {
        Vector3 doorRotation = transform.eulerAngles;
        doorRotation.y -= 90f;
        transform.eulerAngles = doorRotation;
    }
}
