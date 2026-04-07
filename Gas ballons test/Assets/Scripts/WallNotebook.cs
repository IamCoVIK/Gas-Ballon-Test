using TMPro;
using UnityEngine;
using Whisper;
using Whisper.Utils;

public class WallNotebook : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject endButton;
    [Space]
    [SerializeField] private Notebook ogNotebook;

    private void Start()
    {
        ogNotebook.onTextRecognized.AddListener(UpdateText);

        startButton.SetActive(true);
        endButton.SetActive(false);
    }

    public void StartRecord()
    {
        ogNotebook.blockUpdate = true;
        ogNotebook.StartRecording();

        startButton.SetActive(false);
        endButton.SetActive(true);
    }

    public void EndRecord() 
    {
        ogNotebook.StopRecording();

        startButton.SetActive(true);
        endButton.SetActive(false);

        ogNotebook.blockUpdate = false;
    }

    public void SendRecord()
    {
        ogNotebook.SendRecognizedText();
        text.text = "";
    }

    private void UpdateText()
    {
        text.text = ogNotebook.GetText();
    }
}
