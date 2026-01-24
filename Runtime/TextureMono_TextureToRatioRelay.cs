using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_TextureToRatioRelay : MonoBehaviour
    {
        public UnityEvent<float> m_onRatioHorizontalToVertical;
        public void PushInTexture(Texture texture)
        {
            if (texture != null && texture.height != 0)
            {
                float ratio = (float)texture.width / (float)texture.height;
                m_onRatioHorizontalToVertical.Invoke(ratio);
            }
        }
        
    }

}