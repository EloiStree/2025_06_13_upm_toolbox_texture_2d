using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WebcamToolMono_MicrophoneList : MonoBehaviour
{
    public List<string> m_microphoneNames = new List<string>();
    public bool m_useDebugLogForMicrophones = true;

    public UnityEvent<string[]> m_onMicrophoneListRefreshed;
    void Awake()
    {
        RefreshMicrophoneList();
    }
    [ContextMenu("Refresh Microphone List")]
    public void RefreshMicrophoneList()
    {
        m_microphoneNames.Clear();
        foreach (var device in Microphone.devices)
        {
            m_microphoneNames.Add(device);
            if (m_useDebugLogForMicrophones)
                Debug.Log("Microphone detected: " + device);
        }
        m_onMicrophoneListRefreshed?.Invoke(m_microphoneNames.ToArray());
    }
}
