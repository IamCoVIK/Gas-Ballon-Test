using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class FireExt : MonoBehaviour
{
    private Interactable interactable;

    public SteamVR_Action_Boolean fireButton;

    public ParticleSystem particles;

    public Animator animator;

    public AudioSource sound;
    public AudioSource hitSound;

    private Rigidbody rb;

    private bool isFiring = false;
    private bool isHeldByPlayer = false;
    private Hand holdingHand; // Добавляем ссылку на руку, которая держит предмет

    void Start()
    {
        interactable = GetComponent<Interactable>();
        rb = GetComponent<Rigidbody>();
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
        if (fireButton.GetState(inputSource))
        {
            StartFiring();
        }
        else if (fireButton.GetStateUp(inputSource))
        {
            StopFiring();
        }
    }

    // Метод для получения источника ввода от руки, которая держит предмет
    private SteamVR_Input_Sources GetHoldingHandInputSource()
    {
        if (holdingHand != null)
        {
            return holdingHand.handType;
        }
        return SteamVR_Input_Sources.Any; // Возвращаем Any, если предмет не в руке
    }

    void StartFiring()
    {
        isFiring = true;
        animator.SetBool("Fire", true);
        if (!particles.isPlaying)
        {
            particles.Play();
        }
        if (!sound.isPlaying)
        {
            sound.Play();
        }
    }

    void StopFiring()
    {
        isFiring = false;
        animator.SetBool("Fire", false);
        particles.Stop();
        sound.Stop();
    }

    public bool IsFiring()
    {
        return isFiring;
    }

    public void PickedUp()
    {
        isHeldByPlayer = true;
        // Получаем ссылку на руку, которая подняла предмет
        holdingHand = interactable.attachedToHand;
    }

    public void UnpickedUp()
    {
        isHeldByPlayer = false;
        holdingHand = null; // Сбрасываем ссылку на руку

        // Останавливаем измерение при отпускании предмета
        if (isFiring)
        {
            StopFiring();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 0.75f)
        {
            hitSound.Play();
        }
    }
}
