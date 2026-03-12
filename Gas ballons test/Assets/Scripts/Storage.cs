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
        "температура", "температуры", "температуре", "температуру", "температурой", "температурою",
    };
    private static List<string> TemperatureWrongWords = new List<string>() {
        "выше", "превышает", "больше"
    };
    /// <summary>
    /// Температура помещения в °C. Не должна превышать +35°C. Для охлаждения можно использовать полив полов водой и проветривание
    /// </summary>
    public IntParameter Temperature = new("Температура помещения в °C",
        "Не должна превышать +35°C.",
        25, 40, 35, false, TemperatureIdWords, TemperatureWrongWords);
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
    public BoolParameter Ventelation = new("Наличие вентиляции",
        "Должна быть естественной или искусственной, особенно для взрывоопасных газов",
        true, VentelationIdWords, VentelationWrongWords);
    [SerializeField] private GameObject Vents;

    private static List<string> LightIdWords = new List<string>() {
        "свет", "освещение", "света", "освещения"
    };
    private static List<string> LightWrongWords = new List<string>() {
        "сломан", "сломано", "не исправно", "не исправен", "не достаточен", "не достаточно", "недостаточно"
    };
    /// <summary>
    /// Достаточность и исправность света по 5-балльной шкале, где 0 - света нет или он не работает, а 5 - свет исправен и достаточен
    /// </summary>
    public IntParameter Light = new("Достаточность и исправность света", 
        "",
        0, 6, 4, true, LightIdWords, LightWrongWords);
    [SerializeField] private LightControl LightControl;

    private static List<string> SignTrespassingIdWords = new List<string>() {
        "знак вход воспрещён", "знак вход воспрещен", "знак посторонним вход воспрещён", "знак посторонним вход воспрещен", "знак по сторонним вход воспрещён", "знак по сторонним вход воспрещен",
        "знака вход воспрещён", "знака вход воспрещен", "знака посторонним вход воспрещён", "знака посторонним вход воспрещен", "знака по сторонним вход воспрещён", "знака по сторонним вход воспрещен",
        "знак вход в воспрещён", "знак вход в воспрещен", "знак посторонним вход в воспрещён", "знак посторонним вход в воспрещен", "знак по сторонним вход в воспрещён", "знак по сторонним вход в воспрещен",
        "знака вход в воспрещён", "знака вход в воспрещен", "знака посторонним вход в воспрещён", "знака посторонним вход в воспрещен", "знака по сторонним вход в воспрещён", "знака по сторонним вход в воспрещен",
        "знак посторонним вход в спрещён", "знак посторонним вход в спрещен",
        "знака посторонним вход в спрещён", "знака посторонним вход в спрещен",
        "знак запрещающий вход посторонних", "знак запрещающий вход по сторонних", "знак запрещающий вход",
        "знака запрещающий вход посторонних", "знака запрещающий вход по сторонних", "знака запрещающий вход",
    };
    private static List<string> SignTrespassingWrongWords = new List<string>() {
        "отсутствует", "нет", "нету",
    };
    /// <summary>
    /// Наличие знака безопасности, запрещающего вход посторонних
    /// </summary>
    public BoolParameter SignTrespassing = new("Наличие знака безопасности, запрещающего вход посторонних", "", true, SignTrespassingIdWords, SignTrespassingWrongWords);
    [SerializeField] private GameObject signTrespassing;

    private static List<string> SignNoSmokingIdWords = new List<string>() {
        "знак не курить", "знак нельзя курить", "знак курение запрещено", "знак запрещается курить",
        "знака не курить", "знака нельзя курить", "знака курение запрещено", "знака запрещается курить",
    };
    private static List<string> SignNoSmokingWrongWords = new List<string>() {
        "отсутствует", "нет", "нету",
    };
    /// <summary>
    /// Наличие знака безопасности, запрещающего курение
    /// </summary>
    public BoolParameter SignNoSmoking = new ("Наличие знака безопасности, запрещающего курение", "", true, SignNoSmokingIdWords, SignNoSmokingWrongWords);
    [SerializeField] private GameObject signNoSmoking;

    private static List<string> SignNoFireIdWords = new List<string>() {
        "знак запрещается использование открытого огня", "знак запрещающий использование открытого огня",
        "знака запрещается использование открытого огня", "знака запрещающий использование открытого огня",
        "знак запрещается использования открытого огня", "знак запрещающий использования открытого огня",
        "знака запрещается использования открытого огня", "знака запрещающий использования открытого огня",
        "знак запрещается открытый огонь", "знака запрещается открытый огонь",
        "знак запрещающий открытый огонь", "знака запрещающий открытый огонь",
        "знак запрещается огонь", "знака запрещается огонь",
        "знак запрещающий огонь", "знака запрещающий огонь",
    };
    private static List<string> SignNoFireWrongWords = new List<string>() {
        "отсутствует", "нет", "нету",
    };
    /// <summary>
    /// Наличие знака безопасности, запрещающего использование открытого огня
    /// </summary>
    public BoolParameter SignNoFire = new("Наличие знака безопасности, запрещающего использование открытого огня", "", true, SignNoFireIdWords, SignNoFireWrongWords);
    [SerializeField] private GameObject signNoFire;

    private static List<string> FireExtPresenceIdWords = new List<string>() {
        "огнетушитель", "огнетушителя", "огнятушитель", "огнятушителя", "огнитушитель", "огнитушителя",
        "огне тушитель", "огне тушителя", "огня тушитель", "огня тушителя", "огни тушитель", "огни тушителя",
        "огнет ушитель", "огнет ушителя", "огнят ушитель", "огнят ушителя", "огнит ушитель", "огнит ушителя",
    };
    private static List<string> FireExtPresenceWrongWords = new List<string>() {
        "отсутствует", "нет", "нету",
    };
    /// <summary>
    /// Наличие огнетушителя
    /// </summary>
    public BoolParameter FireExtPresence = new("Наличие огнетушителя", "", true, FireExtPresenceIdWords, FireExtPresenceWrongWords);
    [SerializeField] private GameObject fireExtSign;

    private static List<string> IsEmptyInVerticalIdWords = new List<string>() {
        "хранилище полных баллонов", "хранилище наполненных баллонов",
        "хранилище полных", "хранилище наполненных",
        "среди полных", "среди наполненных", "в полных", "в наполненных",
    };
    private static List<string> IsEmptyInVerticalWrongWords = new List<string>() {
        "пустые", "пустой"
    };
    /// <summary>
    /// Наличие пустых баллонов в вертикальном хранилище
    /// </summary>
    public BoolParameter IsEmptyInVertical = new("Наличие пустых баллонов в вертикальном хранилище", 
        "Пустые баллоны должны находиться в соответствующем хранилище", 
        false, IsEmptyInVerticalIdWords, IsEmptyInVerticalWrongWords);

    private static List<string> IsFullInHorisontalIdWords = new List<string>() {
        "хранилище пустых баллонов",
        "хранилище пустых",
        "среди пустых", "в пустых",
    };
    private static List<string> IsFullInHorisontalWrongWords = new List<string>() {
        "полные", "наполненные", "полный"
    };
    /// <summary>
    /// Наличие полных баллонов в горизонтальном хранилище
    /// </summary>
    public BoolParameter IsFullInHorisontal = new("Наличие полных баллонов в горизонтальном хранилище",
        "Полные баллоны должны находиться в соответствующем хранилище", 
        false, IsFullInHorisontalIdWords, IsFullInHorisontalWrongWords);

    private static List<string> IsHorisontalTooHighIdWords = new List<string>() {
        "штабель", "штабеля", "стелаж", "стелажа"
    };
    private static List<string> IsHorisontalTooHighWrongWords = new List<string>() {
        "выше", "больше", "превышает",
    };
    /// <summary>
    /// Высота штабелей. Должна быть меньше 1,5 м
    /// </summary>
    public BoolParameter IsHorisontalTooHigh = new("Высота штабелей", 
        "Должна быть меньше 1,5 м", 
        false, IsHorisontalTooHighIdWords, IsHorisontalTooHighWrongWords);
    [SerializeField] private GameObject newBallonShelf;

    private static List<string> RadiatorDistanceIdWords = new List<string>() {
        "батареи", "батарея", "отопление", "отопления", "радиаторы", "радиатора",
    };
    private static List<string> RadiatorDistanceWrongWords = new List<string>() {
        "близко", "ближе", "близок", "близки", "рядом"
    };
    /// <summary>
    /// Расстояние до радиаторов в дециметрах (должно быть более 1 м)
    /// </summary>
    public IntParameter RadiatorDistance = new("Расстояние до радиаторов в дециметрах", 
        "Должно быть более 1 м", 
        3, 15, 10, true, RadiatorDistanceIdWords, RadiatorDistanceWrongWords);
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
        FireExtPresence.GetParameter();
        IsEmptyInVertical.GetParameter();
        IsFullInHorisontal.GetParameter();
        IsHorisontalTooHigh.GetParameter();
        RadiatorDistance.GetParameter();

        SetTempScale();
        RemoveVentsOrNot();
        SetLightStatus();
        SetSigns();
        SetFireExt();
        SetEmptyInFull();
        SetFullInEmpty();
        SetHorisontalHeight();
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

    private void SetFireExt()
    {
        if (FireExtPresence.Check())
        {
            fireExt.SetActive(false);
            fireExtSign.SetActive(false);
            // Активация возможности проверки
        }
        else
        {
            fireExt.SetActive(true);
            fireExtSign.SetActive(true);
        }
        Debug.Log($"{FireExtPresence} - {FireExtPresence.Value}");
    }

    private void SetEmptyInFull()
    {
        if (IsEmptyInVertical.Check())
        {
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("FullBallons"))
            {
                if (!i.activeSelf)
                {
                    continue;
                }
                Ballon[] bs = i.GetComponentsInChildren<Ballon>();
                if (bs.Length <= 0)
                {
                    continue;
                }
                //int a = Random.Range(0, bs.Length);
                for (int j = 0; j <= bs.Length / 2; j++)
                {
                    bs[j].IsEmpty = true;
                    bs[j].GasPressure = 0.1f;
                }
            }
            // Активация возможности проверки
        }
        else
        {
            
        }
        Debug.Log($"{IsEmptyInVertical} - {IsEmptyInVertical.Value}");
    }

    private void SetFullInEmpty()
    {
        if (IsFullInHorisontal.Check())
        {
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("EmptyBallons"))
            {
                if (!i.activeSelf)
                {
                    continue;
                }
                Ballon[] bs = i.GetComponentsInChildren<Ballon>();
                if (bs.Length <= 0)
                {
                    continue;
                }
                //int a = Random.Range(0, bs.Length);
                for (int j = 0; j <= bs.Length / 2; j++)
                {
                    bs[j].IsEmpty = false;
                    bs[j].GasPressure = 20f;
                }
            }
            // Активация возможности проверки
        }
        else
        {

        }
        Debug.Log($"{IsFullInHorisontal} - {IsFullInHorisontal.Value}");
    }

    private void SetHorisontalHeight()
    {
        if (IsHorisontalTooHigh.Check())
        {
            newBallonShelf.SetActive(true);
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("EmptyBallons"))
            {
                i.transform.localPosition = new Vector3(0, 0.365f, 0);
            }
            // Активация возможности проверки
        }
        else
        {
            newBallonShelf.SetActive(false);
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("EmptyBallons"))
            {
                i.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        Debug.Log($"{IsHorisontalTooHigh} - {IsHorisontalTooHigh.Value}");
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
    private List<Parameter> CheckedList = new List<Parameter>();

    private void FillCheckList()
    {
        CheckedList.Clear();
        CheckList.Clear();
        CheckList.Add(Temperature);
        CheckList.Add(Ventelation);
        CheckList.Add(Light);
        CheckList.Add(SignTrespassing);
        CheckList.Add(SignNoSmoking);
        CheckList.Add(SignNoFire);
        CheckList.Add(FireExtPresence);
        CheckList.Add(IsEmptyInVertical);
        CheckList.Add(IsFullInHorisontal);
        CheckList.Add(IsHorisontalTooHigh);
        CheckList.Add(RadiatorDistance);
    }

    public bool CheckVoiceInput(string text)
    {
        List<Parameter> checkedParams = new List<Parameter>();
        bool result = false;
        foreach (Parameter p in CheckList)
        {
            if (p.TextCheck(text) && !checkedParams.Contains(p))
            {
                if (!CheckedList.Contains(p))
                {
                    CheckedList.Add(p);
                }
                checkedParams.Add(p);
                result = true;
            }
        }
        return result;
    }

    public List<string> MissedParams()
    {
        List<string> result = new();
        foreach (Parameter p in CheckList)
        {
            if (!CheckedList.Contains(p) && p.IsWrong)
            {
                result.Add($"{p.Name} {p.Description}");
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
