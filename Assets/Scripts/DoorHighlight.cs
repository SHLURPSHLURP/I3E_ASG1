using UnityEngine;

public class DoorHighlight : MonoBehaviour
{
  // Start is called once before the first execution of Update after the MonoBehaviour is created

    MeshRenderer myMeshRenderer;
    
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
    
}
