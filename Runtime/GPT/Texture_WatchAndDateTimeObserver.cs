using UnityEngine;
using System;
using System.Diagnostics;
using UnityEngine.Events;

namespace Eloi.TextureUtility
{
    [System.Serializable]
    public class Texture_WatchAndDateTimeObserver
    {

        [System.Serializable]
        public class TickEvents {

            public UnityEvent<long> m_watchTicks = new UnityEvent<long>();
            public UnityEvent<long> m_dateTimeTicks = new UnityEvent<long>();
        }

        [Tooltip("What are you trying to measure ? Reminder and description us at runtime.")]
        [SerializeField] protected double m_watchTimeInTick;
        [SerializeField] protected double m_watchTimeInMilliseconds;
        [SerializeField] protected double m_dateTimeInTick;
        [SerializeField] protected double m_dateTimeInMilliseconds;

        private DateTime m_start;
        private DateTime m_now;
        private Stopwatch m_stopWatch;

        public void GetTick(out double watchStopTick, out double dateTimeTick)
        {
            watchStopTick = m_watchTimeInTick;
            dateTimeTick = m_dateTimeInTick;
        }

        public void GetMilliseconds(out double watchStopMilliseconds, out double dateTimeMilliseconds)
        {
            watchStopMilliseconds = m_watchTimeInMilliseconds;
            dateTimeMilliseconds = m_dateTimeInMilliseconds;
        }

        private void CheckInit()
        {
            if (m_stopWatch != null) return;

            m_start = DateTime.Now;
            m_now = DateTime.Now;
            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
            m_stopWatch.Stop();
        }

        public void StartCounting()
        {
            CheckInit();

            m_start = DateTime.Now;
            m_stopWatch.Reset();
            m_stopWatch.Start();

            m_dateTimeInMilliseconds = 0;
            m_dateTimeInTick = 0;
            m_watchTimeInMilliseconds = 0;
            m_watchTimeInTick = 0;
        }

        public void StopCounting()
        {
            CheckInit();

            m_stopWatch.Stop();
            m_now = DateTime.Now;

            m_dateTimeInMilliseconds = (m_now - m_start).TotalMilliseconds;
            m_dateTimeInTick = (m_now - m_start).Ticks;
            m_watchTimeInMilliseconds = m_stopWatch.ElapsedMilliseconds;
            m_watchTimeInTick = m_stopWatch.ElapsedTicks;

            m_events.m_watchTicks.Invoke((long)m_watchTimeInTick);
            m_events.m_dateTimeTicks.Invoke((long)m_dateTimeInTick);

        }
        public TickEvents m_events = new TickEvents();

        public void WatchTheAction(Action action)
        {
            StartCounting();
            action.Invoke();
            StopCounting();
        }

        public void WatchTheActionAndCatchExceptionAsLog(Action action)
        {
            try
            {
                WatchTheAction(action);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.Log("Exception during watch time: " + exception.StackTrace);
            }
        }
    }
}


