using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI itemsLeftText;
    public Slider healthSlider;

    void Awake()
    {
        // Singleton so PlayerBehaviour can call it easily
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateItemsLeft(int items)
    {
        itemsLeftText.text = "Items Left: " + items;
    }

    public void UpdateHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
