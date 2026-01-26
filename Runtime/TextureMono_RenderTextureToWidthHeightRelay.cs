using UnityEngine;
using UnityEngine.Events;

public class TextureMono_RenderTextureToWidthHeightRelay : MonoBehaviour
{
    public UnityEvent<int, int> m_onValueWidthHeightChanged;
    public int m_width;
    public int m_height;
    public int m_count;
    public void PushIn(RenderTexture texture) {

        int w = texture.width;
        int h = texture.height;
        if (w != m_width || h != m_height){ 
            m_width = w;
            m_height = h;
            m_count = w * h;
            m_onValueWidthHeightChanged.Invoke(w, h);
        }
    }
}

