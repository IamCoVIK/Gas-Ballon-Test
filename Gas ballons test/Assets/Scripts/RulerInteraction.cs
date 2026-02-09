using UnityEngine;
using TMPro;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RulerInteraction : MonoBehaviour
{
    private Interactable interactable;

    [Header("Raycasting Settings")]
    public Transform rayOrigin;
    public float maxRayDistance = 100f;
    public LayerMask raycastLayerMask;

    [Header("Visual Settings")]
    public LineRenderer lineRenderer;
    public Color rayColor = Color.white;
    public float rayWidth = 0.01f;

    [Header("Input Settings")]
    public SteamVR_Action_Boolean fireButton;

    [Header("UI Settings")]
    public TMP_Text distanceText;
    public AudioSource hitSound;

    private bool isFiring = false;
    private Hand holdingHand;
    private Rigidbody rb;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        rb = GetComponent<Rigidbody>();

        if (rayOrigin == null)
        {
            Debug.LogError("Ray Origin transform is not assigned on " + gameObject.name);
            enabled = false;
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

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
        lineRenderer.startColor = rayColor;
        lineRenderer.endColor = rayColor;
        lineRenderer.enabled = false;

        distanceText.text = "0.0 m";
    }

    void Update()
    {
        if (fireButton == null) return;

        // Получаем источник ввода от руки, которая держит предмет
        SteamVR_Input_Sources inputSource = GetHoldingHandInputSource();

        // Если предмет не в руке, сбрасываем состояние и выходим
        if (inputSource == SteamVR_Input_Sources.Any)
        {
            if (isFiring)
            {
                StopFiring();
            }
            return;
        }

        // Проверяем нажатие кнопки только на той руке, которая держит предмет
        if (fireButton.GetStateDown(inputSource))
        {
            StartFiring();
        }
        else if (fireButton.GetStateUp(inputSource))
        {
            StopFiring();
        }

        if (isFiring)
        {
            UpdateRaycast();
        }
    }

    // Метод для получения источника ввода от руки, которая держит предмет
    private SteamVR_Input_Sources GetHoldingHandInputSource()
    {
        PickedUp();
        if (holdingHand != null)
        {
            return holdingHand.handType;
        }
        return SteamVR_Input_Sources.Any; // Возвращаем Any, если предмет не в руке
    }

    void StartFiring()
    {
        isFiring = true;
        lineRenderer.enabled = true;
        UpdateRaycast();
    }

    void StopFiring()
    {
        isFiring = false;
        lineRenderer.enabled = false;
    }

    void UpdateRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, maxRayDistance, raycastLayerMask))
        {
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, hit.point);

            float distance = hit.distance;
            distanceText.text = $"{distance:F2} m";
        }
        else
        {
            lineRenderer.SetPosition(0, rayOrigin.position);
            lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * maxRayDistance);
            distanceText.text = "";
        }
    }

    public bool IsFiring()
    {
        return isFiring;
    }

    public void PickedUp()
    {
        holdingHand = interactable.attachedToHand;
    }

    public void UnpickedUp()
    {
        holdingHand = null;

        if (isFiring)
        {
            StopFiring();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1f)
        {
            hitSound.Play();
        }
    }
}