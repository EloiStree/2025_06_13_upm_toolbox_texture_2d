using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_ProcessDescriptionSplitter : MonoBehaviour {

        public TextureMono_AbstractProcessRenderTextureWithInfo m_process;
        public UnityEvent<string> m_onProcessName;
        public UnityEvent<string> m_onOneLiner;
        public UnityEvent<string> m_onDescription;
        public UnityEvent<string> m_onLearnMoreUrl;
        public UnityEvent<string> m_onCallId;
        public UnityEvent<string> m_onCreditName;
        public UnityEvent<string> m_onCreditUrl;
        public UnityEvent<string> m_onProcessTimeInTicks;


        public bool m_pushOnEnable = true;
        private void OnEnable()
        {
            if (m_pushOnEnable)
                PushIn(m_process);
        }
        public void Update()
        {
            if (m_process != null)
            {
                UpdateTickInfo();
            }
        }

        private void UpdateTickInfo()
        {
            m_process.GetLastProcessTimeInTicks(out long ticks);
            m_onProcessTimeInTicks?.Invoke(ticks.ToString());
        }

        public void PushIn(TextureMono_AbstractProcessRenderTextureWithInfo process)
        {
            if (process == null) {

                m_onProcessName?.Invoke("");
                m_onOneLiner?.Invoke("");
                m_onDescription?.Invoke("");
                m_onLearnMoreUrl?.Invoke("");
                m_onCallId?.Invoke("");
                m_onCreditName?.Invoke("");
                m_onCreditUrl?.Invoke("");
                m_onProcessTimeInTicks?.Invoke("0");
                return;
            }
            m_process = process;
            m_process.GetProcessName(out string name);
            m_onProcessName?.Invoke(name);
            m_process.GetProcessOneLiner(out string oneLiner);
            m_onOneLiner?.Invoke(oneLiner);
            m_process.GetProcessDescription(out string description);
            m_onDescription?.Invoke(description);
            m_process.GetProcessLearnMoreUrl(out string url);
            m_onLearnMoreUrl?.Invoke(url);
            m_process.GetCallTextId(out string id);
            m_onCallId?.Invoke(id);
            m_process.GetCreditName(out string creditName);
            m_onCreditName?.Invoke(creditName);
            m_process.GetCreditUrl(out string creditUrl);
            m_onCreditUrl?.Invoke(creditUrl);
            UpdateTickInfo();

        }
    }
}


