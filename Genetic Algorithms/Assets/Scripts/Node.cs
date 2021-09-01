using UnityEngine;

public class Node : MonoBehaviour
{
    GA_2_Options options;
    bool isSelected = false;
    [SerializeField] Material selectedMaterial;

    private void Start()
    {
        options = FindObjectOfType<GA_2_Options>();
    }
    private void OnMouseDown()
    {
        if (!isSelected)
        {
            if (options.nodes.length<30)
            {
                GetComponent<MeshRenderer>().material = selectedMaterial;
                isSelected = true;
                options.nodes.AddNode(transform.position);
            }
        }
    }
}
