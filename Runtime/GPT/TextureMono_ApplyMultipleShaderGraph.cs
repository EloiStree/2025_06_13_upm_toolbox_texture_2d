using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_ApplyMultipleShaderGraph : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField]
        private Texture m_sourceTexture;

        public int currentWidth = -1;
        public int currentHeight = -1;
        public RenderTextureFormat m_textureFormat = RenderTextureFormat.Default;

      

        public List<MaterialToRenderTexture> m_shaderGraphToApply = new List<MaterialToRenderTexture>();

        [System.Serializable]
        public class MaterialToRenderTexture
        {
            public bool m_active = true;
            public Material m_material;
            public RenderTexture m_renderTexture;
            public UnityEvent<RenderTexture> m_onRenderTextureCreated;
            public Texture_WatchAndDateTimeObserver m_processTime;
        }

        public RenderTexture m_result;
        public UnityEvent<RenderTexture> m_onBlitsComputed;
        public Texture_WatchAndDateTimeObserver m_processTime;

        public bool m_useUpdate = false;
        public bool m_createMaterialCopyAtAwake;
        private void Awake()
        {
            if (m_createMaterialCopyAtAwake) { 
                for (int i = 0; i < m_shaderGraphToApply.Count ; i++) {
                    m_shaderGraphToApply[i].m_material = new Material(m_shaderGraphToApply[i].m_material);
                }
            }
        }

        void Update()
        {
            if (m_useUpdate)
                ApplyShaders();
        }

        public void SetSourceTexture(Texture sourceTexture)
        {
            m_sourceTexture = sourceTexture;
            Cleanup();
            InitializeRenderTargets();
        }

        public void SetAndProcessSourceTexture(Texture sourceTexture)
        {
            m_sourceTexture = sourceTexture;
            InitializeRenderTargets();
            ApplyShaders();
        }
        private void InitializeRenderTargets()
        {
            if (m_sourceTexture == null)
                return;

            currentWidth = m_sourceTexture.width;
            currentHeight = m_sourceTexture.height;

            for (int i = 0; i < m_shaderGraphToApply.Count; i++)
            {
                if (m_shaderGraphToApply[i].m_renderTexture != null)
                {
                    if (RenderTexture.active == m_shaderGraphToApply[i].m_renderTexture)
                        RenderTexture.active = null;
                    m_shaderGraphToApply[i].m_renderTexture.Release();
                }

                m_shaderGraphToApply[i].m_renderTexture = new RenderTexture(currentWidth, currentHeight, 0, m_textureFormat);
                m_shaderGraphToApply[i].m_renderTexture.filterMode = FilterMode.Point;
                m_shaderGraphToApply[i].m_renderTexture.enableRandomWrite = true;
                m_shaderGraphToApply[i].m_renderTexture.Create();
                m_shaderGraphToApply[i].m_onRenderTextureCreated?.Invoke(m_shaderGraphToApply[i].m_renderTexture);

            }
        }

        private void ApplyShaders()
        {
            if (m_sourceTexture == null || m_shaderGraphToApply.Count == 0)
                return;
            m_processTime.StartCounting();
            m_result = null;
            for (int i = 0; i < m_shaderGraphToApply.Count; i++)
            {
                if (m_shaderGraphToApply[i].m_material == null)
                    continue;
                m_shaderGraphToApply[i].m_processTime.StartCounting();

                Texture from = i == 0 ? m_sourceTexture : m_shaderGraphToApply[i - 1].m_renderTexture;
                MaterialToRenderTexture selected = m_shaderGraphToApply[i];

                RenderTexture active = RenderTexture.active;
                RenderTexture.active = selected.m_renderTexture;
                GL.Clear(true, true, Color.black);
                RenderTexture.active = active;


                if (m_shaderGraphToApply[i].m_active)
                {
                    // Use pass 0 by default, unless you have a reason to use another pass
                    Graphics.Blit(from, selected.m_renderTexture, selected.m_material, 0);
                }
                else {
                    // Would be better if I was not copying but skipping the shader
                    // But I am a bit asleep to find how to do that properly right now.
                    Graphics.Blit(from, selected.m_renderTexture);
                }

                    // Ensure the render texture is active and updated
                    selected.m_renderTexture.DiscardContents();
                m_result = selected.m_renderTexture;
                m_shaderGraphToApply[i].m_processTime.StopCounting();
            }

            m_processTime.StopCounting();
            if (m_result != null)
                m_onBlitsComputed.Invoke(m_result);

        }


        private void Cleanup()
        {
            foreach (var s in m_shaderGraphToApply)
            {
                if (s != null && s.m_renderTexture != null)
                {
                    s.m_renderTexture.Release();
                    s.m_renderTexture = null;
                }
            }
        }

        void OnDestroy()
        {
            Cleanup();
        }
    }
}
