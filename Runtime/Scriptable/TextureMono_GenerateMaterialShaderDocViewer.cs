using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility {
    public class TextureMono_GenerateMaterialShaderDocViewer : MonoBehaviour
    {

        public Transform m_whereToCreate;
        public GameObject m_prefabToUseForViewer;
        public UnityEvent m_onFinishCreation;
        public string m_nameFormatter = "MS_DOC_{0}";

        public void Awake()
        {
            if (m_whereToCreate == null)
                m_whereToCreate = this.transform;
        }
        public void PushIn(DocumentedMaterialShaderScritable[] scritables)
        {
            foreach (DocumentedMaterialShaderScritable s in scritables)
            {
                GameObject viewer = GameObject.Instantiate(m_prefabToUseForViewer, m_whereToCreate);
                viewer.name = string.Format(m_nameFormatter, s.name);
                TextureMono_ProcessFilterDocumentationFromTextAsset doc = viewer.GetComponentInChildren<TextureMono_ProcessFilterDocumentationFromTextAsset>();
                if (doc != null)
                {
                    doc.SetTextAsset(s.m_data.m_documentation);
                }
                TextureMono_NoParamsApplyMultipleShaderGraph shaderRunner =
                    viewer.GetComponentInChildren<TextureMono_NoParamsApplyMultipleShaderGraph>();
                if (shaderRunner != null)
                {
                    shaderRunner.SetFirstMaterialShaderOrCreate(s.m_data.m_material);
                }
            }
            m_onFinishCreation.Invoke();
        }

    }
}


