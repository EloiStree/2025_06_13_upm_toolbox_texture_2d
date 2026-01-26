
using System;
using UnityEngine.Events;

using UnityEngine;


namespace Eloi.TextureUtility
{
    public class TextureMono_ComputeToLowerResolutionWH : MonoBehaviour, I_PushRenderTextureToApply
    {

        public ComputeShader m_computeShaderToApply;
        public RenderTexture m_source;
        public RenderTexture m_result;
        public UnityEvent<RenderTexture> m_onCreated;
        public UnityEvent<RenderTexture> m_onUpdated;
        public Texture_WatchAndDateTimeObserver m_computeTime;

        public bool m_useUpdate = false;
        public bool m_useOnEnable = true;
        public void SetTextureToUse(RenderTexture source)
        {
            m_source = source;
            m_source = CheckForChange(m_source);
        }

        public void SetComputeShaderToUse(ComputeShader shader)
        {

            m_computeShaderToApply = shader;
        }

        private RenderTexture CheckForChange(RenderTexture source)
        {
            RenderTextureUtility.CheckThatTextureIsSameSize(ref source, out bool changed, ref m_result);
            if (changed)
            {
                m_onCreated.Invoke(m_result);
            }

            return source;
        }



        [ContextMenu("Recompute Current")]
        public void RecomputeCurrentTexture()
        {
            SetTextureToUseAndCompute(m_source);
        }

        void OnEnable()
        {

            if (!enabled)
                return;
            if (m_useOnEnable)
                ComputeTheTexture();

        }

        private void Update()
        {
            if (!enabled)
                return;
            if (m_useUpdate)
                ComputeTheTexture();
        }
        public bool m_ignoreException;
        public void ComputeTheTexture()
        {
            if (!gameObject.activeInHierarchy)
                return;
            if (!enabled)
                return;
            if (m_computeShaderToApply == null)
                return;
            if (m_result == null || m_computeShaderToApply == null || m_source == null)
                return;
            m_computeTime.StartCounting();
            m_source = CheckForChange(m_source);

            int kernelIndex = m_computeShaderToApply.FindKernel("CSMain");

            m_computeShaderToApply.SetTexture(kernelIndex, "m_source", m_source);
            m_computeShaderToApply.SetTexture(kernelIndex, "m_result", m_result);
            m_computeShaderToApply.SetInt("m_width", m_source.width);
            m_computeShaderToApply.SetInt("m_height", m_source.height);

            try
            {
                m_computeShaderToApply.SetFloat("m_time", Time.timeSinceLevelLoad);
            }
            catch (Exception) { }
            m_computeShaderToApply.SetInt("m_pixelCount", m_source.width * m_source.height);



            int threadGroupsX = Mathf.CeilToInt(m_source.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(m_source.height / 8.0f);

            if (m_ignoreException)
            {

                try
                {
                    m_computeShaderToApply.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
                }
                catch (Exception)
                {
                }
            }

            m_result.Create();
            m_computeTime.StopCounting();
            m_onUpdated.Invoke(m_result);
        }

        public void SetTextureToUseAndCompute(RenderTexture texture)
        {
            SetTextureToUse(texture);
            ComputeTheTexture();
        }
    }
}


