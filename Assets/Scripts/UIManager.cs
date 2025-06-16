using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text scoreText;
    public TMP_Text itemsLeftText;
    public TMP_Text messageText;

    public Slider healthSlider; // <-- Slider instead of text

    [SerializeField] private TMP_Text finalObjectiveText;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }
    }

    public void UpdateItemsLeft(int count)
    {
        itemsLeftText.text = $"Crystals Left: {count}";
    }

    public void ShowTemporaryMessage(string message, float duration = 3f)
    {
        StopAllCoroutines(); // in case something's already showing
        StartCoroutine(DisplayMessage(message, duration));
    }

    public void ShowFinalObjectiveTemporary(string message, float duration = 10f)
    {
        finalObjectiveText.text = message;
        finalObjectiveText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideFinalObjective));
        Invoke(nameof(HideFinalObjective), duration);
    }

    private void HideFinalObjective()
    {
        finalObjectiveText.gameObject.SetActive(false);
    }

    IEnumerator DisplayMessage(string message, float duration)
    {
        messageText.text = message;
        messageText.enabled = true;
        yield return new WaitForSeconds(duration);
        messageText.enabled = false;
    }
}
