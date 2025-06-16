using UnityEngine;

/// Handles instant player death when entering the death zone trigger.
public class DeathZone : MonoBehaviour
{
    /// Called when another collider enters this trigger collider.
    /// Checks if the collider belongs to the player and sets their health to zero.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger zone
        PlayerBehaviour player = other.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            // Instantly set player health to 0
            player.ModifyHealth(-player.CurrentHealth()); // We'll add a getter for currentHealth next
        }
    }
}
