using UnityEngine.Events;




using UnityEngine;

namespace Eloi.TextureUtility
{


    public class TextureMono_CropRenderTextureAsUnityRectInt : MonoBehaviour
    {
        public RenderTexture m_toCrop;
        public int m_cropLeftRightX = 0;
        public int m_cropDownTopY = 0;
        public int m_cropWidth = 256;
        public int m_cropHeight = 256;

        public RenderTexture m_cropped; 
        public UnityEvent<RenderTexture> m_onNewCroppedTextureCreated;
        public UnityEvent<RectInt> m_onNewCroppedTextureCreatedRectInt;
        private int m_currentWidth = -1;
        private int m_currentHeight = -1;

        public Texture_WatchAndDateTimeObserver m_timeToProcess = new Texture_WatchAndDateTimeObserver();
        void Update()
        {
            m_timeToProcess.StartCounting();
            EnsureCroppedRenderTexture();

            // Copy subregion from sourceRT to croppedRT
            if (m_toCrop != null && m_cropped != null)
            {
                Graphics.CopyTexture(m_toCrop, 0, 0, m_cropLeftRightX, m_cropDownTopY, m_cropWidth, m_cropHeight,
                                     m_cropped, 0, 0, 0, 0);
            }
            m_timeToProcess.StopCounting();
        }

        public void SetRectangle(Rect rect)
        {
            m_cropLeftRightX = Mathf.RoundToInt(rect.xMin);
            m_cropDownTopY = Mathf.RoundToInt(rect.yMin);
            m_cropWidth = Mathf.RoundToInt(rect.width);
            m_cropHeight = Mathf.RoundToInt(rect.height);
            EnsureCroppedRenderTexture();
            m_onNewCroppedTextureCreatedRectInt.Invoke(new RectInt(m_cropLeftRightX, m_cropDownTopY, m_cropWidth, m_cropHeight));
        }

        public void SetRectangle(RectInt rectInt)
        {
            m_cropLeftRightX = rectInt.xMin;
            m_cropDownTopY = rectInt.yMin;
            m_cropWidth = rectInt.width;
            m_cropHeight = rectInt.height;
            EnsureCroppedRenderTexture();
            m_onNewCroppedTextureCreatedRectInt.Invoke(rectInt);
        }
            

        public void SetRenderTexture(RenderTexture renderTexture) {

            m_toCrop = renderTexture;
            if (m_toCrop != null)
            {
                m_currentWidth = m_toCrop.width;
                m_currentHeight = m_toCrop.height;
                EnsureCroppedRenderTexture();
            }
            else
            {
                m_toCrop = null;
                m_currentWidth = -1;
                m_currentHeight = -1;
            }
        }

        private void EnsureCroppedRenderTexture()
        {
            if (m_cropped == null || m_cropWidth != m_currentWidth || m_cropHeight != m_currentHeight)
            {
                if (m_cropped != null)
                {
                    m_cropped.Release();
                    Destroy(m_cropped);
                }

                m_cropped = new RenderTexture(m_cropWidth, m_cropHeight, 0, RenderTextureFormat.ARGB32);
                m_cropped.Create();

                m_currentWidth = m_cropWidth;
                m_currentHeight = m_cropHeight;
                m_onNewCroppedTextureCreated?.Invoke(m_cropped);
            }
        }

        // Optional: Convert current croppedRT to a Texture2D
        public Texture2D GetCroppedTexture2D()
        {
            RenderTexture activeBackup = RenderTexture.active;
            RenderTexture.active = m_cropped;

            Texture2D tex = new Texture2D(m_cropWidth, m_cropHeight, TextureFormat.ARGB32, false);
            tex.ReadPixels(new Rect(0, 0, m_cropWidth, m_cropHeight), 0, 0);
            tex.Apply();

            RenderTexture.active = activeBackup;
            return tex;
        }

        // Optional cleanup
        void OnDestroy()
        {
            if (m_cropped != null)
            {
                m_cropped.Release();
                Destroy(m_cropped);
            }
        }
    }
}
