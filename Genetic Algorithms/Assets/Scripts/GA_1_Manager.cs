using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GA_1_Manager : MonoBehaviour

{   enum ScoringPattern {Linear, Exponential }

    string targetString;
    string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz.,;:!?()";
    int populationSize;
    float mutationRate = 0.01f;
    int elitism;
    ScoringPattern scoring;

    private GeneticAlgorithm<char> ga;
    private System.Random random;

    [Header("UI Texts")]
    [SerializeField] Text parametersText;
    [SerializeField] Text specsText;
    [SerializeField] Text populationText;

    [Header("Buttons")]
    [SerializeField] GameObject generateButton;
    [SerializeField] GameObject homeButton;
    [SerializeField] GameObject backButton;
    

    void Start()
    {
        targetString = PlayerPrefs.GetString("GA_1_UserInput", "Infinite Monkey Problem");
        populationSize = PlayerPrefs.GetInt("GA_1_Population", 6);
        mutationRate = PlayerPrefs.GetFloat("GA_1_Mutation", 0.01f);
        elitism = PlayerPrefs.GetInt("GA_1_Elite", 0);
        if (PlayerPrefs.GetString("GA_1_Scoring", "Linear")=="Linear")
        {
            scoring = ScoringPattern.Linear;
        }
        else
        {
            scoring = ScoringPattern.Exponential;
        }
        SetParametersText();
        random = new System.Random();
        ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, random, GetRandomCharacter, FitnessFunction, elitism, mutationRate);
    }

    private char GetRandomCharacter()
    {
        int i = random.Next(validCharacters.Length);
        return validCharacters[i];
    }

    private float FitnessFunction(int id)
    {
        float score = 0;
        DNA<char> dna = ga.Population[id];

        for (int i = 0; i < dna.Genes.Length; i++)
        {
            if (dna.Genes[i]==targetString[i])
            {
                score++;
            }
        }
        score /= targetString.Length;
        if (scoring == ScoringPattern.Exponential)
        {
            score = (Mathf.Pow(5, score) - 1) / 4;
        }
        return score;
    }

    private void SetParametersText()
    {
        string paramText = "Population Size : " + populationSize.ToString();
        paramText += "\nMutation Rate : " + mutationRate.ToString();
        paramText += "\nElitism : " + elitism.ToString();
        paramText += "\nScoring Pattern : " + PlayerPrefs.GetString("GA_1_Scoring", "Linear");
        paramText += "\nTarget String : " + targetString;
        parametersText.text = paramText;
    }

    private void UpdateGenerationText()
    {
        string currentGenerationText = "";
        for (int i = 0; i < populationSize; i++)
        {
            currentGenerationText += new string(ga.Population[i].Genes);
            currentGenerationText += "\n";
        }

        populationText.text = currentGenerationText;

        string specs = "Current Generation : " + ga.Generation.ToString();
        specs += "\nBest String : " + new string(ga.BestGenes);
        specs += "\nBest Score : " + ga.BestFitness.ToString();

        specsText.text = specs;
    }

    public void GenerateButton()
    {
        StartCoroutine(AnimateGA());
        generateButton.SetActive(false);
        homeButton.SetActive(true);
        backButton.SetActive(true);
    }

    IEnumerator AnimateGA()
    {
        float timeBetweenEachGeneration = 0.05f;
        do
        {
            ga.NewGeneration();
            UpdateGenerationText();
            yield return new WaitForSecondsRealtime(timeBetweenEachGeneration);
        } while (ga.BestFitness != 1);

        specsText.color = Color.red;
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
