using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestingSystem : MonoBehaviour
{
    [SerializeField] private StorageParameters storage;

    [SerializeField] private TMP_Text buttonText;
    private string startText = "Начать";
    private string endText = "Завершить";

    [SerializeField] private TMP_Text resultText;
    private List<string> VoiceInputs = new List<string>();

    public bool IsTestRunning = false;

    public void ButtonPressed()
    {
        if (IsTestRunning)
        {
            EndTest();
        }
        else
        {
            StartTest();
        }
    }

    private void StartTest()
    {
        IsTestRunning = true;
        buttonText.text = endText;
        resultText.text = "Найденные ошибки:\n";
        VoiceInputs.Clear();
    }

    private void EndTest()
    {
        IsTestRunning = false;
        buttonText.text = startText;
        resultText.text = "Результаты:\n";
        foreach (string s in VoiceInputs)
        {
            resultText.text += "* " + s + " - верно" + '\n';
        }
    }

    public void AddNewInput(string s)
    {
        VoiceInputs.Add(s);
        resultText.text += "* " + s + '\n';
    }

    private void Start()
    {
        buttonText.text = startText;
        resultText.text = "Тренировка по проверке помещения для баллонов";
    }
}
