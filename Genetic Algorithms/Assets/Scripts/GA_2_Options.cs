using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GA_2_Options : MonoBehaviour
{
    [SerializeField] GameObject nodePrefab;
    [SerializeField] Transform nodeParent;
    [SerializeField] int arrayLength;
    [SerializeField] int arrayWidth;
    [SerializeField] Dropdown mutationDropdown;
    [SerializeField] Dropdown elitismDropdown;
    public TS_SO nodes;

    void Start()
    {
        nodes.ResetSO();
        Vector3 nodePosition;

        for (int i = -arrayLength; i <= arrayLength; i++)
        {
            for (int j = -arrayWidth; j <= arrayWidth; j++)
            {
                nodePosition = new Vector3(i, j, 0);
                Instantiate(nodePrefab, nodePosition, Quaternion.identity, nodeParent);
            }
        }
    }

    public void ValidateButton()
    {
        if (nodes.length>=10)
        {
            nodes.mutation = float.Parse(mutationDropdown.options[mutationDropdown.value].text);
            nodes.elitism = int.Parse(elitismDropdown.options[elitismDropdown.value].text);
            SceneManager.LoadScene(5);
        }
    }
}
