using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

public class TestVoice : MonoBehaviour
{
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public bool streamSegments = true;
    public bool printLanguage = true;

    private string _buffer;

    private void Awake()
    {
        whisper.OnNewSegment += OnNewSegment;
        whisper.OnProgress += OnProgressHandler;

        microphoneRecord.OnRecordStop += OnRecordStop;
    }

    private void Start()
    {
        var sb = new StringBuilder();

        // check if Multilingual
        var multi = whisper.IsMultilingual();
        var msg = "Current model Multilingual: " + multi;
        sb.AppendLine(msg);

        if (multi)
        {
            sb.AppendLine();
            sb.AppendLine("All languages names:");

            // write all languages in one string
            var languages = WhisperLanguage.GetAllLanguages();
            for (var i = 0; i < languages.Length - 1; i++)
                sb.Append(languages[i] + ", ");
            sb.Append(languages[languages.Length - 1] + ".");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonPressed();
        }
    }

    private void OnVadChanged(bool vadStop)
    {
        microphoneRecord.vadStop = vadStop;
    }

    private void OnButtonPressed()
    {
        if (!microphoneRecord.IsRecording)
        {
            microphoneRecord.StartRecord();
            Debug.Log("Stop");
        }
        else
        {
            microphoneRecord.StopRecord();
            Debug.Log("Record");
        }
    }

    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        Debug.Log("Record");
        _buffer = "";

        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null)
            return;

        var time = sw.ElapsedMilliseconds;
        var rate = recordedAudio.Length / (time * 0.001f);
        Debug.Log($"Time: {time} ms\nRate: {rate:F1}x");

        var text = res.Result;
        if (printLanguage)
            text += $"\n\nLanguage: {res.Language}";

        Debug.Log(text);
        //UiUtils.ScrollDown(scroll);
    }

    private void OnLanguageChanged(int ind)
    {
        //var opt = languageDropdown.options[ind];
        //whisper.language = opt.text;
    }

    private void OnTranslateChanged(bool translate)
    {
        whisper.translateToEnglish = translate;
    }

    private void OnProgressHandler(int progress)
    {
        Debug.Log($"Progress: {progress}%");
    }

    private void OnNewSegment(WhisperSegment segment)
    {
        if (!streamSegments)
            return;

        _buffer += segment.Text;
        Debug.Log(_buffer + "...");
    }
}
