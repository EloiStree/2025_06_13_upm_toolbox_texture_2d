
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility
{

    public abstract class TextureMono_AbstractNoParamsProcessOnRenderTexture : MonoBehaviour
    {

        [System.Serializable]
        public class Events { 
            public UnityEvent<RenderTexture> m_onSetGivenTexture;
            public UnityEvent<RenderTexture> m_onResultProcessed;
            public UnityEvent m_onStartUsing;
            public UnityEvent m_onStopUsing;
        }
        public Events m_events = new Events();

        [SerializeField] protected RenderTexture m_givenDebug;
        [SerializeField] protected RenderTexture m_resultDebug;
        [SerializeField] protected long m_lastProcessTimeInTicksEstimation;

        public void StartUsing()
        {
            StartUsingCode();
            m_events.m_onStartUsing?.Invoke();
        }
        public void StopUsing()
        {
            StopUsingCode();
            m_events.m_onStopUsing?.Invoke();
        }

        public abstract void StartUsingCode();
        public abstract void StopUsingCode();
        public void SetLastProcessTimeInTicksEstimation(long ticks)
        {
            m_lastProcessTimeInTicksEstimation = ticks;
        }
        public void ProcessGivenTexture()
        {
            ProcessGivenTextureCode();
            GetResultTexture(out RenderTexture result);
            m_resultDebug = result;
            m_events.m_onResultProcessed?.Invoke(result);
        }
        public void SetGivenTexture(RenderTexture textureRenderer)
        {
            SetGivenTextureCode(textureRenderer);
            GetGivenTexture(out RenderTexture given);
            m_givenDebug = textureRenderer;
            m_events.m_onSetGivenTexture?.Invoke(given);
        }
        protected abstract void SetGivenTextureCode(RenderTexture textureRenderer);
        protected abstract void ProcessGivenTextureCode();
        public abstract void GetGivenTexture(out RenderTexture textureRenderer);
        public abstract void GetResultTexture(out RenderTexture textureRenderer);
        public void SetAndProcessGivenTexture(RenderTexture textureRenderer)
        {
            SetGivenTexture(textureRenderer);
            ProcessGivenTexture();
        }
        public void GetLastProcessTimeInTicks(out long ticks) {

            ticks = m_lastProcessTimeInTicksEstimation;
        }



    }
    
}


