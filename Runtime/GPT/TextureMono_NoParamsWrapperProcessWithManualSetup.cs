
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_NoParamsWrapperProcessWithManualSetup : TextureMono_AbstractNoParamsProcessOnRenderTexture
    {
        
        public void SetResultCallback(RenderTexture renderTexture) {

            m_resultDebug = renderTexture;
        }

        public override void GetGivenTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = base.m_givenDebug;   
     
        }

        public override void GetResultTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = base.m_resultDebug;
        }
        protected override void ProcessGivenTextureCode()
        {
        }

        protected override void SetGivenTextureCode(RenderTexture textureRenderer)
        {

        }

        public override void StartUsingCode()
        {
        }

        public override void StopUsingCode()
        {
        }
    }
}


