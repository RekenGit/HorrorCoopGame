using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneDetection : MonoBehaviour
{
    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    private bool isNoiseMade = false;
    public GameObject panel;
    public GameObject barScream;
    public GameObject barTalk;
    public GameObject barWhisper;
    private const float screamVolume = 22f;
    private const float talkVolume = 7f;
    private const float whisperVolume = 1f;
    private const float weirdError = 42.78979f;

    void Start()
    {
        MicAudioToClip();   
        foreach (var mic in Microphone.devices) Debug.Log("name: " + mic);
        barWhisper.transform.position = new Vector3(barWhisper.transform.position.x, 70f + (whisperVolume * 10) - weirdError, barWhisper.transform.position.z);
        barTalk.transform.position = new Vector3(barTalk.transform.position.x, 70f + (talkVolume * 10) - weirdError, barTalk.transform.position.z);
        barScream.transform.position = new Vector3(barScream.transform.position.x, 70f + (screamVolume * 10) - weirdError, barScream.transform.position.z);
    }

    void Update()
    {
        if (GameManager.players.Count == 0) return;
        float loudness = GetLoudnessFromMic() * 100;
        if (loudness > talkVolume && !isNoiseMade)
        {
            StartCoroutine(NoiseMade(loudness));
        }
        panel.transform.localScale = new Vector3(0.02f, loudness, 1);
    }
    private IEnumerator NoiseMade(float loudness)
    {
        isNoiseMade = true;
        yield return new WaitForSeconds(2);
        GameObject player = GameObject.Find("LocalPlayer(Clone)");
        GameObject enemy = GameObject.Find("Test-Enemy");
        float distance = loudness > screamVolume ? 100f : 35f;
        float playerEnemyDistance = Vector3.Distance(player.transform.localPosition, enemy.transform.localPosition);
        Debug.Log(string.Format("{0} : {1}", playerEnemyDistance, distance));
        if (playerEnemyDistance < distance) ClientSend.PlayerMadeNoise();
        isNoiseMade = false;
    }
    public void MicAudioToClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }
    public float GetLoudnessFromMic()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip) * 2;
    }
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        if (startPosition < 0) return 0;
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++) totalLoudness += Mathf.Abs(waveData[i]);
        return totalLoudness / sampleWindow;
    }
}
