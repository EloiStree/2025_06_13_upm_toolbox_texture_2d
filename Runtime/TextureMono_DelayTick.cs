using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_DelayTick : MonoBehaviour
    {

        public float m_secondsBeforeTicks = 1f;
        public Coroutine m_runningCoroutine;
        public UnityEvent m_onTick;
        public bool m_tickAtEnable = true;
        public void OnEnable()
        {
            m_runningCoroutine = StartCoroutine(TickLoop());
        }
        public void OnDisable()
        {
            if (m_runningCoroutine != null)
                StopCoroutine(m_runningCoroutine);
        }
        private IEnumerator TickLoop()
        {
            if (m_tickAtEnable)
                yield return new WaitForSeconds(m_secondsBeforeTicks);
                m_onTick.Invoke();
            }
        }
}