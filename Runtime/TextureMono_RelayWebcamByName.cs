using System;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {

    public class TextureMono_RelayWebcamByName : MonoBehaviour
    {
        public string [] m_nameOfWebcamsToUse;
        public bool m_ignoreCase=true;
        public bool m_exactName=false;

        public WebCamTexture m_texture;
        public UnityEvent<WebCamTexture> m_onWebcamTextureFound;
        public bool m_autoStartTheWebcam = true;

        public bool m_useDefaultWebcamIfNotFound;
        public int m_defaultWebcamIfNotFound = 0;

        public void OnEnable()
        {
            ResetTheWebcam();
        }

        [ContextMenu("Reset Webcam")]
        public void ResetTheWebcam()
        {
            if (m_texture == null)
            {
                TryToConnect();
            }
            else
            {
                m_texture.Stop();
                m_texture = null;
                TryToConnect();
            }
        }

        private void TryToConnect()
        {
            if (m_texture != null)
                return;

            FindDevice(out bool found, out WebCamDevice device);
            if (found) { 

                m_texture = new WebCamTexture(device.name);
                if (m_autoStartTheWebcam)
                {
                    m_texture.Play();
                }
                if (m_texture==null)
                    return;
                m_onWebcamTextureFound?.Invoke(m_texture);
            }
        }
        private void FindDevice(out bool found, out WebCamDevice device)
        {
            foreach (string name in m_nameOfWebcamsToUse) { 
            string toLookFor = name.Trim();
            if ( m_ignoreCase)
            {
                toLookFor = toLookFor.ToLower();
            }
            foreach (WebCamDevice deviceIndex in WebCamTexture.devices) { 
            
                string nameOfWebcam = deviceIndex.name.Trim();

                if (string.IsNullOrEmpty(nameOfWebcam)) 
                    continue;

                if (m_ignoreCase) { 
                    nameOfWebcam = nameOfWebcam.ToLower();
                }

                if (m_exactName && nameOfWebcam == toLookFor)
                { 
                
                    found = true; 
                    device = deviceIndex;
                    return;
                }
                else if (nameOfWebcam.IndexOf(toLookFor)>=0)
                {
                    found = true;
                    device = deviceIndex; 
                    return;
                }
            }
            found = false;
            device = default;
            return;
            }
            if (m_useDefaultWebcamIfNotFound) {

                if (m_defaultWebcamIfNotFound < WebCamTexture.devices.Length) {
                            WebCamDevice deviceIndex = WebCamTexture.devices[m_defaultWebcamIfNotFound];
                            found = true;
                            device = deviceIndex;
                            return;
                }
            }
        found = false;
            device = default;
            return;
        }
    }

}