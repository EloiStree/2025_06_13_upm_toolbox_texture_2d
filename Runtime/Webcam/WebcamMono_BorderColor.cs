

using UnityEngine.Events;
using UnityEngine;

public class WebcamMono_BorderColor : MonoBehaviour
{

    public WebCamTexture m_source;
    public Texture2D m_texture2D;
    public Color m_colorAverage;


    public UnityEvent<Color> m_onColorUpdated;
    public UnityEvent<Texture2D> m_onTextureUpdated;
    public void SetWebcamTextureToUse(WebCamTexture webcamTexture)
    {
        m_source = webcamTexture;
    }
    public bool m_mipmap = false;
    public bool m_linear = false;

    public void Awake()
    {
        m_texture2D = new Texture2D(3, 3, TextureFormat.RGBA32, m_mipmap, m_linear);
        m_texture2D.wrapMode = TextureWrapMode.Clamp;
        m_texture2D.filterMode = FilterMode.Point;

    }


    void Update()
    {
        if (m_source == null || m_texture2D == null)
        {
            return;
        }
        if (m_source.didUpdateThisFrame)
        {
            int width = m_source.width;
            int height = m_source.height;

            if (width > 0 && height > 0)
            {
                // Read key pixels
                Color topLeft = m_source.GetPixel(0, height - 1);
                Color topRight = m_source.GetPixel(width - 1, height - 1);
                Color bottomLeft = m_source.GetPixel(0, 0);
                Color bottomRight = m_source.GetPixel(width - 1, 0);
                Color center = m_source.GetPixel(width / 2, height / 2);

                Color left = m_source.GetPixel(0, height / 2);
                Color right = m_source.GetPixel(width - 1, height / 2);
                Color top = m_source.GetPixel(width / 2, height - 1);
                Color bottom = m_source.GetPixel(width / 2, 0);

                m_texture2D.SetPixel(0, 0, bottomLeft);
                m_texture2D.SetPixel(1, 0, bottom);
                m_texture2D.SetPixel(2, 0, bottomRight);
                m_texture2D.SetPixel(0, 1, left);
                m_texture2D.SetPixel(1, 1, center);
                m_texture2D.SetPixel(2, 1, right);
                m_texture2D.SetPixel(0, 2, topLeft);
                m_texture2D.SetPixel(1, 2, top);
                m_texture2D.SetPixel(2, 2, topRight);
                m_texture2D.Apply();
                m_colorAverage = (topLeft + topRight + bottomLeft + bottomRight + center) / 5f;

                m_onColorUpdated?.Invoke(m_colorAverage);
                m_onTextureUpdated?.Invoke(m_texture2D);

            }
        }
    }
}
