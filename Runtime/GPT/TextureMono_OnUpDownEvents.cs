using UnityEngine.Events;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Eloi.TextureUtility
{
    public class TextureMono_OnUpDownEvents : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler
    {
        public bool m_pressState = false;
        [SerializeField] private UnityEvent m_OnDown;
        [SerializeField] private UnityEvent m_OnUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            m_pressState = true;
            m_OnDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_pressState = false;
            m_OnUp?.Invoke();
        }
    }
}


