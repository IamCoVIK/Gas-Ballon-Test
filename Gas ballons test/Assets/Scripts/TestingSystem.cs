using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestingSystem : MonoBehaviour
{
    [SerializeField] private StorageParameters storage;
    [SerializeField] private Belt belt;
    [SerializeField] private TMP_Text buttonText;
    private string startText = "Начать";
    private string endText = "Завершить";
    private string resetText = "Сброс";

    [SerializeField] private TMP_Text resultText;
    private List<string> VoiceInputs = new List<string>();

    [SerializeField] private GameObject walls;

    public int TestStatus = 0;
    // 0 - Сброс состояния команты
    // 1 - Прохождение теста в процессе
    // 2 - Обзор результатов

    public void ButtonPressed()
    {
        if (TestStatus == 0)
        {
            StartTest();
        }
        else if (TestStatus == 1)
        {
            EndTest();
        }
        else if (TestStatus == 2)
        {
            ResetTest();
        }
    }

    private void StartTest()
    {
        TestStatus = 1;
        buttonText.text = endText;
        resultText.text = "Найденные ошибки:\n";
        VoiceInputs.Clear();
        walls.SetActive(false);
        belt.ActivateBelt();
    }

    private void EndTest()
    {
        TestStatus = 2;
        buttonText.text = resetText;
        resultText.text = "Результаты:\n";
        foreach (string s in VoiceInputs)
        {
            resultText.text += "* " + s + " - верно" + '\n';
        }
        walls.SetActive(false);
    }

    private void ResetTest()
    {
        TestStatus = 0;
        buttonText.text = startText;
        resultText.text = "Описание сути программы???\n";
        ResetStorage();
        walls.SetActive(true);
        belt.ResetBeltItems();
        belt.DeactivateBelt();
    }

    private void ResetStorage()
    {
        storage.ResetAllPhysicObjs();
    }

    private void NewRandomStorage()
    {

    }

    public void AddNewInput(string s)
    {
        if (TestStatus != 1)
            return;
        VoiceInputs.Add(s);
        resultText.text += "* " + s + '\n';
    }

    private void Start()
    {
        buttonText.text = startText;
        resultText.text = "Описание сути программы???";
        walls.SetActive(true);
    }
}
