using UnityEngine;
[ExecuteAlways]
public class WebcamToolMono_GlobalSoundSetter : MonoBehaviour
{
    [Range(0f, 1f)]
    public float m_globalPercentVolume = 1f;

    public void SetGlobalVolume(float percent)
    {
        m_globalPercentVolume = percent;
        ApplyVolume();
    }

    private void OnValidate()
    {
        ApplyVolume();
    }
    public void OnEnable()
    {
        ApplyVolume();
    }
    private void ApplyVolume()
    {
        AudioListener.volume = m_globalPercentVolume;
    }
}
