
using UnityEngine;

namespace Eloi.TextureUtility
{
    public class TextureMono_NoParamsWrapperComputeShaderProcessOnRenderTexture : TextureMono_AbstractNoParamsProcessOnRenderTexture
    {
        public TextureMono_NoParamsComputeShaderSourceToResultWH m_processWithComputeShader;


        public void Awake()
        {
            UnhookPreviousSetAndHookNew(m_processWithComputeShader);
        }

         void UnhookPreviousSetAndHookNew(TextureMono_NoParamsComputeShaderSourceToResultWH newShader)
        {
            if (m_processWithComputeShader != null)
            {
                m_processWithComputeShader.m_onUpdated.RemoveListener(m_events.m_onResultProcessed.Invoke);
            }
            m_processWithComputeShader = newShader;

            if (m_processWithComputeShader != null)
            {
                m_processWithComputeShader.m_onUpdated.AddListener(m_events.m_onResultProcessed.Invoke);
            }

        }

        public void SetShaderGraphFilter(TextureMono_NoParamsComputeShaderSourceToResultWH process)
        {
            UnhookPreviousSetAndHookNew(process);
        }
        public override void GetGivenTexture(out RenderTexture textureRenderer)
        {
            textureRenderer= m_processWithComputeShader.m_source;
        }

        public override void GetResultTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = m_processWithComputeShader.m_result;
        }

        public override void StartUsingCode()
        {
            m_processWithComputeShader.gameObject.SetActive(true);
        }

        public override void StopUsingCode()
        {
            m_processWithComputeShader.gameObject.SetActive(false);
        }

        protected override void ProcessGivenTextureCode()
        {
            m_processWithComputeShader.ComputeTheTexture();
        }

        protected override void SetGivenTextureCode(RenderTexture textureRenderer)
        {
            m_processWithComputeShader.SetTextureToUse(textureRenderer);
        }
        public void Update()
        {
            m_processWithComputeShader.m_computeTime.GetTick(out double ticks, out double date);
            SetLastProcessTimeInTicksEstimation((long)ticks);
        }
    }
}


