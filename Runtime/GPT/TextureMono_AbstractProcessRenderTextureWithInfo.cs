
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility
{


    public abstract class TextureMono_AbstractProcessRenderTextureWithInfo : MonoBehaviour
    {
        public UnityEvent<RenderTexture> m_onSetGivenTexture;
        public UnityEvent<RenderTexture> m_onResultProcessed;
        public UnityEvent m_onStartUsing;
        public UnityEvent m_onStopUsing;

        [SerializeField] protected RenderTexture m_givenDebug;
        [SerializeField] protected RenderTexture m_resultDebug;
        [SerializeField] protected long m_lastProcessTimeInTicksEstimation;

        public void StartUsing()
        {
            m_onStartUsing?.Invoke();
        }
        public void StopUsing()
        {
            m_onStopUsing?.Invoke();
        }
        public void SetLastProcessTimeInTicksEstimation(long ticks)
        {
            m_lastProcessTimeInTicksEstimation = ticks;
        }
        public abstract void GetCallTextId(out string id);
        public void ProcessGivenTexture()
        {
            ProcessGivenTextureCode();
            GetResultTexture(out RenderTexture result);
            m_resultDebug = result;
            m_onResultProcessed?.Invoke(result);
        }
        public void SetGivenTexture(RenderTexture textureRenderer)
        {
            SetGivenTextureCode(textureRenderer);
            GetGivenTexture(out RenderTexture given);
            m_givenDebug = textureRenderer;
            m_onSetGivenTexture?.Invoke(given);
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
        public void OpenLearnMoreUrl()
        {
            GetProcessLearnMoreUrl(out string urlToLearnMoreAboutIt);
            Application.OpenURL(urlToLearnMoreAboutIt);
        }
        public abstract void GetProcessName(out string name);
        public abstract void GetProcessOneLiner(out string oneLiner);
        public abstract void GetProcessDescription(out string description);
        public abstract void GetProcessLearnMoreUrl(out string url);

        public abstract void GetCreditUrl(out string url);
        public abstract void GetCreditName(out string name);
        public void GetLastProcessTimeInTicks(out long ticks) {

            ticks = m_lastProcessTimeInTicksEstimation;
        }

    }
    public abstract class TextureMono_AbstractProcessRenderTextureWithInfoVariable : TextureMono_AbstractProcessRenderTextureWithInfo
    {
        [SerializeField] string m_processName = "Process Name";
        [SerializeField] string m_processOneLinerDescription = "Description in one line";
        [TextArea(3, 10)]
        [SerializeField] string m_processShortDescription = "Short Description of what is happening";
        [Tooltip("To allows the user to learn about the shader structure and how to do it.")]
        [SerializeField] string m_urlToLearnMoreAboutIt = "https://google.com";
        [Tooltip("Unique id to be called from a scanner call.")]
        [SerializeField] string m_callTextId = "";

        [SerializeField] string m_creditName = "";
        [SerializeField] string m_creditUrl = "";


        protected void Reset()
        {
            m_callTextId = System.Guid.NewGuid().ToString().Replace("-", "");
        }

        public override void GetCallTextId(out string id)
        {
            id = m_callTextId;
        }

        public override void GetProcessName(out string name)
        {
            name = m_processName;
        }
        public override void GetProcessOneLiner(out string oneLiner)
        {
            oneLiner = m_processOneLinerDescription;
        }
        public override void GetProcessDescription(out string description)
        {
            description = m_processShortDescription;
        }
        public override void GetProcessLearnMoreUrl(out string url)
        {
            url = m_urlToLearnMoreAboutIt;
        }
        public override void GetCreditUrl(out string url)
        {
            url = m_creditUrl;
        }
        public override void GetCreditName(out string name)
        {
            name = m_creditName;
        }
    }
}


