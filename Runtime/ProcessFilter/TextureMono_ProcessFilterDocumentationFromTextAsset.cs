using UnityEngine;

public class TextureMono_ProcessFilterDocumentationFromTextAsset : TextureMono_AbstractProcessFilterDocumentation, I_OwnProcessFilterDocumentation
{
    public TextAsset m_textAssetSource;
    public STRUCT_TextureProcessFilterTextInfo m_processFilterTextInfo;
    

    void Awake()
    {
        ImportFileToStruct();
    }
    void OnEnable()
    {
        ImportFileToStruct();
    }


    [ContextMenu("Import From Text Asset")] 
    private void ImportFileToStruct()
    {
        if (m_textAssetSource != null)
        {
            m_processFilterTextInfo = TextureProcessFilterTextFormatImporter.ImportFromTextFile(m_textAssetSource.text);
        }
    }
    public override STRUCT_TextureProcessFilterTextInfo GetProcessFilterTextInfo()
    {
        return m_processFilterTextInfo;
    }
    public void SetTextAsset(TextAsset textAsset)
    {
        m_textAssetSource = textAsset;
        ImportFileToStruct();
    }


}
