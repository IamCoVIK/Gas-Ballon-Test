using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Параметр помещения для хранения баллонов
/// </summary>
public abstract class Parameter
{
    /// <summary>
    /// Название параметра
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Описание параметра
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Включена ли случайная генерация значения параметра
    /// </summary>
    public bool IsRandom { get; set; } = true;
    /// <summary>
    /// Соответствие значения требованиям
    /// </summary>
    public bool IsWrong { get; set; }
    /// <summary>
    /// Слова, по которым определяется какой параметр упоминается в голосовой проверке
    /// </summary>
    public List<string> IdentifierKeyWords;
    /// <summary>
    /// Слова, по которым определяется упоминание неверности значения параметра в голосовой проверке
    /// </summary>
    public List<string> WrongValueKeyWords;

    /// <summary>
    /// Переключение случайной генерации параметра
    /// </summary>
    /// <returns>Включена ли теперь случайная генерация параметра</returns>
    public bool ToggleRandom()
    {
        return IsRandom.Toggle();
    }

    /// <summary>
    /// Проверка транскрипции голосового ввода на упонинание параметра
    /// </summary>
    /// <param name="text">Текстовая транскрипция голосового ввода</param>
    /// <returns>true - текст верно упоминает параметр и его значение</returns>
    public bool TextCheck(string text)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in text)
        {
            if (!char.IsPunctuation(c))
                sb.Append(c);
        }
        text = sb.ToString().ToLower();
        string[] words = text.Split(' ');

        bool id = false;
        bool wrong = false;

        foreach (string word in IdentifierKeyWords)
        {
            if (text.Contains(word))
            {
                id = true;
            }
        }
        foreach (string word in WrongValueKeyWords)
        {
            if (text.Contains(word) && IsWrong)
            {
                wrong = true;
            }
        }
        /*foreach (string word in words)
        {
            if (IdentifierKeyWords.Contains(word))
            {
                id = true;
            }
            if (WrongValueKeyWords.Contains(word) && IsWrong)
            {
                wrong = true;
            }
        }*/

        return id & wrong;
    }

    public override string ToString()
    {
        return Name;
    }
}

/// <summary>
/// Целочисленный параметр помещения для хранения баллонов
/// </summary>
public class IntParameter : Parameter
{
    /// <summary>
    /// Значение параметра
    /// </summary>
    public int Value { get; set; }
    
    public int BorderValue;
    public int MinValue;
    public int MaxValue;

    public bool Inversed = false;

    /// <summary>
    /// Установить значение параметра
    /// </summary>
    /// <param name="value">Устанавливаемое значение параметра</param>
    public void SetParameter(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Генерация случайного значения параметра
    /// </summary>
    /// <returns>Полученное случайное значение параметра</returns>
    public int GenerateRandomValue()
    {
        Value = Random.Range(MinValue, MaxValue);
        return Value;
    }

    /// <summary>
    /// Проверка значения на соответствие требованиям
    /// </summary>
    /// <returns>Несоответствие требованиям</returns>
    public bool Check()
    {
        if (Inversed)
        {
            if (Value >= BorderValue)
            {
                IsWrong = false;
                return false;
            }
            IsWrong = true;
            return true;
        }
        else
        {
            if (Value < BorderValue)
            {
                IsWrong = false;
                return false;
            }
            IsWrong = true;
            return true;
        }
    }

    /// <summary>
    /// Получение значения параметра
    /// </summary>
    /// <returns>Полученное значение параметра</returns>
    public int GetParameter()
    {
        if (IsRandom) { return GenerateRandomValue(); }
        else { return Value; }
    }

    public IntParameter(string name, string description, int minValueInclusive, int maxValueExclusive, int borderValue, bool inversed, List<string> idWords, List<string> wrongWords)
    {
        Name = name;
        Description = description;
        MinValue = minValueInclusive;
        MaxValue = maxValueExclusive;
        BorderValue = borderValue;
        Inversed = inversed;
        IdentifierKeyWords = idWords;
        WrongValueKeyWords = wrongWords;
    }
}

/// <summary>
/// Булевый параметр помещения для хранения баллонов
/// </summary>
public class BoolParameter : Parameter
{
    /// <summary>
    /// Значение параметра
    /// </summary>
    public bool Value { get; set; }

    public bool DefalutValue;

    /// <summary>
    /// Установить значение параметра
    /// </summary>
    /// <param name="value">Устанавливаемое значение параметра</param>
    public void SetParameter(bool value)
    {
        Value = value;
    }

    /// <summary>
    /// Проверка значения на соответствие требованиям
    /// </summary>
    /// <returns>Несоответствие требованиям</returns>
    public bool Check()
    {
        if (Value == DefalutValue)
        {
            IsWrong = false;
            return false;
        }
        IsWrong = true;
        return true;
    }

    /// <summary>
    /// Генерация случайного значения параметра
    /// </summary>
    /// <returns>Полученное случайное значение параметра</returns>
    public bool GenerateRandomValue(float probability = 0.5f)
    {
        Value = RandomExtended.Bool(probability);
        return Value;
    }

    /// <summary>
    /// Получение значения параметра
    /// </summary>
    /// <returns>Полученное значение параметра</returns>
    public bool GetParameter(float probability = 0.5f)
    {
        if (IsRandom) { return GenerateRandomValue(probability); }
        else { return Value; }
    }

    public BoolParameter(string name, string description, bool defaultValue, List<string> idWords, List<string> wrongWords)
    {
        Name = name;
        Description = description;
        DefalutValue = defaultValue;
        IdentifierKeyWords = idWords;
        WrongValueKeyWords = wrongWords;
    }
}
