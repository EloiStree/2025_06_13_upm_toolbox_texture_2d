using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility 
{
    public class TextureMono_OnUpdate : MonoBehaviour
    {
        public UnityEvent m_onUpdate;
        private void Update()
        {
            m_onUpdate?.Invoke();
        }
    }

}
