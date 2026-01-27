using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_InEditorSwitchEvent : MonoBehaviour
    {

        public UnityEvent m_isInEditor;
        public UnityEvent m_isInRuntime;
        public UnityEvent m_isInRuntimeAndroid;
        public UnityEvent m_isInRuntimeWindow;
        public UnityEvent m_isInRuntimeOther;

        public bool m_checkAtStart = true;

        private void Start()
        {
            if (m_checkAtStart)
                CheckEnvironment();
        }
        [ContextMenu("Check Environment")]
        public void CheckEnvironment()
        {
#if UNITY_EDITOR
            m_isInEditor?.Invoke();
#else
                m_isInRuntime?.Invoke();
#if UNITY_ANDROID
                    m_isInRuntimeAndroid?.Invoke();
#elif UNITY_STANDALONE_WIN
                    m_isInRuntimeWindow?.Invoke();
#else
                    m_isInRuntimeOther?.Invoke();
#endif
#endif

        }
    }

}