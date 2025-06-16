using UnityEngine;

/// Heals the player to full health when they enter the healing zone.
public class HealingZone : MonoBehaviour
{
    /// Called when another collider enters this trigger collider.
    /// If it's the player, set their health to maximum.
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
