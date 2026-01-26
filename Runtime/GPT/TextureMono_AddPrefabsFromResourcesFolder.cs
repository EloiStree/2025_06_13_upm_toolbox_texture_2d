
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_AddPrefabsFromResourcesFolder : MonoBehaviour
    {
        [Tooltip("Path in Resources folder to the prefab to instantiate")]
        public string m_resourcePath="Default";
        public GameObject m_whereToCreate;
        public UnityEvent<GameObject> m_onPrefabInstantiated;
        public UnityEvent m_onAllResourceLoad;
        public bool m_loadAtAwake = true;

        public List<GameObject> m_foundPrefab = new List<GameObject>();
        private void Awake()
        {
            if (m_loadAtAwake)
                InstantiatePrefabFromResources();
        }
        public void InstantiatePrefabFromResources()
        {
            m_foundPrefab.Clear();
            GameObject[] prefabs = Resources.LoadAll<GameObject>(m_resourcePath);
            if (prefabs == null || prefabs.Length == 0)
            {
                return;
            }
            foreach (GameObject prefab in prefabs)
            {
                GameObject instance = Instantiate(prefab, m_whereToCreate != null ? m_whereToCreate.transform : null);
                m_foundPrefab.Add(instance);
                m_onPrefabInstantiated?.Invoke(instance);
            }

            m_onAllResourceLoad?.Invoke();
        }
    }
}


