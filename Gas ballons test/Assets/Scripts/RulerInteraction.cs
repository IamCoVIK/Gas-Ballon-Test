using UnityEngine;
using TMPro; // Импортируем пространство имен TextMeshPro
using Valve.VR; // Импортируем пространство имен SteamVR
using Valve.VR.InteractionSystem;

public class RulerInteraction : MonoBehaviour
{
    private Interactable interactable;

    [Header("Raycasting Settings")]
    public Transform rayOrigin; // Пустой объект, откуда будет исходить луч
    public float maxRayDistance = 100f; // Максимальная длина луча
    public LayerMask raycastLayerMask; // Слои, которые будет учитывать луч (для фильтрации)

    [Header("Visual Settings")]
    public LineRenderer lineRenderer; // Компонент LineRenderer для отображения луча
    public Color rayColor = Color.white; // Цвет луча
    public float rayWidth = 0.01f; // Ширина луча

    [Header("Input Settings")]
    //public SteamVR_Input_ActionSet actionSet; // Action Set, содержащий ваши действия
    public SteamVR_Action_Boolean fireButton; // Действие для выстрела (например, кнопка "Trigger" или "Grip")

    [Header("UI Settings")]
    public TMP_Text distanceText; // Ссылка на TextMeshProUGUI для отображения расстояния

    private bool isFiring = false; // Флаг, показывающий, зажата ли кнопка выстрела
    private bool isHeldByPlayer = false;

    void Start()
    {
        interactable = GetComponent<Interactable>();

        // Проверяем наличие всех необходимых компонентов
        if (rayOrigin == null)
        {
            Debug.LogError("Ray Origin transform is not assigned on " + gameObject.name);
            enabled = false; // Отключаем скрипт, если нет источника луча
            return;
        }

        if (lineRenderer == null)
        {
            Debug.LogError("Line Renderer is not assigned on " + gameObject.name);
            enabled = false;
            return;
        }

        if (distanceText == null)
        {
            Debug.LogError("Distance TextMeshProUGUI is not assigned on " + gameObject.name);
            enabled = false;
            return;
        }

        // Настраиваем LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;
        lineRenderer.enabled = false; // Луч изначально не виден

        // Инициализируем TextMeshPro
        distanceText.text = "0.0"; // Очищаем текст при старте
        //distanceText.gameObject.SetActive(false); // Скрываем текст при старте

        // Инициализация SteamVR ввода (если вы еще не настроили его глобально)
        // Если вы используете SteamVR Input System, то это может быть уже сделано.
        // Если нет, вам может понадобиться добавить эту настройку:
        // SteamVR_Input.Initialize();
    }

    void Update()
    {
        if (fireButton == null) return;

        // 1. Проверяем, удерживается ли предмет в руке

        // 2. Проверяем нажатие кнопки ВЫСТРЕЛА
        if (fireButton.GetState(SteamVR_Input_Sources.Any))
        {
            // Стреляем ТОЛЬКО если предмет в руке
            if (isHeldByPlayer)
            {
                StartFiring();
            }
            else if (isFiring)
            {
                // Если кнопка нажата, но предмет не в руке, сбрасываем состояние
                StopFiring();
            }
        }
        else if (fireButton.GetStateUp(SteamVR_Input_Sources.Any))
        {
            StopFiring();
        }

        // Обновляем луч, если мы в процессе "стрельбы"
        if (isFiring)
        {
            UpdateRaycast();
        }
    }

    void StartFiring()
    {
        isFiring = true;
        lineRenderer.enabled = true;
        //distanceText.gameObject.SetActive(true);
        UpdateRaycast(); // Выполняем первый ресткаст сразу при нажатии
    }

    void StopFiring()
    {
        isFiring = false;
        lineRenderer.enabled = false;
        //distanceText.text = "0.0"; // Скрываем текст при отпускании
        //distanceText.gameObject.SetActive(false);
    }

    void UpdateRaycast()
    {
        RaycastHit hit; // Структура для хранения информации о столкновении

        // Выполняем луч
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, maxRayDistance, raycastLayerMask))
        {
            // Луч попал в объект
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, hit.point); // Конечная точка луча - место попадания

            float distance = hit.distance; // Получаем расстояние до объекта
            distanceText.text = $"{distance:F2} m"; // Отображаем расстояние с двумя знаками после запятой

            // Позиционируем и поворачиваем текст, чтобы он был виден
            // Можно прикрепить TextMeshPro как дочерний объект к объекту, в который попал луч,
            // но это может быть сложно, если луч попадает в разные объекты.
            // Простой вариант - расположить текст около конечной точки луча.
            // Здесь мы просто ставим его рядом с конечной точкой.
            //distanceText.transform.position = hit.point + rayOrigin.forward * 0.1f; // Небольшой сдвиг вперед
            //distanceText.transform.rotation = Quaternion.LookRotation(rayOrigin.forward); // Ориентируем текст по направлению луча
        }
        else
        {
            // Луч ни во что не попал
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * maxRayDistance); // Луч упирается в максимальную дистанцию
            distanceText.text = ""; // Очищаем текст
        }
    }

    // Метод для получения текущего состояния выстрела (может быть полезен для других скриптов)
    public bool IsFiring()
    {
        return isFiring;
    }

    public void PickedUp()
    {
        isHeldByPlayer = true;
    }

    public void UnpickedUp()
    { isHeldByPlayer = false; }
}
