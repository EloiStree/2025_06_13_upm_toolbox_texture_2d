
//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using System.Collections.Generic;

//namespace Eloi.TextureUtility
//{



//    public class TextureMono_ApplyMultipleShaderGraph : MonoBehaviour
//    {
//        [Header("Input")]
//        [SerializeField]
//        private Texture m_sourceTexture;

//        public int currentWidth = -1;
//        public int currentHeight = -1;
//        public RenderTextureFormat m_textureFormat = RenderTextureFormat.Default;

//        public void SetSourceTexture(Texture sourceTexture)
//        {
//            m_sourceTexture = sourceTexture;
//            Cleanup();
//            InitializeRenderTargets();
//        }

//        public List<MaterialToRenderTexture> m_shaderGraphToApply = new List<MaterialToRenderTexture>();

//        [System.Serializable]
//        public class MaterialToRenderTexture
//        {
//            public Material m_material;
//            public RenderTexture m_renderTexture;
//            public UnityEvent<RenderTexture> m_onRenderTextureCreated;
//        }

//        public RenderTexture m_result;
//        public UnityEvent<RenderTexture> m_onBlitsComputed;
//        public bool m_useUpdate = false;
//        public bool m_createMaterialCopyAtAwake;
//        private void Awake()
//        {
//            if (m_createMaterialCopyAtAwake)
//            {
//                for (int i = 0; i < m_shaderGraphToApply.Count; i++)
//                {
//                    m_shaderGraphToApply[i].m_material = new Material(m_shaderGraphToApply[i].m_material);
//                }
//            }
//        }

//        void Update()
//        {
//            if (m_useUpdate)
//                ApplyShaders();
//        }

//        private void ApplyShaders()
//        {
//            if (m_sourceTexture == null || m_shaderGraphToApply.Count == 0)
//                return;

//            m_result = null;

//            for (int i = 0; i < m_shaderGraphToApply.Count; i++)
//            {
//                if (m_shaderGraphToApply[i].m_material == null)
//                    continue;

//                Texture from = i == 0 ? m_sourceTexture : m_shaderGraphToApply[i - 1].m_renderTexture;
//                MaterialToRenderTexture selected = m_shaderGraphToApply[i];

//                RenderTexture active = RenderTexture.active;
//                RenderTexture.active = selected.m_renderTexture;
//                GL.Clear(true, true, Color.black);
//                RenderTexture.active = active;

//                // Use pass 0 by default, unless you have a reason to use another pass
//                Graphics.Blit(from, selected.m_renderTexture, selected.m_material, 0);

//                // Ensure the render texture is active and updated
//                selected.m_renderTexture.DiscardContents();
//                m_result = selected.m_renderTexture;
//            }

//            if (m_result != null)
//                m_onBlitsComputed.Invoke(m_result);
//        }

//        private void InitializeRenderTargets()
//        {
//            if (m_sourceTexture == null)
//                return;

//            currentWidth = m_sourceTexture.width;
//            currentHeight = m_sourceTexture.height;

//            for (int i = 0; i < m_shaderGraphToApply.Count; i++)
//            {
//                if (m_shaderGraphToApply[i].m_renderTexture != null)
//                    m_shaderGraphToApply[i].m_renderTexture.Release();

//                m_shaderGraphToApply[i].m_renderTexture = new RenderTexture(currentWidth, currentHeight, 0, m_textureFormat);
//                m_shaderGraphToApply[i].m_renderTexture.filterMode = FilterMode.Point;
//                m_shaderGraphToApply[i].m_renderTexture.enableRandomWrite = true;
//                m_shaderGraphToApply[i].m_renderTexture.Create();
//                m_shaderGraphToApply[i].m_onRenderTextureCreated?.Invoke(m_shaderGraphToApply[i].m_renderTexture);

//            }
//        }

//        private void Cleanup()
//        {
//            foreach (var s in m_shaderGraphToApply)
//            {
//                if (s != null && s.m_renderTexture != null)
//                {
//                    s.m_renderTexture.Release();
//                    s.m_renderTexture = null;
//                }
//            }
//        }

//        void OnDestroy()
//        {
//            Cleanup();
//        }

//        internal RenderTexture GetGivenTexture()
//        {
//            return m_sourceTexture as RenderTexture;
//        }

//        internal RenderTexture GetResultTexture()
//        {
//            return m_result;
//        }

//        internal void ProcessSourceGiven()
//        {
//            ApplyShaders(); 
//        }
//    }
//}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.TextureUtility
{
    public class TextureMono_NoParamsApplyMultipleShaderGraph : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField]
        private Texture m_sourceTexture;

        public int currentWidth = -1;
        public int currentHeight = -1;
        public RenderTextureFormat m_textureFormat = RenderTextureFormat.ARGBHalf;



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

        public RenderTexture GetGivenTexture()
        {
            return m_sourceTexture as RenderTexture;
        }
        private void Awake()
        {
            if (m_createMaterialCopyAtAwake)
            {
                for (int i = 0; i < m_shaderGraphToApply.Count; i++)
                {
                    m_shaderGraphToApply[i].m_material = new Material(m_shaderGraphToApply[i].m_material);
                }
            }
        }

        void Update()
        {
            if (m_useUpdate)
                ApplyShaders();
        }


        public void SetSourceTexture(WebCamTexture webcam) {

            m_sourceTexture = webcam as Texture;
        }
        public void SetSourceTexture(RenderTexture sourceTexture)
        {
            m_sourceTexture = sourceTexture;
            Cleanup();
            InitializeRenderTargets();
        }

        public void SetAndProcessSourceTexture(RenderTexture sourceTexture)
        {
            SetSourceTexture(sourceTexture);
            ApplyShaders();
        }

        public void ProcessSourceGiven()
        {
            //InitializeRenderTargets();
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
                m_shaderGraphToApply[i].m_renderTexture.wrapMode = TextureWrapMode.Clamp;
                // Set as RGBA32
                m_shaderGraphToApply[i].m_renderTexture.enableRandomWrite = true;
                m_shaderGraphToApply[i].m_renderTexture.Create();
                m_shaderGraphToApply[i].m_onRenderTextureCreated?.Invoke(m_shaderGraphToApply[i].m_renderTexture);

            }
        }
        public string m_textureName = "_MainTex";
        private void ApplyShaders()
        {
            if (m_sourceTexture == null || m_shaderGraphToApply.Count == 0)
                return;

            m_processTime.StartCounting();
            m_result = null;

            Texture currentSource = m_sourceTexture;

            for (int i = 0; i < m_shaderGraphToApply.Count; i++)
            {
                var shaderStep = m_shaderGraphToApply[i];
                if (shaderStep.m_material == null || shaderStep.m_renderTexture == null)
                    continue;

                shaderStep.m_processTime.StartCounting();

                RenderTexture target = shaderStep.m_renderTexture;

                if (!target.IsCreated())
                    target.Create();

                shaderStep.m_material.SetTexture(m_textureName, currentSource);

                
                if (shaderStep.m_active)
                {
                    Graphics.Blit(null, target, shaderStep.m_material, 0);
                }
                else
                {
                    Graphics.Blit(null, target);
                }

                //if (shaderStep.m_active)
                //{
                //    Graphics.Blit(currentSource, target, shaderStep.m_material, 0);
                //}
                //else
                //{
                //    Graphics.Blit(currentSource, target);
                //}

                // Output becomes input for the next pass
                currentSource = target;
                m_result = target;

                shaderStep.m_processTime.StopCounting();
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
                    // Ensure we are not releasing the currently active RenderTexture
                    if (RenderTexture.active == s.m_renderTexture)
                    {
                        RenderTexture.active = null;
                    }
                    s.m_renderTexture.Release();
                    s.m_renderTexture = null;
                }
            }
        }

        void OnDestroy()
        {
            Cleanup();
        }

        public RenderTexture GetResultTexture()
        {
            return m_result;
        }

        public void SetFirstMaterialShaderOrCreate(Material m_material)
        {
            if (m_shaderGraphToApply.Count == 0)
            {
                MaterialToRenderTexture newMat = new MaterialToRenderTexture();
                newMat.m_material = m_material;
                newMat.m_processTime = new Texture_WatchAndDateTimeObserver();
                m_shaderGraphToApply.Add(newMat);
            }
            else
            {
                m_shaderGraphToApply[0].m_material = m_material;
            }
        }
    }
}
