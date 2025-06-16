using UnityEngine;
using UnityEngine.SceneManagement;

/// Handles the activation of the mana crystal once all keys are collected.
/// Triggers victory logic and displays the success screen.
public class ManaCrystalBehaviour : MonoBehaviour
{
    /// Prefab to show the restored version of the mana crystal.
    [SerializeField] private GameObject restoredCrystalPrefab;

    /// Canvas that shows the success screen.
    [SerializeField] private GameObject successScreenCanvas;

    /// Flag to check if the game has been finished.
    private bool gameFinished = false;

    /// Tries to activate the mana crystal if the player has collected all crystal keys.
    /// Plays victory audio, shows success UI, and pauses the game.
    public void TryActivate(PlayerBehaviour player)
    {
        if (!player.HasAllCrystals())
        {
            UIManager.Instance.ShowTemporaryMessage("You must collect all 5 crystal keys first...");
            return;
        }

        Debug.Log("Mana Crystal Restored!");
        Instantiate(restoredCrystalPrefab, transform.position, transform.rotation);
        Destroy(gameObject);

        AudioController.Instance.PlayVictory();

        if (successScreenCanvas != null)
        {
            successScreenCanvas.SetActive(true);
            Time.timeScale = 0f;
            gameFinished = true;
        }
    }

    /// Listens for the E key after game completion to restart the game.
    void Update()
    {
        if (gameFinished && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}
