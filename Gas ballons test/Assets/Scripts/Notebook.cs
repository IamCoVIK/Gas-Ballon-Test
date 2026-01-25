using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Whisper;
using Whisper.Utils;

public class Notebook : MonoBehaviour
{
    [SerializeField] private AudioSource hitSound;
    [Space]
    [SerializeField] private TMP_Text text;
    [SerializeField] private SteamVR_Action_Boolean recordButton;
    [Space]
    [SerializeField] private WhisperManager whisper;
    [SerializeField] private MicrophoneRecord microphoneRecord;
    [Space]
    [SerializeField] private Rigidbody rb;
    [Space]
    [SerializeField] private TestingSystem tsystem;
    private Interactable interactable;
    
    private string recognizedText = string.Empty;
    private string _buffer;

    private void Awake()
    {
        whisper.OnNewSegment += OnNewSegment;
        whisper.OnProgress += OnProgressHandler;

        microphoneRecord.OnRecordStop += OnRecordStop;
    }

    private void Start()
    {
        interactable = GetComponent<Interactable>();
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
            if (microphoneRecord.IsRecording)
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
        if (!microphoneRecord.IsRecording)
        {
            microphoneRecord.StartRecord();
            Debug.Log("Запись голоса начата.");
        }
    }

    private void StopRecording() 
    {
        if (microphoneRecord.IsRecording)
        {
            microphoneRecord.StopRecord();
            Debug.Log("Запись голоса закончена.");
        }
    }

    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        _buffer = "";

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        WhisperResult res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null)
            return;

        long time = sw.ElapsedMilliseconds;
        var rate = recordedAudio.Length / (time * 0.001f);
        Debug.Log($"Time: {time} ms\nRate: {rate:F1}x");

        string text = res.Result;

        recognizedText = text;
        Debug.Log(recognizedText);
        UpdateText();
    }

    private void OnProgressHandler(int progress)
    {
        Debug.Log($"Progress: {progress}%");
    }

    private void OnNewSegment(WhisperSegment segment)
    {
        _buffer += segment.Text;
        Debug.Log(_buffer + "...");
    }

    private void UpdateText() 
    {
        text.text = recognizedText;
    }

    public void SendRecognizedText()
    {
        Debug.Log("Текст отправлен на проверку.");
        tsystem.AddNewInput(recognizedText);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (rb.velocity.magnitude > 1f)
        {
            hitSound.Play();
        }
    }
}
