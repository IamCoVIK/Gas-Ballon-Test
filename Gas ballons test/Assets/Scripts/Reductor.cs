using UnityEngine;
using Valve.VR.InteractionSystem;

public class Reductor : MonoBehaviour
{
    public Transform Pointer;
    public float MinAngle = 0f;
    public float MaxAngle = 180f;
    public float MaxPressure = 200f;
    public float AttachDistance = 0.1f;
    public Transform ConnectionPoint;
    public Transform ValveHandle; // Ручка вентиля
    public float ValveRotationAngle = 90f; // Угол поворота вентиля для открытия

    public Ballon _attachedBallon;
    private Rigidbody _rb;
    private Interactable _interactable;

    private Quaternion _initialPointerRotation;
    private Quaternion _initialValveRotation;

    public LayerMask excludedLayer;

    public bool block;
    public bool IsValveOpen = false; // Состояние вентиля (открыт/закрыт)

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _interactable = GetComponent<Interactable>();

        if (_interactable == null)
        {
            Debug.LogError("Regulator требует SteamVR_Interactable компонент!");
        }
        _initialValveRotation = ValveHandle.localRotation;
        _initialPointerRotation = Pointer.localRotation;

        block = false;
        IsValveOpen = false;
    }

    void Update()
    {
        // Если редуктор не присоединен и его держат, проверяем близость к баллону
        if (_attachedBallon == null && _interactable.isHovering)
        {
            TryAttachToNearbyBallon();
        }

        //Если редуктор присоединен, то обновляем стрелку на редукторе
        //if (_attachedBallon != null && IsValveOpen)
        //{
        //    UpdatePressureDisplay();
        //}
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

        if (IsValveOpen) // Если заблокировано, выходим
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

    public void OpenPressure()
    {
        float pressure = 0f;
        pressure = _attachedBallon.GasPressure;
        float angle = (MaxAngle - MinAngle) / MaxPressure * pressure;
        Quaternion targetRotation = _initialPointerRotation * Quaternion.Euler(angle, 0, 0);
        Pointer.localRotation = targetRotation;
        UpdateValveRotation(90f);
        Debug.Log("Угол: " + angle);
    }

    public void ClosePressure()
    {
        float pressure = 0f;
        float angle = (MaxAngle - MinAngle) / MaxPressure * pressure;
        Quaternion targetRotation = _initialPointerRotation * Quaternion.Euler(angle, 0, 0);
        Pointer.localRotation = targetRotation;
        UpdateValveRotation(0f);
    }

    //Метод для открытия/закрытия вентиля
    public void OpenValve()
    {
        if (_attachedBallon != null && !IsValveOpen)
        {
            IsValveOpen = true;
            Debug.Log("Valve is now " + (IsValveOpen ? "Open" : "Closed"));
            //_interactable.enabled = !IsValveOpen;
            OpenPressure();
        }
    }

    public void CloseValve()
    {
        if (_attachedBallon != null && IsValveOpen)
        {
            IsValveOpen = false;
            Debug.Log("Valve is now " + (IsValveOpen ? "Open" : "Closed"));
            //_interactable.enabled = !IsValveOpen;
            ClosePressure();
        }
    }

    //Метод для поворота вентиля
    private void UpdateValveRotation(float angleChange)
    {
        if (ValveHandle == null) return;
        Quaternion targetRotation = _initialValveRotation * Quaternion.Euler(angleChange, 0, 0);
        ValveHandle.localRotation = targetRotation;
        Debug.Log("Вентиль повёрнут на " + targetRotation);
    }
}
