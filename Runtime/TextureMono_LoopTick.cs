using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_LoopTick : MonoBehaviour
    {

        public float m_secondsBetweenTicks = 1f;
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
                m_onTick.Invoke();
            while (true)
            {
                yield return new WaitForSeconds(m_secondsBetweenTicks);
                m_onTick.Invoke();
            }
        }
    }

}