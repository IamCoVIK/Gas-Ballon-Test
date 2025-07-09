using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Дополнительные методы на основе встроенного в Unity класса Random
/// </summary>
public static class RandomExtended
{
    /// <summary>
    /// Получение случайного значения true или false
    /// </summary>
    /// <param name="probability"> Вероятность получения значения true в диапазоне [0.0...1.0] (по умолчанию 0.5)</param>
    /// <returns>Cлучайное значение true или false</returns>
    public static bool Bool(float probability = 0.5f)
    {
        if (probability < 0)
        {
            probability = 0;
        }
        if (probability > 1)
        {
            probability = 1;
        }
        if (Random.value <= probability)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

/// <summary>
/// Расширения для типа bool
/// </summary>
public static class BoolExtention
{
    /// <summary>
    /// Переключение значения bool на противоположное
    /// </summary>
    /// <param name="obj">Целевая переменная</param>
    /// <returns>Новое значение целевой переменной</returns>
    public static bool Toggle(this bool obj)
    {
        obj = !obj;
        return obj;
    }
}
