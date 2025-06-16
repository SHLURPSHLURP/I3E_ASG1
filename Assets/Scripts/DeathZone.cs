using UnityEngine;

public class DeathZone : MonoBehaviour
{
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
