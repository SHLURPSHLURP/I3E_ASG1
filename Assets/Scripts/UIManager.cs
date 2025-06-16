using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// Handles all UI updates and display logic in the game.
public class UIManager : MonoBehaviour
{
    /// Singleton instance of the UIManager.
    public static UIManager Instance;

    /// UI text displaying the player's current score.
    public TMP_Text scoreText;

    /// UI text displaying how many crystal keys are left.
    public TMP_Text itemsLeftText;

    /// UI text for temporary on-screen messages.
    public TMP_Text messageText;

    /// Health bar slider UI element.
    public Slider healthSlider;

    /// UI text for showing the final objective message.
    [SerializeField] private TMP_Text finalObjectiveText;

    /// Sets up the singleton instance.
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// Updates the score text on the UI.
    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    /// Updates the health bar slider based on current and max health.
    public void UpdateHealth(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }

    /// Updates the text showing how many crystal keys remain.
    public void UpdateItemsLeft(int count)
    {
        itemsLeftText.text = $"Crystals Left: {count}";
    }

    /// Shows a message temporarily on screen.
    public void ShowTemporaryMessage(string message, float duration = 3f)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayMessage(message, duration));
    }

    /// Shows a final objective message temporarily.
    public void ShowFinalObjectiveTemporary(string message, float duration = 10f)
    {
        finalObjectiveText.text = message;
        finalObjectiveText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideFinalObjective));
        Invoke(nameof(HideFinalObjective), duration);
    }

    /// Hides the final objective message.
    private void HideFinalObjective()
    {
        finalObjectiveText.gameObject.SetActive(false);
    }

    /// Coroutine for showing and hiding a temporary message.
    IEnumerator DisplayMessage(string message, float duration)
    {
        messageText.text = message;
        messageText.enabled = true;
        yield return new WaitForSeconds(duration);
        messageText.enabled = false;
    }
}
