using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_GenerateComputeShaderUnDocFileViewer : MonoBehaviour
    {

        public Transform m_whereToCreate;
        public GameObject m_prefabToUseForViewer;
        public UnityEvent m_onFinishCreation;
        public string m_nameFormatter = "CS_UNDOC_{0}";

        public void PushIn(ComputeShader[] shaders)
        {
            foreach (ComputeShader s in shaders)
            {
                
                GameObject viewer = GameObject.Instantiate(m_prefabToUseForViewer, m_whereToCreate);
                viewer.name = string.Format(m_nameFormatter, s.name);
                TextureMono_ProcessFilterDocumentation doc = viewer.GetComponentInChildren<TextureMono_ProcessFilterDocumentation>();
                if (doc != null)
                {
                    doc.m_processFilterTextInfo = new STRUCT_TextureProcessFilterTextInfo()
                    {
                        m_processName = s.name,
                        m_callTextId = "CS_UNDOC_"+s.name,
                        m_processOneLiner = "Undocumented Compute Shader.",
                        m_processDescription = "No documentation available.",
                    };
                }
                TextureMono_NoParamsComputeShaderSourceToResultWH shaderRunner =
                    viewer.GetComponentInChildren<TextureMono_NoParamsComputeShaderSourceToResultWH>();
                if (shaderRunner != null)
                {
                    shaderRunner.SetComputeShaderToUse(s);
                }
            }
            m_onFinishCreation.Invoke();
        }

    }
}


