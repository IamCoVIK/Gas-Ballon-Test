using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Reductor : MonoBehaviour
{
    public Transform Pointer;
    public float MinAngle = -90f;
    public float MaxAngle = 90f;
    public float MaxPressure = 200f;
    public float AttachDistance = 0.1f;
    public Transform ConnectionPoint;
    public Transform ValveHandle; // Ручка вентиля
    public float ValveRotationAngle = 90f; // Угол поворота вентиля для открытия

    public Ballon _attachedBallon;
    private Rigidbody _rb;
    private Interactable _interactable;

    private Quaternion _initialValveRotation;

    public LayerMask excludedLayer;

    public bool block;
    public bool IsValveOpen = false; // Состояние вентиля (открыт/закрыт)
    public bool IsAttachedAndLocked = false; // Блокировка отсоединения

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _interactable = GetComponent<Interactable>();

        if (_interactable == null)
        {
            Debug.LogError("Regulator требует SteamVR_Interactable компонент!");
        }
        _initialValveRotation = ValveHandle.localRotation;

        block = false;
        MinAngle = Pointer.rotation.x;
        MaxAngle += Pointer.rotation.x;
    }

    void Update()
    {
        // Если редуктор не присоединен и его держат, проверяем близость к баллону
        if (_attachedBallon == null && _interactable.isHovering)
        {
            TryAttachToNearbyBallon();
        }

        //Если редуктор присоединен, то обновляем стрелку на редукторе
        if (_attachedBallon != null && IsValveOpen)
        {
            UpdatePressureDisplay();
        }
    }

    void TryAttachToNearbyBallon()
    {
        Collider[] hitColliders = Physics.OverlapSphere(ConnectionPoint.position, AttachDistance);
        Debug.Log($"Найдено коллайдеров в радиусе {AttachDistance}: {hitColliders.Length}");
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("BallonConnector"))
            {
                Ballon cylinder = hitCollider.GetComponentInParent<Ballon>();
                if (cylinder != null)
                {
                    Debug.Log("GasCylinder найден: " + cylinder.ToString());
                    if (cylinder.CanAttachReductor(this))
                    {
                        AttachToBallon(cylinder);
                        break;
                    }
                    else
                    {
                        Debug.Log("CanAttachRegulator вернул false для " + cylinder.ToString());
                    }
                }
            }
            
        }
    }

    public void AttachToBallon(Ballon cylinder)
    {
        if (block) return;

        _attachedBallon = cylinder;
        Debug.Log("AttachToCylinder called with cylinder: " + cylinder.name);

        // Убираем редуктор из руки
        if (GetComponent<Interactable>().attachedToHand != null)
        {
            Hand hand = GetComponent<Interactable>().attachedToHand;
            hand.DetachObject(gameObject);
            hand.HoverUnlock(GetComponent<Interactable>());
            Debug.Log("Detached from hand.");
        }

        // Отключаем Rigidbody
        _rb.isKinematic = true;
        _rb.detectCollisions = true; // Отключаем обнаружение столкновений
        _rb.excludeLayers = excludedLayer;

        //_interactable.enabled = !IsValveOpen;
        _interactable.enabled = true;

        transform.SetParent(cylinder.transform);
        Vector3 offset = ConnectionPoint.position - transform.position;
        Vector3 targetPosition = cylinder.GetReductorMountPoint() - offset;
        transform.position = targetPosition;
        transform.rotation = cylinder.transform.rotation;

        block = true;

        //cylinder.AttachReductor(this);
        //UpdatePressureDisplay(); // Не показываем давление сразу
        //UpdateValveRotation(ValveRotationAngle); //Вентиль открыт
    }

    public void DetachFromBallon()
    {
        Debug.Log("DetachFromCylinder called");

        if (IsAttachedAndLocked) // Если заблокировано, выходим
        {
            Debug.Log("DetachFromCylinder: Отсоединение заблокировано, Valve Open.");
            return;
        }

        if (_attachedBallon == null) return;

        //_attachedBallon.DetachReductor();
        _attachedBallon = null;
        transform.SetParent(null);

        // Включаем Rigidbody
        _rb.isKinematic = false;
        _rb.detectCollisions = true; // Включаем обнаружение столкновений
        _rb.excludeLayers = 0;

        //Сбрасываем скорость
        //_rb.velocity = Vector3.zero;
        //_rb.angularVelocity = Vector3.zero;

        _interactable.enabled = true;

        //Сброс вращения вентиля при отсоединении
        //UpdateValveRotation(-ValveRotationAngle);
        //UpdatePressureDisplay(); // Сбросить показания давления
        //IsValveOpen = false;
    }

    public void UpdatePressureDisplay()
    {
        float pressure = 0f;

        if (_attachedBallon != null && IsValveOpen)
        {
            pressure = _attachedBallon.GasPressure;
        }
        if (!IsValveOpen)
        {
            Pointer.localRotation = Quaternion.Euler(0, 0, 0);
            //float angle = Mathf.Lerp(MinAngle, MaxAngle, pressure / MaxPressure);
            float angle = (MaxAngle - MinAngle) / MaxPressure * pressure;
            Pointer.localRotation = Quaternion.Euler(angle, 0, 0);
            UpdateValveRotation(90);
        }
        else
        {
            Pointer.localRotation = Quaternion.Euler(0, 0, 0);
            UpdateValveRotation(0);
        }
        
    }

    //Метод для открытия/закрытия вентиля
    public void ToggleValve()
    {
        if (_attachedBallon != null)
        {
            IsValveOpen = !IsValveOpen;
            IsAttachedAndLocked = IsValveOpen; // Блокируем/разблокируем отсоединение
            Debug.Log("Valve is now " + (IsValveOpen ? "Open" : "Closed"));
            //_interactable.enabled = !IsValveOpen;
            UpdatePressureDisplay();
        }
    }

    //Метод для поворота вентиля
    private void UpdateValveRotation(float angleChange)
    {
        if (ValveHandle == null) return;

        Quaternion targetRotation = _initialValveRotation * Quaternion.Euler(angleChange, 0, 0);
        ValveHandle.localRotation = targetRotation;
    }
}
