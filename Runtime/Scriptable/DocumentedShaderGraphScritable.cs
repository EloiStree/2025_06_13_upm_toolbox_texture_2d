
using UnityEngine;
namespace Eloi.TextureUtility
{

[CreateAssetMenu(
    fileName = "sgdoc",
    menuName = "ScriptableObjects/Shader Garph with Documentation",
    order = 1
)]
public class DocumentedShaderGraphScritable : ScriptableObject
{
    public DocumentedShaderGraphData m_data;
}

[System.Serializable]
public struct DocumentedShaderGraphData
{
    public TextAsset m_documentation;
    public Shader m_shaderGraph;
}




}