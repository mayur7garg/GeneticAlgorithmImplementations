using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GA_2_Manager : MonoBehaviour
{
    [SerializeField] TS_SO nodeData;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] GameObject edgePrefab;
    [SerializeField] Transform bestNodesParent;
    [SerializeField] Transform currentNodesParent;
    [SerializeField] Vector3 offSet;
    [SerializeField] Vector3 edgeInitialPos;

    [Header("UI Texts")]
    [SerializeField] Text parameterText;
    [SerializeField] Text currentGenerationText;

    int populationSize = 20;
    System.Random random;
    TS_GA ga;
    Vector3[] nodePositions;
    GameObject[] bestEdges;
    GameObject[] currentEdges;

    void Start()
    {
        nodePositions = new Vector3[nodeData.length];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            nodePositions[i] = nodeData.nodePositions[i];
        }

        foreach (Vector3 node in nodePositions)
        {
            Instantiate(nodePrefab, node + offSet, Quaternion.identity, bestNodesParent);
            Instantiate(nodePrefab, node, Quaternion.identity, currentNodesParent);
        }
        random = new System.Random();
        ga = new TS_GA(populationSize, nodeData.length, random, FitnessFunction, nodeData.elitism, nodeData.mutation);

        bestEdges = new GameObject[nodePositions.Length];
        currentEdges = new GameObject[nodePositions.Length];

        for (int f = 0; f < nodePositions.Length; f++)
        {
            bestEdges[f] = Instantiate(edgePrefab, edgeInitialPos, Quaternion.identity, bestNodesParent);
            currentEdges[f] = Instantiate(edgePrefab, edgeInitialPos, Quaternion.identity, currentNodesParent);
        }
        StartCoroutine(AnimateGA());
    }

    IEnumerator AnimateGA()
    {
        float timeBetweenEachDNA = 0.3f;
        SetParameterText();
        yield return new WaitForSecondsRealtime(timeBetweenEachDNA);
        while (true)
        {
            ga.NewGeneration();
            SetBestEdges();
            SetGenerationText();
            for (int i = 0; i < populationSize; i++)
            {
                SetEdges(i);
                yield return new WaitForSecondsRealtime(timeBetweenEachDNA);
            }
        }
    }

    private float FitnessFunction(int id)
    {
        float[] distances = GetDistances(id);
        float distance = 0;
        for (int i = 0; i < distances.Length; i++)
        {
            distance += distances[i];
        }
        float score = 100 / distance;
        return score;
    }

    void SetBestEdges()
    {
        Vector3 initial;
        Vector3 final;
        Vector3 mid;
        Vector3 edgeScale = bestEdges[0].transform.localScale;
        float angle;
        for (int i = 1; i < ga.BestGenes.Length; i++)
        {
            initial = nodePositions[ga.BestGenes[i - 1]];
            final = nodePositions[ga.BestGenes[i]];
            mid = (final + initial) / 2;
            bestEdges[i - 1].transform.position = mid + offSet;
            edgeScale.y = Mathf.Abs(Vector3.Magnitude(final - initial))/2;
            bestEdges[i - 1].transform.localScale = edgeScale;
            angle = Vector3.SignedAngle(initial - final, Vector3.up, Vector3.back);
            bestEdges[i - 1].transform.rotation = Quaternion.Euler(0, 0, angle);

        }
        initial = nodePositions[ga.BestGenes[ga.BestGenes.Length - 1]];
        final = nodePositions[ga.BestGenes[0]];
        mid = (final + initial) / 2;
        bestEdges[ga.BestGenes.Length - 1].transform.position = mid + offSet;
        edgeScale.y = Mathf.Abs(Vector3.Magnitude(final - initial))/2;
        bestEdges[ga.BestGenes.Length - 1].transform.localScale = edgeScale;
        angle = Vector3.SignedAngle(initial - final, Vector3.up, Vector3.back);
        bestEdges[ga.BestGenes.Length - 1].transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void SetEdges(int id)
    {
        float[] distances = GetDistances(id);
        TS_DNA dna = ga.Population[id];
        Vector3 initial;
        Vector3 final;
        Vector3 mid;
        Vector3 edgeScale = currentEdges[0].transform.localScale;
        float angle;
        for (int i = 1; i < dna.Genes.Length; i++)
        {
            initial = nodePositions[dna.Genes[i - 1]];
            final = nodePositions[dna.Genes[i]];
            mid = (final + initial) / 2;
            currentEdges[i - 1].transform.position = mid;
            edgeScale.y = distances[i - 1]/2;
            currentEdges[i - 1].transform.localScale = edgeScale;
            angle = Vector3.SignedAngle(initial- final, Vector3.up, Vector3.back);
            currentEdges[i - 1].transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        initial = nodePositions[dna.Genes[dna.Genes.Length - 1]];
        final = nodePositions[dna.Genes[0]];
        mid = (final + initial) / 2;
        currentEdges[dna.Genes.Length - 1].transform.position = mid;
        edgeScale.y = distances[dna.Genes.Length - 1]/2;
        currentEdges[dna.Genes.Length - 1].transform.localScale = edgeScale;
        angle = Vector3.SignedAngle(initial- final, Vector3.up, Vector3.back);
        currentEdges[dna.Genes.Length - 1].transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    float[] GetDistances(int id)
    {
        TS_DNA dna = ga.Population[id];
        float[] distances = new float[dna.Genes.Length];
        Vector3 initial;
        Vector3 final;
        for (int i = 1; i < dna.Genes.Length; i++)
        {
            initial = nodePositions[dna.Genes[i - 1]];
            final = nodePositions[dna.Genes[i]];
            distances[i - 1] = Mathf.Abs(Vector3.Magnitude(final - initial));
        }
        initial = nodePositions[dna.Genes[dna.Genes.Length - 1]];
        final = nodePositions[dna.Genes[0]];
        distances[dna.Genes.Length - 1] = Mathf.Abs(Vector3.Magnitude(final - initial));
        return distances;
    }

    void SetParameterText()
    {
        string text = "Population Size : " + populationSize.ToString();
        text += "\nMutation Rate : " + nodeData.mutation.ToString();
        text += "\nElitism : " + nodeData.elitism.ToString();
        parameterText.text = text;
    }

    void SetGenerationText()
    {
        string text = "Generation : " + ga.Generation.ToString();
        currentGenerationText.text = text;
    }
}
