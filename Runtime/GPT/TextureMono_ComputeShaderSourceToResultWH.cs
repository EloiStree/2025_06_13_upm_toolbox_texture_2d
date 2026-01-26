using System;
using UnityEngine.Events;
using UnityEngine;
namespace Eloi.TextureUtility
{

    public class TextureMono_ComputeShaderSourceToResultWH : MonoBehaviour , I_PushRenderTextureToApply
    {
        public bool m_useTheComputeShader=true;
        // Put the compute shader file with the code here
        public ComputeShader m_computeShaderToApply;
        // a bit of debug
        public RenderTexture m_source;
        public RenderTexture m_result;
        public UnityEvent<RenderTexture> m_onCreated;
        public UnityEvent<RenderTexture> m_onUpdated;
        //You want to observe the computation time of the shader.
        public Texture_WatchAndDateTimeObserver m_computeTime;
        public bool m_useUpdate = false;
        public bool m_useOnEnable = true;
        public bool m_ignoreException;

        public void SetTextureToUse(RenderTexture source){
            m_source = source;
            m_source = CheckForChange(m_source);
        }
        public void SetComputeShaderToUse(ComputeShader shader) { 
            m_computeShaderToApply = shader;
        }
        private RenderTexture CheckForChange(RenderTexture source){
            RenderTextureUtility.CheckThatTextureIsSameSize(ref source, out bool changed, ref m_result);
            if (changed)
            {
                m_onCreated.Invoke(m_result);
            }
            return source;
        }
        [ContextMenu("Recompute Current")]
        public void RecomputeCurrentTexture(){
            SetTextureToUseAndCompute(m_source);
        }
        void OnEnable() {
            if (!enabled)
                return;
            if (m_useOnEnable)
                ComputeTheTexture();
        }

        private void Update(){
            if (!enabled )
                return;
            if (m_useUpdate)
                ComputeTheTexture();
        }
        public void ComputeTheTexture(){
            if (!gameObject.activeInHierarchy)
                return;
            if (!enabled)
                return;
            if (m_computeShaderToApply == null)
                return;
            if (m_result == null || m_computeShaderToApply == null || m_source == null)
                return;

            // Start the clock
            m_computeTime.StartCounting();

            if (!m_useTheComputeShader)
            {
                Graphics.Blit(m_source, m_result);
                m_computeTime.StopCounting();
                m_onUpdated.Invoke(m_result);
                return;
            }
            else { 

                //Change the size if it changed since the last time
                m_source = CheckForChange(m_source);
                // Look for the methode to call in the Shader Code
                int kernelIndex = m_computeShaderToApply.FindKernel("CSMain");

                // Give were to find the pixel and where to apply the new pixels as parameter
                m_computeShaderToApply.SetTexture(kernelIndex, "m_source", m_source);
                m_computeShaderToApply.SetTexture(kernelIndex, "m_result", m_result);
                // Most advance shader will need width and some time the height.
                m_computeShaderToApply.SetInt("m_width", m_source.width);
                m_computeShaderToApply.SetInt("m_height", m_source.height);

                try {
                    // Some will need time for randomness or for continuity
                    m_computeShaderToApply.SetFloat("m_time", Time.timeSinceLevelLoad);
                }
                catch (Exception) { }
                // To avoid making width*height 200000 times in the shader we precompute it here
                // You can precompute some other value here if needed by your shader
                m_computeShaderToApply.SetInt("m_pixelCount", m_source.width * m_source.height);

                // We cut the work in 8x8 pixel chunks and make the GPU work on all chunk in parallel
                int threadGroupsX = Mathf.CeilToInt(m_source.width / 8.0f);
                int threadGroupsY = Mathf.CeilToInt(m_source.height / 8.0f);
                if (m_ignoreException){
                    try{
                        //We stop preparing, we push the code and the data in the GPU and we run it
                        m_computeShaderToApply.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
                        // And we wait...
                    }
                    catch (Exception){}
                }
                else {
                    //Same but without ignoring the errors
                    m_computeShaderToApply.Dispatch(kernelIndex, threadGroupsX, threadGroupsY, 1);
                }
                // If we had to recover data and not a result texture
                // We would add here the code to recovert it.
                m_result.Create();
                m_computeTime.StopCounting();
                // Done
                m_onUpdated.Invoke(m_result);
            }
        }

        public void SetTextureToUseAndCompute(RenderTexture texture){
            SetTextureToUse(texture);
            ComputeTheTexture();
        }
    }
}


