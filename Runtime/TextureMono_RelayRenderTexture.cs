using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility 
{
    public class TextureMono_RelayRenderTexture : MonoBehaviour {
        public RenderTexture m_textureRelayed;
        public UnityEvent<RenderTexture> m_onRenderTextureRelay;
        public void PushIn(RenderTexture texture) { 
        
            m_textureRelayed = texture;
            m_onRenderTextureRelay?.Invoke(texture);
        }
        [ContextMenu("Push in inspector texture")]
        public void PushTextureInInspector() { 
        
            PushIn(m_textureRelayed);
        }
    }
}
