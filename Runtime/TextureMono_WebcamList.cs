using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {

    public class TextureMono_WebcamList : MonoBehaviour {
        public List<string> m_devicesNameList = new List<string>();
        public UnityEvent<string[]> m_onWebcamListRefesh;

        [ContextMenu("Refresh")]
        public void Refresh()
        {
            m_devicesNameList= WebCamTexture.devices.Select (x => x.name).ToList ();
            m_onWebcamListRefesh?.Invoke(m_devicesNameList.ToArray());
        }
        private void Awake()
        {
           Refresh();
        }
    }
}