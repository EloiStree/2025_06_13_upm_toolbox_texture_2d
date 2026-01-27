using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
public class TextureMono_RelaySelectComputeShaderFromList : MonoBehaviour
{
    public List<ComputeShader> m_computeShaders;
    public UnityEvent<ComputeShader> m_onIndexChanged;
    public int m_index;


    public bool m_useLoop;
    public bool m_ignoreException;
    private void Awake()
    {
        TriggerChange();
    }




    [ContextMenu("Next")]
    public void Next()
    {
        if (m_computeShaders == null || m_computeShaders.Count == 0) return;
        if (m_useLoop)
        {
            m_index = (m_index + 1) % m_computeShaders.Count;
        }
        else
        {
            m_index = Mathf.Min(m_index + 1, m_computeShaders.Count - 1);
        }
        TriggerChange();
    }

    [ContextMenu("Previous")]
    public void Previous()
    {
        if (m_computeShaders == null || m_computeShaders.Count == 0) return;
        if (m_useLoop)
        {
            m_index = (m_index - 1 + m_computeShaders.Count) % m_computeShaders.Count;
        }
        else
        {
            m_index = Mathf.Max(m_index - 1, 0);
        }
        TriggerChange();
    }

    public void TriggerChange()
    {
        if (m_computeShaders == null || m_computeShaders.Count == 0) return;
        m_index = Mathf.Clamp(m_index, 0, m_computeShaders.Count - 1);

        if (m_ignoreException)
        {
            try
            {
                m_onIndexChanged?.Invoke(m_computeShaders[m_index]);
            }
            catch (Exception) { }
        }
        else {

            m_onIndexChanged?.Invoke(m_computeShaders[m_index]);
        }
    }
    /// <summary>
    /// Selects a compute shader by index and triggers the change event.
    /// </summary>
    /// <param name="index">The index of the compute shader to select.</param>
    public void Select(int index)
    {
        if (m_computeShaders == null || m_computeShaders.Count == 0) return;
        m_index = Mathf.Clamp(index, 0, m_computeShaders.Count - 1);
        TriggerChange();
    }
}


