using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class TextureMono_RenderTextureToNativeColor32 : MonoBehaviour
{
    public RenderTexture sourceRenderTexture;
    public NativeArray2DColor32WH m_pixelsRecovered;
    public UnityEvent<NativeArray2DColor32WH> m_sizeChanged;
    public UnityEvent<NativeArray2DColor32WH> m_onUpdatedColor;

    public bool m_useUpdate = true;

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        if (renderTexture == null || renderTexture.width <= 0 || renderTexture.height <= 0)
            return;

        bool sizeChanged = m_pixelsRecovered.m_width != renderTexture.width ||
                           m_pixelsRecovered.m_height != renderTexture.height;

        sourceRenderTexture = renderTexture;
        m_pixelsRecovered.ReCreateIfSizeChanged(renderTexture.width, renderTexture.height);

        if (sizeChanged)
        {
            m_sizeChanged?.Invoke(m_pixelsRecovered);
        }
    }

    private void Update()
    {
        if (m_useUpdate)
        {
            Refresh();
        }
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        if (sourceRenderTexture == null)
            return;

        AsyncGPUReadback.Request(sourceRenderTexture, 0, TextureFormat.RGBA32, OnCompleteReadback);
    }

    private void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError || !request.done)
        {
            Debug.LogError("GPU Readback error or not done.");
            return;
        }

        NativeArray<Color32> cTemp = request.GetData<Color32>();

        // Copy the data to our custom structure
        m_pixelsRecovered.CopyNativeArray(cTemp, m_pixelsRecovered.m_width, m_pixelsRecovered.m_height);

        m_onUpdatedColor?.Invoke(m_pixelsRecovered);
    }

    private void OnDestroy()
    {
        if (m_pixelsRecovered.m_nativeArray.IsCreated)
        {
            m_pixelsRecovered.m_nativeArray.Dispose();
        }
    }
}

