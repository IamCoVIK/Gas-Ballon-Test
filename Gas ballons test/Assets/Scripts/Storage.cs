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
    private static List<string> TemperatureIdWords = new List<string>() { 
        "температура",
    };
    private static List<string> TemperatureWrongWords = new List<string>() {
        "выше", "выше нормы", "превышает", "больше"
    };
    /// <summary>
    /// Температура помещения в °C. Не должна превышать +35°C. Для охлаждения можно использовать полив полов водой и проветривание
    /// </summary>
    public IntParameter Temperature = new("Температура помещения в °C", 25, 40, 35, false, TemperatureIdWords, TemperatureWrongWords);
    [SerializeField] private Transform TempScale;

    private static List<string> VentelationIdWords = new List<string>() {
        "вентиляция", "вентиляции"
    };
    private static List<string> VentelationWrongWords = new List<string>() {
        "отсутствует", "нет", "нету",
    };
    /// <summary>
    /// Наличие вентиляции. Должна быть естественной или искусственной, особенно для взрывоопасных газов
    /// </summary>
    public BoolParameter Ventelation = new("Наличие вентиляции", true, VentelationIdWords, VentelationWrongWords);
    [SerializeField] private GameObject Vents;

    private static List<string> LightIdWords = new List<string>() {
        "свет", "освещение"
    };
    private static List<string> LightWrongWords = new List<string>() {
        "сломан", "сломано",
    };
    /// <summary>
    /// Достаточность и исправность света по 5-балльной шкале, где 0 - света нет или он не работает, а 5 - свет исправен и достаточен
    /// </summary>
    public IntParameter Light = new("Достаточность и исправность света", 0, 6, 4, true, LightIdWords, LightWrongWords);
    [SerializeField] private LightControl LightControl;

    /// <summary>
    /// Наличие знака безопасности, запрещающего вход посторонних
    /// </summary>
    public BoolParameter SignTrespassing = new("Наличие знака безопасности, запрещающего вход посторонних", true, new List<string>(), new List<string>());
    [SerializeField] private GameObject signTrespassing;

    /// <summary>
    /// Наличие знака безопасности, запрещающего курение
    /// </summary>
    public BoolParameter SignNoSmoking = new ("Наличие знака безопасности, запрещающего курение", true, new List<string>(), new List<string>());
    [SerializeField] private GameObject signNoSmoking;

    /// <summary>
    /// Наличие знака безопасности, запрещающего использование открытого огня
    /// </summary>
    public BoolParameter SignNoFire = new("Наличие знака безопасности, запрещающего использование открытого огня", true, new List<string>(), new List<string>());
    [SerializeField] private GameObject signNoFire;

    /// <summary>
    /// Вертикальное хранилище полных баллонов
    /// </summary>
    public List<Ballon> VerticalBallons;

    /// <summary>
    /// Наличие пустых баллонов в вертикальном хранилище
    /// </summary>
    public BoolParameter IsEmptyInVertical = new("Наличие пустых баллонов в вертикальном хранилище", false, new List<string>(), new List<string>());

    /// <summary>
    /// Хранение одновременно кислорода + ацетилен/пропан/водород или ацетилен + хлор или водород + фтор
    /// </summary>
    public BoolParameter IsForbiddenGasMixes = new("Хранение одновременно кислорода + ацетилен/пропан/водород или ацетилен + хлор или водород + фтор", false, new List<string>(), new List<string>());

    /// <summary>
    /// Горизонтальное хранилище пустых баллонов
    /// </summary>
    public List<Ballon> HorisontalBallons;

    /// <summary>
    /// Наличие полных баллонов в горизонтальном хранилище
    /// </summary>
    public BoolParameter IsFullInHorisontal = new("Наличие полных баллонов в горизонтальном хранилище", false, new List<string>(), new List<string>());

    /// <summary>
    /// Высота штабелей больше 1,5 м
    /// </summary>
    public BoolParameter IsHorisontalTooHigh = new("Высота штабелей больше 1,5 м", false, new List<string>(), new List<string>());

    private static List<string> RadiatorDistanceIdWords = new List<string>() {
        "батареи"
    };
    private static List<string> RadiatorDistanceWrongWords = new List<string>() {
        "близко"
    };
    /// <summary>
    /// Расстояние до радиаторов в дециметрах (должно быть более 1 м)
    /// </summary>
    public IntParameter RadiatorDistance = new("Расстояние до радиаторов в дециметрах", 3, 15, 10, true, RadiatorDistanceIdWords, RadiatorDistanceWrongWords);
    [SerializeField] private Transform radiators;

    [SerializeField] private GameObject referenceMainDoors;
    private GameObject mainDoors;
    [SerializeField] private GameObject referenceBallonDoors;
    private GameObject ballonDoors;
    [SerializeField] private GameObject referenceBallons;
    [SerializeField] private GameObject referenceReductors;
    [SerializeField] private GameObject fireExt;
    [SerializeField] private Transform fireExtStartPosition;

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

        SetTempScale();
        RemoveVentsOrNot();
        SetLightStatus();
        SetSigns();
        SetRadiators();
    }

    private void SetTempScale()
    {
        // 40 locpos = -0.06064 locsca = 0.3431705
        // 0 locpos = -0.22372 locsca = 0.180049
        //int i = Temperature.Value - Temperature.MinValue;
        //float a = (-0.06064f - (-0.16334f)) / (Temperature.MaxValue - Temperature.MinValue) * i;
        //float b = (0.3431705f - 0.2404572f) / (Temperature.MaxValue - Temperature.MinValue) * i;
        int i = Temperature.Value;
        float a = (-0.06064f - (-0.22372f)) / Temperature.MaxValue * i;
        float b = (0.3431705f - 0.180049f) / Temperature.MaxValue * i;
        TempScale.localPosition = new Vector3(0, -0.22372f + a, -0.0054f);
        TempScale.localScale = new Vector3(2.127844f, 0.180049f + b, 0.1016777f);
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

    public void ResetAllPhysicObjs()
    {
        Destroy(mainDoors);
        mainDoors = Instantiate(referenceMainDoors, referenceMainDoors.transform.parent);
        referenceMainDoors.SetActive(false);
        mainDoors.SetActive(true);

        Destroy(ballonDoors);
        ballonDoors = Instantiate(referenceBallonDoors, referenceBallonDoors.transform.parent);
        referenceBallonDoors.SetActive(false);
        ballonDoors.SetActive(true);

        referenceBallons.SetActive(false);
        foreach (GameObject ballon in GameObject.FindGameObjectsWithTag("Ballon"))
        {
            if (ballon.activeSelf)
            {
                Destroy(ballon);
            }
        }
        Instantiate(referenceBallons, referenceBallons.transform.parent).SetActive(true);

        referenceReductors.SetActive(false);
        foreach (GameObject reductor in GameObject.FindGameObjectsWithTag("Reductor"))
        {
            if (reductor.activeSelf)
            {
                Destroy(reductor);
            }
        }
        Instantiate(referenceReductors, referenceReductors.transform.parent).SetActive(true);

        fireExt.transform.position = fireExtStartPosition.position;
        fireExt.transform.rotation = fireExtStartPosition.rotation;

        FillCheckList();
    }


    private List<Parameter> CheckList = new List<Parameter>();
    
    private void FillCheckList()
    {
        CheckList.Clear();
        CheckList.Add(Temperature);
        CheckList.Add(Ventelation);
        CheckList.Add(RadiatorDistance);
        CheckList.Add(Light);
        // добавить все параметры
    }

    public bool CheckVoiceInput(string text)
    {
        List<Parameter> checkedParams = new List<Parameter>();
        bool result = false;
        foreach (Parameter p in CheckList)
        {
            if (p.TextCheck(text) && !checkedParams.Contains(p))
            {
                checkedParams.Add(p);
                result = true;
            }
        }
        return result;
    }

    private void Start()
    {
        ResetAllPhysicObjs();

        GenerateStorageSituation();

        FillCheckList();
    }
}
