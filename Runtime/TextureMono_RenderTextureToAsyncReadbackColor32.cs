using Eloi.TextureUtility;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class TextureMono_RenderTextureToAsyncReadbackColor32 : MonoBehaviour
{
    [SerializeField] private RenderTexture m_givenTexture;

    public UnityEvent<RenderTexture> m_onNewRenderTexture;
    public NativeColorEvent m_onReadbackNativeColor32Ready = new NativeColorEvent();

    private NativeArray<Color32> _buffer;
    public NativeArray<Color32> Buffer => _buffer;

    public Texture_WatchAndDateTimeObserver m_timeUsedAfterReadBack;



    [Header("Debug")]
    public long m_readBackCount = 0;
    public int m_readBackLenght;
    public int m_textureRenderingWidth;
    public int m_textureRenderingHeight;

    public void SetRenderTexture(RenderTexture texture) {


        if (texture == null) {

            bool changedValue = m_givenTexture != null;
            m_givenTexture = null;
            if(changedValue)    
                m_onNewRenderTexture.Invoke(null);
            return;
        }

        bool changed = m_givenTexture != texture || (texture.width != m_givenTexture.width || texture.height != m_givenTexture.height);
        m_givenTexture = texture;
        if (changed)
            m_onNewRenderTexture.Invoke(m_givenTexture);
    }
    public void SetAndRequestReadBackOfTexture(RenderTexture texture)
    {
        SetRenderTexture(texture);
        RequestReadback();
    }

    public bool m_useUpdate = false;
    private void Update()
    {
        if (m_useUpdate)
        {
            RequestReadback();
        }
    }

    public void RequestReadback()
    {
        if (m_givenTexture == null)
        {
            return;
        }

        if (!m_givenTexture.IsCreated())
        {
            return;
        }

        AsyncGPUReadback.Request(
            m_givenTexture,
            0,
            TextureFormat.RGBA32,
            OnReadbackCompleted
        );
    }

    private void OnReadbackCompleted(AsyncGPUReadbackRequest request)
    {
        m_timeUsedAfterReadBack.StartCounting();
        if (request.hasError)
        {
            Debug.LogError("AsyncGPUReadback failed.");
            return;
        }

        var gpuData = request.GetData<Color32>();
        EnsureCapacity(gpuData.Length);

        _buffer.CopyFrom(gpuData);

        m_readBackCount++;
        m_readBackLenght = _buffer.Length;
        m_textureRenderingWidth = m_givenTexture.width;
        m_textureRenderingHeight = m_givenTexture.height;

        m_timeUsedAfterReadBack.StopCounting();
        m_onReadbackNativeColor32Ready.Invoke(_buffer);

    }

    private void EnsureCapacity(int requiredLength)
    {
        if (_buffer.IsCreated && _buffer.Length == requiredLength)
            return;

        if (_buffer.IsCreated)
            _buffer.Dispose();

        _buffer = new NativeArray<Color32>(
            requiredLength,
            Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory
        );
    }

    private void OnDestroy()
    {
        if (_buffer.IsCreated)
            _buffer.Dispose();
    }

    [System.Serializable]
    public class NativeColorEvent : UnityEvent<NativeArray<Color32>> { }
}

