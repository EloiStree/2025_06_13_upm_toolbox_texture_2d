using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_GenerateComputeShaderDocViewer : MonoBehaviour
    {

        public Transform m_whereToCreate;
        public GameObject m_prefabToUseForViewer;
        public UnityEvent m_onFinishCreation;
        public string m_nameFormatter = "CS_DOC_{0}";

        public void Awake()
        {
            if (m_whereToCreate == null)
                m_whereToCreate = this.transform;
        }
        public void PushIn(DocumentedComputeShaderScritable[] scritables)
        {
            foreach (DocumentedComputeShaderScritable s in scritables)
            {
                GameObject viewer = GameObject.Instantiate(m_prefabToUseForViewer, m_whereToCreate);
                viewer.name = string.Format(m_nameFormatter, s.name);
                TextureMono_ProcessFilterDocumentationFromTextAsset doc = viewer.GetComponentInChildren<TextureMono_ProcessFilterDocumentationFromTextAsset>();
                if (doc != null)
                {
                    doc.SetTextAsset(s.m_data.m_documentation);
                }
                TextureMono_NoParamsComputeShaderSourceToResultWH shaderRunner =
                    viewer.GetComponentInChildren<TextureMono_NoParamsComputeShaderSourceToResultWH>();
                if (shaderRunner != null)
                {
                    shaderRunner.SetComputeShaderToUse(s.m_data.m_computeShader);
                }
            }
            m_onFinishCreation.Invoke();
        }

    }
}


