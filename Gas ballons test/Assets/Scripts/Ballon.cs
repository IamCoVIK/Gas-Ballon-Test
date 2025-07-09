using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Газы в баллонах
/// </summary>
public enum Gases
{
    /// <summary>
    /// Кислород
    /// </summary>
    Oxygen,
    /// <summary>
    /// Ацетилен
    /// </summary>
    Acetylene,
    /// <summary>
    /// Пропан
    /// </summary>
    Propane,
    /// <summary>
    /// Водород
    /// </summary>
    Hydrogen,
    /// <summary>
    /// Хлор
    /// </summary>
    Chlorine,
    /// <summary>
    /// Фтор
    /// </summary>
    Fluorine
}

/// <summary>
/// Состояние газового баллона
/// </summary>
public class Ballon : MonoBehaviour
{
    /// <summary>
    /// Серьёзность трещин, вмятин, коррозии корпуса баллона по 5-балльной шкале, где 0 - отсутствие повреждений (глубина повреждений не более 10% от толщины стенки)
    /// </summary>
    public IntParameter BodyDamage;
    /// <summary>
    /// Наличие масляных пятен на корпусе баллона
    /// </summary>
    public BoolParameter OilStains;
    /// <summary>
    /// Наличие повреждений башмака баллона
    /// </summary>
    public BoolParameter ShoeDamage;
    /// <summary>
    /// Наличие повреждений защитного колпака
    /// </summary>
    public BoolParameter CapDamage;
    /// <summary>
    /// Наличие товарного знака завода-изготовителя
    /// </summary>
    public BoolParameter TradeMark;
    /// <summary>
    /// Наличие номера баллона
    /// </summary>
    public BoolParameter NumberMark;
    /// <summary>
    /// Наличие даты изготовления и следующего освидетельствования (раз в 5 лет)
    /// </summary>
    public BoolParameter DatesMark;
    /// <summary>
    /// Наличие рабочего и пробного давления (в МПа или кгс/см²)
    /// </summary>
    public BoolParameter PressureMark;
    /// <summary>
    /// Наличие массы пустого баллона (с точностью до 0,2 кг)
    /// </summary>
    public BoolParameter EmptyMass;
    /// <summary>
    /// Наличие вместимости (литража)
    /// </summary>
    public BoolParameter VolumeMark;
    /// <summary>
    /// Наличие клейма ОТК (круглое, диаметром 10 мм)
    /// </summary>
    public BoolParameter OTKMark;
    /// <summary>
    /// Нарушение гермитичности вентиля 
    /// </summary>
    public BoolParameter ValveLeaks;
    /// <summary>
    /// Остаточное давление ниже нормы или нет, если баллон пуст
    /// </summary>
    public BoolParameter LeftPressure;
    /// <summary>
    /// Газ в баллоне
    /// </summary>
    public Gases GasType;
    /// <summary>
    /// Пустой ли баллон
    /// </summary>
    public bool IsEmpty;
    /// <summary>
    /// Давление газа в баллоне в (ед. изм.)
    /// </summary>
    public float GasPressure;

    public override string ToString()
    {
        return "Баллон с " + GasType.ToString();
    }

    public Ballon(Gases gas, bool isEmpty)
    {
        GasType = gas;
        IsEmpty = isEmpty;
        /*BodyDamage.GetParameter();
        OilStains.GetParameter();
        ShoeDamage.GetParameter();
        CapDamage.GetParameter();
        TradeMark.GetParameter();
        NumberMark.GetParameter();
        DatesMark.GetParameter();
        PressureMark.GetParameter();
        EmptyMass.GetParameter();
        VolumeMark.GetParameter();
        OTKMark.GetParameter();
        ValveLeaks.GetParameter();
        LeftPressure.GetParameter();*/
        /*if (IsEmpty)
        {
            GasPressure = 0;
        }
        else if (LeftPressure.Value)
        {
            GasPressure = 1; // ниже нормы
        }
        else
        {
            GasPressure = 1; // норма
        }*/
    }
}
