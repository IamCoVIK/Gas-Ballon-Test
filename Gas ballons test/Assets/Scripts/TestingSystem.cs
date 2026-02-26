using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

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
    private string aboutText =
        "Обучение условиям хранения газовых баллонов в интерактивной форме\n\n" +
        "1) Нажмите кнопку ниже, чтобы начать\n" +
        "2) Изучите представленное помещение на предмет нарушений условий хранения баллонов\n" +
        "3) Занесите найденные нарушения в блокнот с помощью голосового ввода\n" +
        "4) Нажмите на кнопку еще раз, чтобы узнать результаты проверки\n";

    [SerializeField] private TMP_Text missedText;

    [SerializeField] private GameObject walls;

    [SerializeField] private AudioSource failSound;

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
        storage.GenerateStorageSituation();
    }

    private void EndTest()
    {
        TestStatus = 2;
        buttonText.text = resetText;
        resultText.text = "Результаты:\n";
        foreach (string s in VoiceInputs)
        {
            if (storage.CheckVoiceInput(s))
            {
                resultText.text += "* " + s + " - верно" + '\n';
            }
            else
            {
                resultText.text += "* " + s + " - неверно" + '\n';
            }
        }
        List<string> missed = storage.MissedParams();
        missedText.text = "Пропущенные ошибки:\n";
        if (missed.Count == 0)
        {
            missedText.text += "Все верно!\n";
        }
        foreach (string s in missed)
        {
            missedText.text += "* " + s + '\n';
        }
        walls.SetActive(false);
    }

    private void ResetTest()
    {
        TestStatus = 0;
        buttonText.text = startText;
        resultText.text = aboutText;
        missedText.text = "";
        ResetStorage();
        walls.SetActive(true);
        belt.ResetBeltItems();
        belt.DeactivateBelt();
    }

    private void ResetStorage()
    {
        storage.ResetAllPhysicObjs();
    }

    public void FailedTest()
    {
        TestStatus = 2;
        buttonText.text = resetText;
        resultText.text = "Вы нарушили технику безопасности!";
        walls.SetActive(false);

        failSound.Play();
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
        resultText.text = aboutText;
        walls.SetActive(true);
    }
}
