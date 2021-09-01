using UnityEngine.UI;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GA_1_Options : MonoBehaviour
{
    string validCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz.,;:!?()";
    [SerializeField] Dropdown populationSize;
    [SerializeField] Dropdown elitism;
    [SerializeField] Dropdown mutationLevel;
    [SerializeField] Dropdown scoring;
    [SerializeField] Text userInput;
    [SerializeField] Text errorMessage;

    public void ValidateButton(int index)
    {
        if (ValidateString())
        {
            int population = int.Parse(populationSize.options[populationSize.value].text);
            PlayerPrefs.SetInt("GA_1_Population", population);

            int elite = int.Parse(elitism.options[elitism.value].text);
            PlayerPrefs.SetInt("GA_1_Elite", elite);

            float mutation = float.Parse(mutationLevel.options[mutationLevel.value].text);
            PlayerPrefs.SetFloat("GA_1_Mutation", mutation);

            string scoringPattern = scoring.options[scoring.value].text;
            PlayerPrefs.SetString("GA_1_Scoring", scoringPattern);

            PlayerPrefs.SetString("GA_1_UserInput", userInput.text);

            SceneManager.LoadScene(index);
        }
    }

    private bool ValidateString()
    {
        if (userInput.text.Length<5)
        {
            errorMessage.text = "Input string too short!";
            return false;
        }
        if (userInput.text.Length>30)
        {
            errorMessage.text = "Input string too long!";
            return false;
        }
        foreach (char c in userInput.text)
        {
            if (!(validCharacters.Contains(c.ToString())))
            {
                errorMessage.text = "Input string has invalid characters!";
                return false;
            }
        }
        errorMessage.text = "";
        return true;
    }
}
