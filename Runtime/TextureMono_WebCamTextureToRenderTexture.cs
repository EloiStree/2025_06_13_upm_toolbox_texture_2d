using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility {
    }
    public class TextureMono_WebCamTextureToRenderTexture : MonoBehaviour {

        public WebCamTexture m_selected;
        public RenderTexture m_result;
        public UnityEvent<RenderTexture> m_onRenderTextureCreated;
        public UnityEvent<RenderTexture> m_onRenderTextureBlit;
        public bool m_useUpdateBlit = true;
        public void PushInWebcamTexture(WebCamTexture texture) {

            m_selected = texture;
            if (m_selected != null)
            {
                CheckForChange(texture);
            }
        }
        public void Update()
        {
            if (!m_useUpdateBlit)
                return;
            BlitTexture();
        }
        private void CheckForChange(Texture texture)
        {
            if (m_result == null || texture.width != m_result.width || texture.height != m_result.height)
            {
                if (m_result != null)
                    m_result.Release();
                m_result = new RenderTexture(texture.width, texture.height, 0);
                m_result.enableRandomWrite = true;
                m_result.filterMode = FilterMode.Point;
                m_result.wrapMode = TextureWrapMode.Clamp;

                m_result.Create();
                m_onRenderTextureCreated.Invoke(m_result);
            }
        }
        private void BlitTexture()
        {
            if ( m_selected && m_result)
            {
                CheckForChange(m_result);
                Graphics.Blit(m_selected, m_result);
                m_onRenderTextureBlit.Invoke(m_result);
            }
        }
    }

