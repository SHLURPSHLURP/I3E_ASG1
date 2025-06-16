using UnityEngine;

/// Controls the behavior of a crystal key in the game,
/// including highlighting, unhighlighting, and collection logic.
public class CKeyBehaviour : MonoBehaviour
{
    /// Reference to the MeshRenderer component of the key.
    MeshRenderer myMeshRenderer;

    /// Value of the key added to the player's score when collected.
    [SerializeField]
    int keyValue = 50;

    /// Material used to highlight the key when interactable.
    [SerializeField]
    Material highlightMat;

    /// Stores the original material of the key to restore after highlight.
    Material GetMaterial;

    /// Called on the frame when the script is enabled.
    /// Initializes the MeshRenderer and caches the original material.
    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        GetMaterial = myMeshRenderer.material;
    }

    /// Changes the key's material to the highlight material.
    public void Highlight()
    {
        myMeshRenderer.material = highlightMat;
    }

    /// Reverts the key's material back to the original.
    public void Unhighlight()
    {
        myMeshRenderer.material = GetMaterial;
    }

    /// Collects the crystal key, adds to player's score, and destroys the key object.
    ///Reference to the PlayerBehaviour collecting the key.
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Crystal collected!");
        player.ModifyScore(keyValue);
        Destroy(gameObject);
    }
}
