using UnityEngine;

public class CKeyBehaviour : MonoBehaviour
{
    MeshRenderer myMeshRenderer;
    [SerializeField]
    int keyValue = 50;

    [SerializeField]
    Material highlightMat;
    Material GetMaterial;

    //METHOD FOR HIGHLIGHTING INTERACTABLE KEY
    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        GetMaterial = myMeshRenderer.material;
    }

    public void Highlight()
    {
        myMeshRenderer.material = highlightMat;

    }

    public void Unhighlight()
    {
        myMeshRenderer.material = GetMaterial;
    }

    //METHOD TO COLLECT CRYSTAL KEY
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Crystal collected!");
        player.ModifyScore(keyValue);
        Destroy(gameObject);
    }

}