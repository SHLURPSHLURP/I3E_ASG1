using UnityEngine;
using UnityEngine.SceneManagement;

public class ManaCrystalBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject restoredCrystalPrefab;
    [SerializeField] private GameObject successScreenCanvas;

    private bool gameFinished = false;

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

    void Update()
    {
        if (gameFinished && Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}
