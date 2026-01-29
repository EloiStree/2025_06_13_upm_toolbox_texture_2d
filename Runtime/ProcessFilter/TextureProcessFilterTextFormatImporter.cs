public class TextureProcessFilterTextFormatImporter 
{
    public static STRUCT_TextureProcessFilterTextInfo ImportFromTextFile(string textFileContents)
    {
        STRUCT_TextureProcessFilterTextInfo outInfo = new STRUCT_TextureProcessFilterTextInfo();
        string[] lines = textFileContents.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        System.Text.StringBuilder descriptionBuilder = new System.Text.StringBuilder();
        bool isDescriptionSection = false;
        foreach (string line in lines)
        {
            if (line.StartsWith("PROCESS_NAME:"))
            {
                outInfo.m_processName = line.Substring("PROCESS_NAME:".Length).Trim();
            }
            else if (line.StartsWith("ONE_LINER:"))
            {
                outInfo.m_processOneLiner = line.Substring("ONE_LINER:".Length).Trim();
            }
            else if (line.StartsWith("LEARN_MORE_URL:"))
            {
                outInfo.m_urlToLearnMoreAboutIt = line.Substring("LEARN_MORE_URL:".Length).Trim();
            }
            else if (line.StartsWith("CALL_TEXT_ID:"))
            {
                outInfo.m_callTextId = line.Substring("CALL_TEXT_ID:".Length).Trim();
            }
            else if (line.StartsWith("CREATOR_NAME:"))
            {
                outInfo.m_creatorName = line.Substring("CREATOR_NAME:".Length).Trim();
            }
            else if (line.StartsWith("CREATOR_CONTACT_URL:"))
            {
                outInfo.m_creatorContactUrl = line.Substring("CREATOR_CONTACT_URL:".Length).Trim();
            }
            else if (line.StartsWith("DESCRIPTION:"))
            {
                isDescriptionSection = true;
                descriptionBuilder.AppendLine(line.Substring("DESCRIPTION:".Length).Trim());
            }
            else if (isDescriptionSection)
            {
                descriptionBuilder.AppendLine(line.Trim());
            }
        }
        outInfo.m_processDescription = descriptionBuilder.ToString().Trim();
        return outInfo;
    }

}
