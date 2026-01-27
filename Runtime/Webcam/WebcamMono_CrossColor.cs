

using UnityEngine.Events;
using UnityEngine;

public class WebcamMono_CrossColor : MonoBehaviour
{
    public WebCamTexture m_source;
    public Texture2D m_texture2D;
    public Color m_colorAverage;

    public int m_crossRadius = 5;
    public UnityEvent<Color> m_onColorUpdated;
    public UnityEvent<Texture2D> m_onTextureUpdated;
    public bool m_mipmap = false;
    public bool m_linear = false;


    public void Awake()
    {
        m_texture2D = new Texture2D(m_crossRadius * 2 + 1, m_crossRadius * 2 + 1, TextureFormat.RGBA32, m_mipmap, m_linear);
        m_texture2D.wrapMode = TextureWrapMode.Clamp;
        m_texture2D.filterMode = FilterMode.Point;

        for
            (int x = 0; x < m_texture2D.width; x++)
        {
            for (int y = 0; y < m_texture2D.height; y++)
            {
                m_texture2D.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
    }

    private Color m_blackTransparent = new Color(0, 0, 0, 0);
    public void Update()
    {
        if (m_source != null && m_source.isPlaying)
        {
            Color averageTotale = Color.clear;
            int colorCount = m_crossRadius + m_crossRadius;
            int width = m_source.width;
            int height = m_source.height;
            int centerX = width / 2;
            int centerY = height / 2;


            for (int x = 0 - m_crossRadius; x <= 0 + m_crossRadius; x++)
            {
                if (centerX + x < 0 || centerX + x >= width || centerY < 0 || centerY >= height)
                {
                    m_texture2D.SetPixel(x + m_crossRadius, m_crossRadius, m_blackTransparent);
                    continue;
                }
                Color pixelColor = m_source.GetPixel(centerX + x, centerY);
                averageTotale += pixelColor;
                m_texture2D.SetPixel(x + m_crossRadius, m_crossRadius, pixelColor);
            }
            for (int y = 0 - m_crossRadius; y <= 0 + m_crossRadius; y++)

            {
                if (centerX < 0 || centerX >= width || centerY + y < 0 || centerY + y >= height)
                {
                    m_texture2D.SetPixel(m_crossRadius, y + m_crossRadius, m_blackTransparent);
                    continue;
                }
                Color pixelColor = m_source.GetPixel(centerX, centerY + y);
                averageTotale += pixelColor;
                m_texture2D.SetPixel(m_crossRadius, y + m_crossRadius, pixelColor);
            }


            m_colorAverage = averageTotale / colorCount;
            m_onColorUpdated?.Invoke(m_colorAverage);
            m_onTextureUpdated?.Invoke(m_texture2D);
            m_texture2D.Apply();

        }
    }

    public void SetWebcamTextureToUse(WebCamTexture webcamTexture)
    {
        m_source = webcamTexture;

    }

}