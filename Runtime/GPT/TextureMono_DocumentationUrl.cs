using UnityEngine;
namespace Eloi.TextureUtility
{
    public class TextureMono_DocumentationUrl : MonoBehaviour
    {
        [TextArea(1, 1)]
        public string m_documentationUrl = "";
        [TextArea(2, 10)]
        public string m_description = "";

        [ContextMenu("Open Documentation URL")]
        public void OpenUrl()
        {
            if (!string.IsNullOrEmpty(m_documentationUrl))
                Application.OpenURL(m_documentationUrl);
        }
    }
}
