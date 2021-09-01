using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TS_SO", menuName = "TS/SO")]
public class TS_SO : ScriptableObject
{
    public int length = 0;
    public float mutation = 0.01f;
    public int elitism = 1;
    public List<Vector3> nodePositions = new List<Vector3>();

    public void AddNode(Vector3 node)
    {
        nodePositions.Add(node);
        length++;
    }

    public void ResetSO()
    {
        length = 0;
        mutation = 0.01f;
        elitism = 1;
        nodePositions = new List<Vector3>();
    }
}
