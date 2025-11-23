using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Whisper;

public class Notebook : MonoBehaviour
{
    [SerializeField] private AudioSource hitSound;
    [Space]
    [SerializeField] private TMP_Text text;
    [SerializeField] private SteamVR_Action_Boolean recordButton;
    public bool isRecording = false;
    [SerializeField] private Rigidbody rb;
    private Interactable interactable;
    private WhisperManager whisper;
    private string recognizedText = string.Empty;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    private void DictationInProcess(string text)
    {
        recognizedText = text;
        UpdateText();
        Debug.Log("Распознано - " + recognizedText);
    }

    private void DictationEnd(DictationCompletionCause cause)
    {
        UpdateText();
        Debug.Log("Финальный результат stt - " + recognizedText);
    }

    void Update()
    {
        if (recordButton == null) return;

        Hand holdingHand = interactable.attachedToHand;

        SteamVR_Input_Sources inputSource;

        if (holdingHand != null)
        {
            inputSource = holdingHand.handType;
        }
        else
            inputSource = SteamVR_Input_Sources.Any;

        if (inputSource == SteamVR_Input_Sources.Any)
        {
            if (isRecording)
            {
                StopRecording();
            }
            return;
        }

        if (recordButton.GetStateDown(inputSource))
        {
            StartRecording();
        }
        else if (recordButton.GetStateUp(inputSource))
        {
            StopRecording();
        }
    }

    private void StartRecording() 
    {
        if (!isRecording)
        {
            //dictationRecognizer.Start();
            isRecording = true;
            Debug.Log("Распознавание голоса начато.");
        }
    }

    private void StopRecording() 
    {
        if (isRecording)
        {
            //dictationRecognizer.Stop();
            isRecording = false;
            Debug.Log("Распознавание голоса закончено.");
        }
    }

    private void UpdateText() 
    {
        text.text = recognizedText;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1f)
        {
            hitSound.Play();
        }
    }
}
