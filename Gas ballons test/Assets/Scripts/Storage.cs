using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Состояние помещения для хранения баллонов относительно общих требований
/// </summary>
public class StorageParameters : MonoBehaviour
{
    /// <summary>
    /// Температура помещения в °C. Не должна превышать +35°C. Для охлаждения можно использовать полив полов водой и проветривание
    /// </summary>
    public IntParameter Temperature = new("Температура помещения в °C", 15, 40, 33);
    [SerializeField] private Transform TempScale;

    /// <summary>
    /// Наличие вентиляции. Должна быть естественной или искусственной, особенно для взрывоопасных газов
    /// </summary>
    public BoolParameter Ventelation = new("Наличие вентиляции", true);
    [SerializeField] private GameObject Vents;

    /// <summary>
    /// Достаточность и исправность света по 5-балльной шкале, где 0 - света нет или он не работает, а 5 - свет исправен и достаточен
    /// </summary>
    public IntParameter Light = new("Достаточность и исправность света", 0, 6, 4);
    [SerializeField] private LightControl LightControl;

    /// <summary>
    /// Наличие знака безопасности, запрещающего вход посторонних
    /// </summary>
    public BoolParameter SignTrespassing = new("Наличие знака безопасности, запрещающего вход посторонних", true);
    [SerializeField] private GameObject signTrespassing;

    /// <summary>
    /// Наличие знака безопасности, запрещающего курение
    /// </summary>
    public BoolParameter SignNoSmoking = new ("Наличие знака безопасности, запрещающего курение", true);
    [SerializeField] private GameObject signNoSmoking;

    /// <summary>
    /// Наличие знака безопасности, запрещающего использование открытого огня
    /// </summary>
    public BoolParameter SignNoFire = new("Наличие знака безопасности, запрещающего использование открытого огня", true);
    [SerializeField] private GameObject signNoFire;

    /// <summary>
    /// Вертикальное хранилище полных баллонов
    /// </summary>
    public List<Ballon> VerticalBallons;

    /// <summary>
    /// Наличие пустых баллонов в вертикальном хранилище
    /// </summary>
    public BoolParameter IsEmptyInVertical = new("Наличие пустых баллонов в вертикальном хранилище", false);

    /// <summary>
    /// Хранение одновременно кислорода + ацетилен/пропан/водород или ацетилен + хлор или водород + фтор
    /// </summary>
    public BoolParameter IsForbiddenGasMixes = new("Хранение одновременно кислорода + ацетилен/пропан/водород или ацетилен + хлор или водород + фтор", false);

    /// <summary>
    /// Горизонтальное хранилище пустых баллонов
    /// </summary>
    public List<Ballon> HorisontalBallons;

    /// <summary>
    /// Наличие полных баллонов в горизонтальном хранилище
    /// </summary>
    public BoolParameter IsFullInHorisontal = new("Наличие полных баллонов в горизонтальном хранилище", false);

    /// <summary>
    /// Высота штабелей больше 1,5 м
    /// </summary>
    public BoolParameter IsHorisontalTooHigh = new("Высота штабелей больше 1,5 м", false);

    /// <summary>
    /// Расстояние до радиаторов в дециметрах (должно быть более 1 м)
    /// </summary>
    public IntParameter RadiatorDistance = new("Расстояние до радиаторов в дециметрах", 3, 15, 10);
    [SerializeField] private Transform radiators;

    /// <summary>
    /// Найти случайный свободный индекс в списке баллонов
    /// </summary>
    /// <param name="l">Список индексов</param>
    /// <returns>Найденный индекс</returns>
    private int FindFreeIndex(List<int> l)
    {
        int a = Random.Range(0, 12);
        while (!l.Contains(a))
        {
            a = Random.Range(0, 12);
        }
        return a;
    }

    /// <summary>
    /// Случайное заполнение списка вертикально хранящихся баллонов
    /// </summary>
    private void GenerateVerticalBallons()
    {
        VerticalBallons = new();
        List<int> occupied = new(); 
        if (IsEmptyInVertical.Value)
        {
            int a = Random.Range(0, 12);
            VerticalBallons[a] = new(Gases.Oxygen, true);
            occupied.Add(a);
        }
        if (IsForbiddenGasMixes.Value)
        {
            int a = FindFreeIndex(occupied);
            occupied.Add(a);
            int b = FindFreeIndex(occupied);
            occupied.Add(b);
            switch (Random.Range(0, 3))
            {
                case 0:
                    VerticalBallons[a] = new(Gases.Oxygen, false);
                    switch (Random.Range(0, 3))
                    {
                        case 0:
                            VerticalBallons[b] = new(Gases.Acetylene, false);
                            break;
                        case 1:
                            VerticalBallons[b] = new(Gases.Propane, false);
                            break;
                        case 2:
                            VerticalBallons[b] = new(Gases.Hydrogen, false);
                            break;
                    }
                    break;
                case 1:
                    VerticalBallons[a] = new(Gases.Acetylene, false);
                    VerticalBallons[b] = new(Gases.Chlorine, false);
                    break;
                case 2:
                    VerticalBallons[a] = new(Gases.Hydrogen, false);
                    VerticalBallons[b] = new(Gases.Fluorine, false);
                    break;
            }
        }
        for (int i = 0; i < 12; i++)
        {
            if (!occupied.Contains(i))
            {
                VerticalBallons.Add(new Ballon(Gases.Oxygen, false));
            }
        }
    }

    private void GenerateHorisontalBallons()
    {
        HorisontalBallons = new List<Ballon>();
        int amount = 12;
        int full = -1;
        if (IsFullInHorisontal.Value)
        {
            full = Random.Range(0, 12);
            HorisontalBallons[full] = new Ballon(Gases.Oxygen, false);
        }
        if (IsHorisontalTooHigh.Value)
        {
            amount = 16;
        }
        for (int i = 0; i < amount; i++)
        {
            if (i != full)
            {
                HorisontalBallons.Add(new Ballon(Gases.Oxygen, true));
            }
        }
    }

    /// <summary>
    /// Генерация состояния помещения для хранения баллонов
    /// </summary>
    public void GenerateStorageSituation()
    {
        Temperature.GetParameter();
        Ventelation.GetParameter();
        Light.GetParameter();
        SignTrespassing.GetParameter();
        SignNoSmoking.GetParameter();
        SignNoFire.GetParameter();
        //IsEmptyInVertical.GetParameter();
        //IsForbiddenGasMixes.GetParameter();
        //IsFullInHorisontal.GetParameter();
        //IsHorisontalTooHigh.GetParameter();
        //GenerateVerticalBallons();
        //GenerateHorisontalBallons();
        RadiatorDistance.GetParameter();
    }

    private void SetTempScale()
    {
        int i = Temperature.Value - Temperature.MinValue;
        float a = (-0.06064f - (-0.16334f)) / (Temperature.MaxValue - Temperature.MinValue) * i;
        float b = (0.3431705f - 0.2404572f) / (Temperature.MaxValue - Temperature.MinValue) * i;
        TempScale.localPosition = new Vector3(0, -0.16334f + a, -0.0054f);
        TempScale.localScale = new Vector3(2.127844f, 0.2404572f + b, 0.1016777f);
        if (Temperature.Check())
        {
            // Активация возможности проверки
        }
        Debug.Log($"{Temperature} - {Temperature.Value}");
    }

    private void RemoveVentsOrNot()
    {
        if (Ventelation.Check())
        {
            Vents.SetActive(false);
            // Активация возможности проверки
        }
        else
        {
            Vents.SetActive(true);
        }
        Debug.Log($"{Ventelation} - {Ventelation.Value}");
    }

    private void SetLightStatus()
    {
        LightControl.Status = Light.Value;
        if (Light.Check())
        {
            // Активация возможности проверки
        }
        Debug.Log($"{Light} - {Light.Value}");
    }

    private void SetSigns()
    {
        if (SignTrespassing.Check())
        {
            signTrespassing.SetActive(false);
            // Активация возможности проверки
        }
        else
        {
            signTrespassing.SetActive(true);
        }
        Debug.Log($"{SignTrespassing} - {SignTrespassing.Value}");
        if (SignNoSmoking.Check())
        {
            signNoSmoking.SetActive(false);
            // Активация возможности проверки
        }
        else
        {
            signNoSmoking.SetActive(true);
        }
        Debug.Log($"{SignNoSmoking} - {SignNoSmoking.Value}");
        if (SignNoFire.Check())
        {
            signNoFire.SetActive(false);
            // Активация возможности проверки
        }
        else
        {
            signNoFire.SetActive(true);
        }
        Debug.Log($"{SignNoFire} - {SignNoFire.Value}");
    }

    private void SetRadiators()
    {
        radiators.localPosition = new Vector3(((float)RadiatorDistance.Value / 10) - 1, 0, 0);
        if (RadiatorDistance.Check())
        {
            // Активация возможности проверки
        }
        Debug.Log($"{RadiatorDistance} - {RadiatorDistance.Value}");
    }

    private void Start()
    {
        GenerateStorageSituation();
        SetTempScale();
        RemoveVentsOrNot();
        SetLightStatus();
        SetSigns();
        SetRadiators();
    }
}
