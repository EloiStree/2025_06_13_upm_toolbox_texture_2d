using Eloi.TextureUtility;
using UnityEngine;
using UnityEngine.Events;



/// <summary>
/// NOT TESTED YET
/// </summary>
public class TextureMono_GraphBlitResizeRenderTexture : MonoBehaviour
{
    [Header("Input / Output")]
    [SerializeField] private RenderTexture m_source;
    [SerializeField] private RenderTexture m_result;

    [Header("Target Size")]
    [SerializeField] private int m_targetWidth = 1280;
    [SerializeField] private int m_targetHeight = 930;

    [Header("Behaviour")]
    [SerializeField] private bool m_blitEveryFrame;

    public UnityEvent<RenderTexture> m_onCreated;
    public UnityEvent<RenderTexture> m_onUpdated;

    private RenderTextureDescriptor currentDescriptor;
    public Texture_WatchAndDateTimeObserver m_timeToBlit;

    private void Awake()
    {
        TryCreateResult();
    }

    private void Update()
    {
        if (m_blitEveryFrame)
        {
            Blit();
        }
    }

    public void SetSource(RenderTexture newSource)
    {
        if (m_source == newSource)
            return;

        m_source = newSource;
        TryCreateResult();
    }

    public void SetAndResizeTexture(RenderTexture source)
    {
            SetSource(source);
            Blit();
    }

    public void SetTargetSize(int width, int height)
    {
        if (m_targetWidth == width && m_targetHeight == height)
            return;

        m_targetWidth = width;
        m_targetHeight = height;
        TryCreateResult();
    }

    private void TryCreateResult()
    {
        if (m_source == null)
        {
            ReleaseResult();
            return;
        }

        var desiredDescriptor = m_source.descriptor;
        desiredDescriptor.width = m_targetWidth;
        desiredDescriptor.height = m_targetHeight;
        desiredDescriptor.useMipMap = false;
        desiredDescriptor.autoGenerateMips = false;

        // Only recreate if m_result is null or the size/descriptor is different
        if (m_result != null &&
            m_result.width == desiredDescriptor.width &&
            m_result.height == desiredDescriptor.height &&
            currentDescriptor.Equals(desiredDescriptor))
        {
            return;
        }

        ReleaseResult();

        currentDescriptor = desiredDescriptor;
        m_result = new RenderTexture(currentDescriptor);
        m_result.Create();

        m_onCreated?.Invoke(m_result);
    }

    public void Blit()
    {
        m_timeToBlit.StartCounting();
        if (m_source == null || m_result == null)
            return;
        Graphics.Blit(m_source, m_result);
        m_timeToBlit.StopCounting();
        m_onUpdated?.Invoke(m_result);
    }

    private void ReleaseResult()
    {
        if (m_result == null)
            return;

        m_result.Release();
        Destroy(m_result);
        m_result = null;
    }

    private void OnDestroy()
    {
        ReleaseResult();
    }
}
