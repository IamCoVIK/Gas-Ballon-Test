using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BallonValveInteraction : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;

    private Interactable interactable;
    private CircularDrive circularDrive;

    public Animator valveAnimator;
    public AudioSource valveSpinSound;
    public AudioSource valveLeakSound;

    public float explosionTime;
    private float explosionTimer;
    private float ExplosionTimer
    {
        get 
        {
            return explosionTimer;
        }
        set
        {
            explosionTimer = value;
            if (explosionTimer <= 0)
            {
                Explosion.Invoke();
            }
        }
    }

    public bool isOpen = false;
    private float maxVolume;
    private Ballon ballon;
    public AttachedReductor attachedReductor;

    public GameObject arrow;
    public float arrowMinAngle;
    public float arrowMaxAngle;

    public UnityEvent Explosion;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        circularDrive = GetComponent<CircularDrive>();
        ExplosionTimer = explosionTime;
        ballon = GetComponentInParent<Ballon>();

        maxVolume = valveLeakSound.volume;

        //arrowMinAngle = arrow.transform.rotation.eulerAngles.x;
    }

    /*private void HandHoverUpdate(Hand hand)
    {
        if (grabAction.GetStateDown(hand.handType))
        {
            ToggleValve();
        }
    }*/

    private void Update()
    {
        /*if (isOpen)
        {
            if (!ballon.withReductor)
            {
                ExplosionTimer -= Time.deltaTime;
            }
        }*/
        if (circularDrive.outAngle == circularDrive.startAngle)
        {
            isOpen = false;
            valveLeakSound.volume = 0;
            valveLeakSound.Stop();
            ExplosionTimer = explosionTime;

            if (ballon.withReductor)
            {
                arrow.transform.rotation = Quaternion.Euler(new Vector3(arrowMinAngle, 0, 0));
            }
        }
        else
        {
            isOpen = true;
            float a = Mathf.Abs(circularDrive.outAngle) / Mathf.Abs(circularDrive.maxAngle - circularDrive.minAngle);
            if (!ballon.withReductor)
            {
                if (!valveLeakSound.isPlaying)
                {
                    valveLeakSound.Play();
                }
                valveLeakSound.volume = maxVolume * a;
                ExplosionTimer -= Time.deltaTime * a;
            }
            else
            {
                arrow.transform.rotation = Quaternion.Euler(new Vector3((arrowMaxAngle - arrowMinAngle) * a + arrowMinAngle, 0, 0));
                Debug.Log(arrow.transform.rotation.eulerAngles.x);
            }
        }
    }

    void ToggleValve()
    {
        if (isOpen)
        {
            AnimateValve("Valve_Close");
            valveLeakSound.Stop();
            ExplosionTimer = explosionTime;
            if (ballon.withReductor)
            {
                attachedReductor.ArrowDown();
            }
        }
        else
        {
            AnimateValve("Valve_Open");
            if (!ballon.withReductor)
            {
                valveLeakSound.Play();
            }
            else
            {
                attachedReductor.ArrowUp(ballon.GasPressure);
            }
        }

        isOpen = !isOpen;
        valveSpinSound.Play();
    }

    void AnimateValve(string anim)
    {
        valveAnimator.SetTrigger(anim);
    }

    public void PlayLeakSound()
    {
        valveLeakSound.Play();
    }
}
