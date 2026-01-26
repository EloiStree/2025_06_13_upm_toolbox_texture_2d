using System.Linq;
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_RenderTextureProcessList : TextureMono_AbstractProcessRenderTextureWithInfo
    {

        [SerializeField]
        private TextureMono_AbstractProcessRenderTextureWithInfo[] m_processes;
        public int m_counter = 0;
        public bool m_counterLoop = false;

        public TextureMono_AbstractProcessRenderTextureWithInfo m_currentFocus;
        public bool m_useUpdateToProcess = false;

        public UnityEvent<TextureMono_AbstractProcessRenderTextureWithInfo> m_onCurrentFocusChanged;

        private RenderTexture m_lastGivenRenderTexture;
        public bool m_autoCompleteAtAwakeWithChildren = true;
        public bool m_disableAllAtAwake = true;

        [ContextMenu("Set Random Index")]
        public void SetRandomIndex()
        {
            if (m_processes.Length == 0)
                return;
            m_counter = Random.Range(0, m_processes.Length);
            SetCurrentFocusFromIndex();
        }
        public void Update()
        {
            if (m_useUpdateToProcess)
            {
                SetCurrentFocusFromIndex();
                if (m_currentFocus != null)
                    m_currentFocus.ProcessGivenTexture();
            }
        }
        public void SetCurrentFocusFromIndex()
        {
            if (m_processes.Length == 0)
                return;

            if (m_currentFocus != null) {

                m_currentFocus.StopUsing();
            }

            if (m_counter < 0)
                m_counter = 0;
            if (m_counter >= m_processes.Length)
                m_counter = m_processes.Length - 1;

            m_currentFocus = m_processes[m_counter];
            if (m_currentFocus == null)
                return;
            m_currentFocus.StartUsing();
            m_currentFocus.SetGivenTexture(m_lastGivenRenderTexture);
            m_onCurrentFocusChanged.Invoke(m_currentFocus);
        }

        public void SetIndex(int index)
        {
            if (index < 0)
                index = 0;
            if (index >= m_processes.Length)
                index = m_processes.Length - 1;
            m_counter = index;
            SetCurrentFocusFromIndex();
        }
        public void SetIndexFromCallId(string callId)
        {
            for (int i = 0; i < m_processes.Length; i++)
            {
                string id;
                m_processes[i].GetCallTextId(out id);
                if (id == callId)
                {
                    m_counter = i;
                    SetCurrentFocusFromIndex();
                    return;
                }
            }
        }
        public void SetIndexFromExactProcessName(string processName)
        {
            for (int i = 0; i < m_processes.Length; i++)
            {
                string name;
                m_processes[i].GetProcessName(out name);
                if (name == processName)
                {
                    m_counter = i;
                    SetCurrentFocusFromIndex();
                    return;
                }
            }
        }
        public void SetIndexFromContainsInProcessName(string processNamePart)
        {
            for (int i = 0; i < m_processes.Length; i++)
            {
                string name;
                m_processes[i].GetProcessName(out name);
                if (name.Contains(processNamePart))
                {
                    m_counter = i;
                    SetCurrentFocusFromIndex();
                    return;
                }
            }
        }
        [ContextMenu("Incress Index")]
        public void IncreaseIndex()
        {
            m_counter++;
            if (m_counter >= m_processes.Length)
            {
                if (m_counterLoop)
                    m_counter = 0;
                else
                    m_counter = m_processes.Length - 1;
            }
            SetCurrentFocusFromIndex();
        }
        [ContextMenu("Decrease Index")]
        public void DecreaseIndex()
        {
            m_counter--;
            if (m_counter < 0)
            {
                if (m_counterLoop)
                    m_counter = m_processes.Length - 1;
                else
                    m_counter = 0;
            }
            SetCurrentFocusFromIndex();
        }
        public void GetProcesses(out TextureMono_AbstractProcessRenderTextureWithInfo[] processes)
        {
            processes = m_processes;
        }


        protected override void SetGivenTextureCode(RenderTexture textureRenderer)
        {
            m_lastGivenRenderTexture = textureRenderer;
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.SetGivenTexture(textureRenderer);
        }

        protected override void ProcessGivenTextureCode()
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.ProcessGivenTexture();

        }

        public override void GetGivenTexture(out RenderTexture textureRenderer)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetGivenTexture(out textureRenderer);
            else
                textureRenderer = null;
        }

        public override void GetResultTexture(out RenderTexture textureRenderer)
        {
            if (m_processes.Length == 0){
                GetGivenTexture(out RenderTexture given);
                textureRenderer = given;
                return;
            }             
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetResultTexture(out textureRenderer);
            else
                textureRenderer = null;
        }

        public override void GetCallTextId(out string id)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetCallTextId(out id);
            else
                id = "";
       }

        public override void GetProcessName(out string name)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetProcessName(out name);
            else
                name = "";
        }

        public override void GetProcessOneLiner(out string oneLiner)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetProcessOneLiner(out oneLiner);
            else
                oneLiner = "";
        }

        public override void GetProcessDescription(out string description)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetProcessDescription(out description);
            else
                description = "";
        }

        public override void GetProcessLearnMoreUrl(out string url)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetProcessLearnMoreUrl(out url);
            else
                url = "";
        }


        private void Awake()
        {
            if (m_autoCompleteAtAwakeWithChildren)
                AutoCompleteProcessWithChildren();
            if (m_disableAllAtAwake)
            {
                for (int i = 0; i < m_processes.Length; i++)
                {
                    if (m_processes[i] != null)
                        m_processes[i].StopUsing();
                }
            }
        }
        [ContextMenu("Auto Complete Process With Childrent")]
        public void AutoCompleteProcessWithChildren()
        {
            m_processes = GetComponentsInChildren<TextureMono_AbstractProcessRenderTextureWithInfo>();
            m_processes = m_processes.Where(item => item != this).ToArray();
        }

        public override void GetCreditUrl(out string url)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetCreditUrl(out url);
            else
                url = "";
        }

        public override void GetCreditName(out string name)
        {
            SetCurrentFocusFromIndex();
            if (m_currentFocus != null)
                m_currentFocus.GetCreditName(out name);
            else
                name = "";
        }
    }
}


