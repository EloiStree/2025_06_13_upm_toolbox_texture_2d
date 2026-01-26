
using System;
using UnityEngine.Events;

namespace Eloi.TextureUtility
{
    using UnityEngine;

    public class TextureMono_SourceResultShaderToy : MonoBehaviour, I_PushRenderTextureToApply
    {

        public ComputeShader m_computeShaderToApply;
        public RenderTexture m_source;
        public RenderTexture m_channel1;
        public RenderTexture m_result;
        public UnityEvent<RenderTexture> m_onCreated;
        public UnityEvent<RenderTexture> m_onUpdated;
        public Texture_WatchAndDateTimeObserver m_computeTime;
        public float m_mousePercentX;
        public float m_mousePercentY;
        public float[] m_sounds= new float[0];
        public float m_time=0;
        public bool m_useUnityTime = true;

        public bool m_useUpdate = false;
        public bool m_useOnEnable = true;

        [Header("Debug")]
        public Vector2Int m_mousePositionXY;
        public void SetTextureToUse(RenderTexture source)
        {
            m_source = source;
            m_source = CheckForChange(m_source);
        }

        public void SetTextureChannel1(RenderTexture channel1) { 
        
            m_channel1 = channel1;
        }


        public void SetTimeInSeconds(float timeInSeconds) { 
        
            m_time = timeInSeconds;
        }
        public void SetSound(float[] soundsAsFloatArray) {
            m_sounds = soundsAsFloatArray;
        }

        public void SetMouseInfoAsPixel(Vector2Int mouseInfo)
        {
            m_mousePositionXY = mouseInfo;
            m_mousePercentX = mouseInfo.x / m_source.width;
            m_mousePercentY = mouseInfo.y / m_source.height;
        }
        public void SetMouseInfoAsPercentOfTexture(Vector2 mouseInfo)
        {
            m_mousePercentX = mouseInfo.x;
            m_mousePercentY = mouseInfo.y;
            m_mousePositionXY = new Vector2Int( (int)(m_mousePercentX * m_source.width), (int)(m_mousePercentY * m_source.height) );
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

            if (m_useUnityTime)
                m_time = Time.time;
            try
            {
                m_computeShaderToApply.SetFloat("m_time", m_time);
            }
            catch (Exception) { }
            try
            {
                m_computeShaderToApply.SetInt("m_mousePixelX", m_mousePositionXY.x);
            }
            catch (Exception) { }

            try
            {
                m_computeShaderToApply.SetInt("m_mousePixelY", m_mousePositionXY.x);
            }
            catch (Exception) { }
            try
            {
                m_computeShaderToApply.SetInt("m_channel1", m_mousePositionXY.x);
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


