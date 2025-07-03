using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Общие требования к помещению для хранения баллонов
/// </summary>
public class StorageSituation
{
    /// <summary>
    /// Температура помещения в °C. Не должна превышать +35°C. Для охлаждения можно использовать полив полов водой и проветривание
    /// </summary>
    public int Temperature;

    /// <summary>
    /// Наличие вентиляции. Должна быть естественной или искусственной, особенно для взрывоопасных газов
    /// </summary>
    public bool Ventelation;

    /// <summary>
    /// Достаточность и исправность света по 5-балльной шкале, где 0 - света нет или он не работает, а 5 - свет исправен и достаточен
    /// </summary>
    public int Light;

    /// <summary>
    /// Наличие знака безопасности, запрещающего вход посторонних
    /// </summary>
    public bool SignTrespassing;

    /// <summary>
    /// Наличие знака безопасности, запрещающего курение
    /// </summary>
    public bool SignNoSmoking;

    /// <summary>
    /// Наличие знака безопасности, запрещающего использование открытого огня
    /// </summary>
    public bool SignNoFire;

    





    /// <summary>
    /// Генерация случайного состояния помещения для хранения баллонов
    /// </summary>
    public void GenerateStorageSituation()
    {
        Temperature = Random.Range(-15, 7) + 30;
        Ventelation = RandomBool();
        Light = Random.Range(0, 6);
        SignTrespassing = RandomBool();
        SignNoSmoking = RandomBool();
        SignNoFire = RandomBool();
    }

    private bool RandomBool()
    {
        if (Random.Range(0, 2) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
