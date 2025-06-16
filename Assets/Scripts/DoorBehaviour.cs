using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    
    public void Open()
    {
        Vector3 doorRotation = transform.eulerAngles;
        doorRotation.y += 90f;
        transform.eulerAngles = doorRotation;
    }

    public void Close()
    {
        Vector3 doorRotation = transform.eulerAngles;
        doorRotation.y -= 90f;
        transform.eulerAngles = doorRotation;
    }
}