using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {



    public class TextureMono_SearchForScriptableShader<T> : MonoBehaviour where T:Object {
        public string m_relativePathInResources = "";
        public T[] m_found;
        public UnityEvent<T[]> m_onScriptableListFound;
        public bool m_loadAtAwake;
        private void Awake()
        {
            if (m_loadAtAwake)
                SearchInResources();
        }
        [ContextMenu("SearchInResources")]
        public void SearchInResources()
        {
            m_found = Resources.LoadAll<T>(m_relativePathInResources);
            m_onScriptableListFound.Invoke(m_found);
        }


    }
}


