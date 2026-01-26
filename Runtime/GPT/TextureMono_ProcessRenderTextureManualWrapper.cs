
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_ProcessRenderTextureManualWrapper : TextureMono_AbstractProcessRenderTextureWithInfoVariable
    {
        public UnityEvent<RenderTexture> m_onRequestToProcessTexture;
        //[Header("Hook SetGivenTexture event to your code to set the process image")]
        //private string m_setupYourProcessHere = "";
        //[Header("Call from your code SetResultCallback when your process is done")]
        //private string m_callSetResultWhenDoneHere = "";

        public void SetResultCallback(RenderTexture renderTexture) {

            m_resultDebug = renderTexture;
            m_onResultProcessed?.Invoke(renderTexture);
        }

        public override void GetGivenTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = base. m_givenDebug;   
     
        }

        public override void GetResultTexture(out RenderTexture textureRenderer)
        {
            textureRenderer = base.m_resultDebug;
        }
        protected override void ProcessGivenTextureCode()
        {
            GetGivenTexture(out RenderTexture given);
            m_onRequestToProcessTexture.Invoke(given);
        }

        protected override void SetGivenTextureCode(RenderTexture textureRenderer)
        {

        }
    }
}


