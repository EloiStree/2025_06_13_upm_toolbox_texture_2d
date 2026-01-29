using UnityEngine;

namespace Eloi.TextureUtility { 
    [CreateAssetMenu(
        fileName = "msdoc",
        menuName = "ScriptableObjects/Material Shader with Documentation",
        order = 1
    )]
    public class DocumentedMaterialShaderScritable : ScriptableObject
    {
        public DocumentedMaterialShaderData m_data;
    }

    [System.Serializable]
    public struct DocumentedMaterialShaderData
    {
        public TextAsset m_documentation;
        public Material m_material;
    }
}


