using UnityEngine;

namespace Eloi.TextureUtility 
{
    public class TextureMono_SetStereoEyesShaderMono : MonoBehaviour
    {
        public Renderer[] m_targetsRendererToAffect;
        public Material[] m_targetsMaterialToAffect;
        public string m_materialLeftName = "_Left";
        public string m_materialRightName = "_Right";
        [Header("Textures Debug")]
        public Texture m_leftTexture;
        public Texture m_rightTexture;


        public void SetLeftTexture(Texture left)
        {
            m_leftTexture = left; Refresh();

        }
        public void SetRightTexture(Texture right)
        {
            m_rightTexture = right; Refresh();

        }
        public void SetTextureS(Texture left, Texture right)
        {
            m_leftTexture = left;
            m_rightTexture = right;
            Refresh();
        }

        public void Refresh()
        {
            if (m_targetsRendererToAffect != null)
            {
                for (int i = 0; i < m_targetsRendererToAffect.Length; i++)
                {
                    if (m_targetsRendererToAffect[i] != null && m_targetsRendererToAffect[i].material != null)
                    {
                        m_targetsRendererToAffect[i].material.SetTexture(m_materialLeftName, m_leftTexture);
                        m_targetsRendererToAffect[i].material.SetTexture(m_materialRightName, m_rightTexture);
                    }
                }

                for (int i = 0; i < m_targetsMaterialToAffect.Length; i++)
                {
                    if (m_targetsMaterialToAffect[i] != null)
                    {
                        m_targetsMaterialToAffect[i].SetTexture(m_materialLeftName, m_leftTexture);
                        m_targetsMaterialToAffect[i].SetTexture(m_materialRightName, m_rightTexture);
                    }
                }
            }
        }
    }

}
