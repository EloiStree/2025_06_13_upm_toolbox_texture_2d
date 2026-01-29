using UnityEngine;

public class TextureMono_ProcessFilterDocumentation : TextureMono_AbstractProcessFilterDocumentation, I_OwnProcessFilterDocumentation
{
    public STRUCT_TextureProcessFilterTextInfo m_processFilterTextInfo;
    public  override STRUCT_TextureProcessFilterTextInfo GetProcessFilterTextInfo()
    {
        return m_processFilterTextInfo;
    }
    public void SetProcessFilterDocumentation(string text) {
        m_processFilterTextInfo = TextureProcessFilterTextFormatImporter.ImportFromTextFile(text);
    }
    public void SetProcessFilterDocumentation(TextAsset textAsset)
    {
        m_processFilterTextInfo = TextureProcessFilterTextFormatImporter.ImportFromTextFile(textAsset.text);
    }
}
