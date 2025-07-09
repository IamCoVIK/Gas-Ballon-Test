using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    /// Включена ли случайная генерация значения параметра
    /// </summary>
    public bool IsRandom { get; set; } = true;
    /// <summary>
    /// Соответствие значения требованиям
    /// </summary>
    public bool IsWrong { get; set; }

    /// <summary>
    /// Переключение случайной генерации параметра
    /// </summary>
    /// <returns>Включена ли теперь случайная генерация параметра</returns>
    public bool ToggleRandom()
    {
        return IsRandom.Toggle();
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
    
    public int DefalutValue;
    public int MinValue;
    public int MaxValue;

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
        if (Value >= DefalutValue)
        {
            IsWrong = false;
            return false;
        }
        IsWrong = true;
        return true;
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

    public IntParameter(string name, int minValueInclusive, int maxValueExclusive, int defaultValue)
    {
        Name = name;
        MinValue = minValueInclusive;
        MaxValue = maxValueExclusive;
        DefalutValue = defaultValue;
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

    public BoolParameter(string name, bool defaultValue)
    {
        Name = name;
        DefalutValue = defaultValue;
    }
}
