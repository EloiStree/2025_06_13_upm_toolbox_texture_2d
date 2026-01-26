using UnityEngine;

namespace Eloi.TextureUtility {
    /// <summary>
    /// From the top left corner to the down right corner
    /// Give the percent information of the rect transform compare to an other parent rect transform
    /// crop can be n child under the parent.
    /// </summary>
    public class TextureMono_RectTransformCropPercentInParentDLTR
        : MonoBehaviour
    {
        public RectTransform m_parentContainer;
        public RectTransform m_cropPercentAnchor;
        public Vector2 m_centerPercent = new Vector2(0.5f, 0.5f);
        public Vector2 m_topLeftPercent = new Vector2(0.0f, 1.0f);
        public Vector2 m_topRightPercent = new Vector2(1.0f, 1.0f);
        public Vector2 m_bottomRightPercent = new Vector2(1.0f, 0.0f);
        public Vector2 m_bottomLeftPercent = new Vector2(0.0f, 0.0f);

        private void Reset()
        {
            m_cropPercentAnchor = GetComponent<RectTransform>();
            m_parentContainer = m_cropPercentAnchor.parent as RectTransform;
            RefrsehInspectorDebugValue();
        }

        public void Update()
        {
            RefrsehInspectorDebugValue();
        }

        public void RefrsehInspectorDebugValue()
        {
            if (m_parentContainer != null && m_cropPercentAnchor != null)
            {
                // Get world corners of parent and anchor
                Vector3[] parentCorners = new Vector3[4];
                Vector3[] anchorCorners = new Vector3[4];
                m_parentContainer.GetWorldCorners(parentCorners);
                m_cropPercentAnchor.GetWorldCorners(anchorCorners);

                // Parent rect in world space
                Vector2 parentMin = parentCorners[0]; // Bottom Left
                Vector2 parentMax = parentCorners[2]; // Top Right
                Vector2 parentSize = parentMax - parentMin;

                // Calculate percent for each anchor corner relative to parent
                // Top Left
                m_topLeftPercent = new Vector2(
                    (anchorCorners[1].x - parentMin.x) / parentSize.x,
                    (anchorCorners[1].y - parentMin.y) / parentSize.y
                );
                // Top Right
                m_topRightPercent = new Vector2(
                    (anchorCorners[2].x - parentMin.x) / parentSize.x,
                    (anchorCorners[2].y - parentMin.y) / parentSize.y
                );
                // Bottom Right
                m_bottomRightPercent = new Vector2(
                    (anchorCorners[3].x - parentMin.x) / parentSize.x,
                    (anchorCorners[3].y - parentMin.y) / parentSize.y
                );
                // Bottom Left
                m_bottomLeftPercent = new Vector2(
                    (anchorCorners[0].x - parentMin.x) / parentSize.x,
                    (anchorCorners[0].y - parentMin.y) / parentSize.y
                );
                // Center
                Vector2 anchorCenter = (anchorCorners[0] + anchorCorners[2]) * 0.5f;
                m_centerPercent = new Vector2(
                    (anchorCenter.x - parentMin.x) / parentSize.x,
                    (anchorCenter.y - parentMin.y) / parentSize.y
                );
            }
        }
    }
    }
