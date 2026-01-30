using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{

  
    public class TextureMono_ProcessDescriptionSplitter : MonoBehaviour {

        public UnityEvent<string> m_onProcessName;
        public UnityEvent<string> m_onOneLiner;
        public UnityEvent<string> m_onDescription;
        public UnityEvent<string> m_onLearnMoreUrl;
        public UnityEvent<string> m_onCallId;
        public UnityEvent<string> m_onCreditName;
        public UnityEvent<string> m_onCreditUrl;
        public UnityEvent<string> m_onProcessTimeInTicks;



        public TextureMono_AbstractProcessFilterDocumentation m_source;


        [ContextMenu("Open Url")]
        public void OpenUrl()
        {
            if (m_source == null)
                return;
            m_source.GetProcessLearnMoreUrl(out string url);
            if (string.IsNullOrEmpty(url))
                return;
            Application.OpenURL(url);
        }

        public void PushIn(TextureMono_AbstractProcessFilterDocumentation process)
        {
            m_source = process;
            m_onProcessName?.Invoke("");
            m_onOneLiner?.Invoke("");
            m_onDescription?.Invoke("");
            m_onLearnMoreUrl?.Invoke("");
            m_onCallId?.Invoke("");
            m_onCreditName?.Invoke("");
            m_onCreditUrl?.Invoke("");
            m_onProcessTimeInTicks?.Invoke("0");
            process.GetProcessName(out string name);
            m_onProcessName?.Invoke(name);
            process.GetProcessOneLiner(out string oneLiner);
            m_onOneLiner?.Invoke(oneLiner);
            process.GetProcessDescription(out string description);
            m_onDescription?.Invoke(description);
            process.GetProcessLearnMoreUrl(out string url);
            m_onLearnMoreUrl?.Invoke(url);
            process.GetCallTextId(out string id);
            m_onCallId?.Invoke(id);
            process.GetCreditName(out string creditName);
            m_onCreditName?.Invoke(creditName);
            process.GetCreditUrl(out string creditUrl);
            m_onCreditUrl?.Invoke(creditUrl);
            

        }
    }
}


