using Eloi.TextureUtility;
using UnityEngine;

public class TextureMono_SyncProcessFilterListLeftRight : MonoBehaviour
{
    public TextureMono_RenderTextureProcessList m_leftList;
    public TextureMono_RenderTextureProcessList m_rightList;



    public void SyncLeftToRightIndex()
    {

        if (m_leftList == null || m_rightList == null)
            return;
        m_rightList.m_counter = m_leftList.m_counter;
        m_rightList.SetCurrentFocusFromIndex();
    }
    public void SyncRightToLeftIndex()
    {
        if (m_leftList == null || m_rightList == null)
            return;
        m_leftList.m_counter = m_rightList.m_counter;
        m_leftList.SetCurrentFocusFromIndex();
    }

    public void IncrementBoth()
    {
        if (m_leftList == null || m_rightList == null)
            return;
        m_leftList.m_counter++;
        m_leftList.SetCurrentFocusFromIndex();
        m_rightList.m_counter ++;
        m_rightList.SetCurrentFocusFromIndex();
    }
    public void DecrementBoth()
    {
        if (m_leftList == null || m_rightList == null)
            return;
        m_leftList.m_counter--;
        m_leftList.SetCurrentFocusFromIndex();
        m_rightList.m_counter--;
        m_rightList.SetCurrentFocusFromIndex();
    }

}
