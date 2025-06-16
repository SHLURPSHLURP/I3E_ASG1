using UnityEngine;

public class ManaCrystalBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject restoredCrystalPrefab;

    public void TryActivate(PlayerBehaviour player)
    {
        if (!player.HasAllCrystals())
        {
            UIManager.Instance.ShowTemporaryMessage("You must collect all 5 crystal keys first...");
            return;
        }

        Debug.Log("Mana Crystal Restored!");
        Instantiate(restoredCrystalPrefab, transform.position, transform.rotation);
        Destroy(gameObject); // remove the old red mana crystal
        UIManager.Instance.ShowFinalObjectiveTemporary("Mana crystal restored. Peace returns to the kingdom...", 10f);
    }
}

