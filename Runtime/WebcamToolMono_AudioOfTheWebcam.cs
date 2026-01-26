using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WebcamToolMono_AudioOfTheWebcam : MonoBehaviour
{
    [Header("Audio")]
    public List<string> m_priorityMicrophones = new List<string>();
    public string m_microphoneName;
    public int m_sampleRate = 44100;

    private AudioSource m_audioSource;

    void Awake()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
        m_audioSource.loop = true;
        m_audioSource.playOnAwake = false;
        m_audioSource.spatialBlend = 0f;
    }

    void Start()
    {
        SelectMicrophone();
        StartMicrophone();
    }

    void SelectMicrophone()
    {
        var devices = Microphone.devices;
        if (devices.Length == 0)
        {
            Debug.LogWarning("No microphone detected.");
            return;
        }

        foreach (var priority in m_priorityMicrophones)
        {
            foreach (var device in devices)
            {
                if (device == priority)
                {
                    m_microphoneName = device;
                    return;
                }
            }
        }

        m_microphoneName = devices[0];
    }

    void StartMicrophone()
    {
        if (string.IsNullOrEmpty(m_microphoneName))
            return;

        m_audioSource.clip = Microphone.Start(m_microphoneName, true, 1, m_sampleRate);

        while (Microphone.GetPosition(m_microphoneName) <= 0) { }

        m_audioSource.Play();
    }

    void OnDisable()
    {
        if (!string.IsNullOrEmpty(m_microphoneName))
            Microphone.End(m_microphoneName);
    }
}
