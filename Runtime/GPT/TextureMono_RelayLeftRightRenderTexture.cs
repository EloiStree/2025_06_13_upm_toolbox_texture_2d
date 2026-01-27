using UnityEngine;
using UnityEngine.Events;

public class TextureMono_RelayLeftRightRenderTexture : MonoBehaviour { 

    public RenderTexture m_leftRenderTexture;
    public RenderTexture m_rightRenderTexture;
    public UnityEvent<RenderTexture> m_onLeftRenderTextureUpdated;
    public UnityEvent<RenderTexture> m_onRightRenderTextureUpdated;
    public UnityEvent<RenderTexture, RenderTexture> m_onLeftRightRenderTextureUpdated;


    public void SetBothWithOnTexture(RenderTexture rt)
    {
        m_leftRenderTexture = rt;
        m_rightRenderTexture = rt;
        Relay();
    }

    public void SetLeftTexture(RenderTexture rt)
    {
        m_leftRenderTexture = rt;
        Relay();
    }
    public void SetRightTexture(RenderTexture rt)
    {
        m_rightRenderTexture = rt;
        Relay();
    }
    public void SetLeftRightTexture(RenderTexture left, RenderTexture right)
    {
        m_leftRenderTexture = left;
        m_rightRenderTexture = right;
        Relay();
    }

    public void Relay()
    {
        m_onLeftRightRenderTextureUpdated?.Invoke(m_leftRenderTexture, m_rightRenderTexture);
        m_onLeftRenderTextureUpdated?.Invoke(m_leftRenderTexture);
        m_onRightRenderTextureUpdated?.Invoke(m_rightRenderTexture);
    }
}


