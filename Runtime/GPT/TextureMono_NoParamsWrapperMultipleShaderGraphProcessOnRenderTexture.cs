
using UnityEngine;

namespace Eloi.TextureUtility
{
    public class TextureMono_NoParamsWrapperMultipleShaderGraphProcessOnRenderTexture : TextureMono_AbstractNoParamsProcessOnRenderTexture
    {

      
        public TextureMono_NoParamsApplyMultipleShaderGraph m_processWithShaderGraph;



        public void SetCallBack(RenderTexture renderTexture)
        {
            m_resultDebug = renderTexture;
            m_events.m_onResultProcessed.Invoke(renderTexture);
        }
        //public void Awake()
        //{
        //    UnhookPreviousSetAndHookNew(m_processWithShaderGraph);
        //}

        //void UnhookPreviousSetAndHookNew(TextureMono_ApplyMultipleShaderGraph newShader)
        //{
        //    if (m_processWithShaderGraph != null)
        //    {
        //        m_processWithShaderGraph.m_onBlitsComputed.RemoveListener(SetCallBack);
        //    }
        //    m_processWithShaderGraph = newShader;

        //    if (m_processWithShaderGraph != null)
        //    {
        //        m_processWithShaderGraph.m_onBlitsComputed.AddListener(SetCallBack);
        //    }

        //}

        public void SetShaderGraphFilter(TextureMono_NoParamsApplyMultipleShaderGraph process)
        {
            m_processWithShaderGraph = process;
//            UnhookPreviousSetAndHookNew(process);
        }
        public override void GetGivenTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = m_processWithShaderGraph.GetGivenTexture();
        }

        public override void GetResultTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = m_processWithShaderGraph.GetResultTexture();
        }

        public override void StartUsingCode()
        {
            m_processWithShaderGraph.gameObject.SetActive(true);
        }

        public override void StopUsingCode()
        {
            m_processWithShaderGraph.gameObject.SetActive(false);
        }

        protected override void ProcessGivenTextureCode()
        {
            m_processWithShaderGraph.SetAndProcessSourceTexture(m_givenDebug);
        }

        protected override void SetGivenTextureCode(RenderTexture textureRenderer)
        {
            m_processWithShaderGraph.SetAndProcessSourceTexture(textureRenderer);
        }
        public void Update()
        {
            m_processWithShaderGraph.m_processTime.GetTick(out double ticks, out double date);
            SetLastProcessTimeInTicksEstimation((long)ticks);

            //SetCallBack(m_processWithShaderGraph.GetResultTexture());
        }
    }
    
}


