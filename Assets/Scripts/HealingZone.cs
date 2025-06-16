using UnityEngine;

public class HealingZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            // Heal player to full health instantly
            player.SetHealthToMax();
        }
    }
}