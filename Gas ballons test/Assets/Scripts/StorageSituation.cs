using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ���������� � ��������� ��� �������� ��������
/// </summary>
public class StorageSituation
{
    /// <summary>
    /// ����������� ��������� � �C. �� ������ ��������� +35�C. ��� ���������� ����� ������������ ����� ����� ����� � �������������
    /// </summary>
    public int Temperature;

    /// <summary>
    /// ������� ����������. ������ ���� ������������ ��� �������������, �������� ��� ������������� �����
    /// </summary>
    public bool Ventelation;

    /// <summary>
    /// ������������� � ����������� ����� �� 5-�������� �����, ��� 0 - ����� ��� ��� �� �� ��������, � 5 - ���� �������� � ����������
    /// </summary>
    public int Light;

    /// <summary>
    /// ������� ����� ������������, ������������ ���� �����������
    /// </summary>
    public bool SignTrespassing;

    /// <summary>
    /// ������� ����� ������������, ������������ �������
    /// </summary>
    public bool SignNoSmoking;

    /// <summary>
    /// ������� ����� ������������, ������������ ������������� ��������� ����
    /// </summary>
    public bool SignNoFire;

    





    /// <summary>
    /// ��������� ���������� ��������� ��������� ��� �������� ��������
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
