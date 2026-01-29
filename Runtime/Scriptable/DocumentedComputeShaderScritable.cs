using UnityEngine;

namespace Eloi.TextureUtility
{

[CreateAssetMenu(
    fileName = "csdoc",
    menuName = "ScriptableObjects/Compute Shader with Documentation",
    order = 1
)]
public class DocumentedComputeShaderScritable : ScriptableObject
{
    public DocumentedComputeShaderData m_data;
}

[System.Serializable]
public struct DocumentedComputeShaderData
{
    public TextAsset m_documentation;
    public ComputeShader m_computeShader;
}




}