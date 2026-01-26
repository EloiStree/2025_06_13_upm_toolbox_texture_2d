using UnityEngine.Events;



using UnityEngine;
using System;

namespace Eloi.TextureUtility
{
    public class TextureMono_CropRenderTextureAsMaginPercent : MonoBehaviour
    {
        public RenderTexture m_toCrop;
        public ComputeShader m_cropShader;

        [Range(0, 1)]
        public float m_percentTop = 0.1f;
        [Range(0, 1)]
        public float m_percentDown = 0.1f;
        [Range(0, 1)]
        public float m_percentLeft = 0.1f;
        [Range(0, 1)]
        public float m_percentRight = 0.1f;

        public RenderTexture m_cropped;
        public UnityEvent<RenderTexture> m_onNewCroppedTextureCreated;
        public UnityEvent<RectInt> m_onNewCroppedTextureCreatedRectInt;
        private int m_currentWidth = -1;
        private int m_currentHeight = -1;

        public int m_cropLeftRightX = 0;
        public int m_cropDownTopY = 0;
        public int m_cropWidth = 0;
        public int m_cropHeight = 0;

        public Texture_WatchAndDateTimeObserver m_timeToProcess = new Texture_WatchAndDateTimeObserver();

        void Update()
        {
            if (m_toCrop == null)
                return;

            m_timeToProcess.StartCounting();
            ComputePixelsMargin();
            EnsureCroppedRenderTexture();

            if (m_cropWidth > 0 && m_cropHeight > 0 && m_cropped != null)
            {
                CropRenderTexture();
            }
            m_timeToProcess.StopCounting();
        }

        private void CropRenderTexture()
        {
            if (m_toCrop == null || m_cropped == null || m_cropWidth <= 0 || m_cropHeight <= 0)
                return;

            UseComputeShaderToTransfertCropImage();
            m_onNewCroppedTextureCreatedRectInt?.Invoke(new RectInt(m_cropLeftRightX, m_cropDownTopY, m_cropWidth, m_cropHeight));
        }

        private void UseComputeShaderToTransfertCropImage()
        {
            if (m_cropShader == null || m_toCrop == null || m_cropped == null)
                return;

            int kernel = m_cropShader.FindKernel("CSMain");
            m_cropShader.SetTexture(kernel, "m_source", m_toCrop);
            m_cropShader.SetTexture(kernel, "m_result", m_cropped);

            m_cropShader.SetInt("cropX", m_cropLeftRightX);
            m_cropShader.SetInt("cropY", m_cropDownTopY);
            m_cropShader.SetInt("cropWidth", m_cropWidth);
            m_cropShader.SetInt("cropHeight", m_cropHeight);

            int threadGroupsX = Mathf.CeilToInt(m_cropWidth / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(m_cropHeight / 8.0f);
            m_cropShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);
        }

        private void ComputePixelsMargin()
        {
            if (m_toCrop == null || m_toCrop.width <= 0 || m_toCrop.height <= 0)
            {
                m_cropLeftRightX = 0;
                m_cropDownTopY = 0;
                m_cropWidth = 2;
                m_cropHeight = 2;
                return;
            }

            int width = m_toCrop.width;
            int height = m_toCrop.height;

            int left = Mathf.RoundToInt(m_percentLeft * width);
            int right = Mathf.RoundToInt(m_percentRight * width);
            int top = Mathf.RoundToInt(m_percentDown  * height);
            int down = Mathf.RoundToInt(m_percentTop  * height);

            m_cropLeftRightX = left;
            m_cropDownTopY = top;
            m_cropWidth = Mathf.Max(1, width - (left + right));
            m_cropHeight = Mathf.Max(1, height - (top + down));
        }

        public void SetRenderTexture(RenderTexture renderTexture)
        {
            m_toCrop = renderTexture;
            if (m_toCrop != null)
            {
                m_currentWidth = m_toCrop.width;
                m_currentHeight = m_toCrop.height;
                ComputePixelsMargin();
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
            if (m_toCrop == null || m_cropWidth <= 0 || m_cropHeight <= 0)
                return;

            if (m_cropped == null || m_cropped.width != m_cropWidth || m_cropped.height != m_cropHeight)
            {
                if (m_cropped != null)
                {
                    m_cropped.Release();
                    Destroy(m_cropped);
                }
                m_cropped = new RenderTexture(m_cropWidth, m_cropHeight, 0);
                m_cropped.enableRandomWrite = true;
                m_cropped.filterMode = FilterMode.Point;
                m_cropped.wrapMode = TextureWrapMode.Clamp;

                m_cropped.Create();
                m_currentWidth = m_cropWidth;
                m_currentHeight = m_cropHeight;
                m_onNewCroppedTextureCreated?.Invoke(m_cropped);
            }
        }

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
