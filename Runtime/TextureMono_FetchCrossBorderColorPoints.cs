using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TextureMono_FetchCrossBorderColorPoints : MonoBehaviour { 

    public int m_widthOfTexture = 0;
    public int m_heightOfTexture = 0;
    public int m_expectedLenght = 0;
    public int m_nativeArrayLenght = 0;
    public NativeArray<Color32> m_colors;

    public Color32 m_centerColor;
    public Color32 m_topLeftColor;
    public Color32 m_topRightColor;
    public Color32 m_bottomLeftColor;
    public Color32 m_bottomRightColor;
    public Color32 m_topColor;
    public Color32 m_rightColor;
    public Color32 m_downColor;
    public Color32 m_leftColor;

    public UnityEvent<Color32> m_onCenterUpdated;

    public void ProcessNativeArrayToColors()
    {
        // 1D NativeArray coming from a RenderTexture (row-major)
        m_nativeArrayLenght = m_colors.Length;

        if (m_widthOfTexture <= 0 || m_heightOfTexture <= 0)
            return;

        if (m_nativeArrayLenght != m_widthOfTexture * m_heightOfTexture)
            return;

        int w = m_widthOfTexture;
        int h = m_heightOfTexture;

        int centerX = w / 2;
        int centerY = h / 2;

        int Center(int x, int y) => y * w + x;

        // Center
        m_centerColor = m_colors[Center(centerX, centerY)];

        // Corners
        m_bottomLeftColor = m_colors[Center(0, 0)];
        m_bottomRightColor = m_colors[Center(w - 1, 0)];
        m_topLeftColor = m_colors[Center(0, h - 1)];
        m_topRightColor = m_colors[Center(w - 1, h - 1)];

        // Edges (midpoints)
        m_topColor = m_colors[Center(centerX, h - 1)];
        m_downColor = m_colors[Center(centerX, 0)];
        m_leftColor = m_colors[Center(0, centerY)];
        m_rightColor = m_colors[Center(w - 1, centerY)];

        m_onCenterUpdated.Invoke(m_centerColor);
    }


    public void SetTextureAsNativeColor(NativeArray<Color32> colors) { 
    
        m_colors = colors;
        m_expectedLenght = colors.Length;
        ProcessNativeArrayToColors();   

    }
    public void SetTextureDimension(int width, int height)
    {
        m_widthOfTexture = width;
        m_heightOfTexture = height;
        m_expectedLenght = width*height;
    }
    public void SetTextureDimensionFromSource(Texture source)
    {
        m_widthOfTexture = source.width;
        m_heightOfTexture = source.height;
        m_expectedLenght = source.width * source.height;
    }
}

