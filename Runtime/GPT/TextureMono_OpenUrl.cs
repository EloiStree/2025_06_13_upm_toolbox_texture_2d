using UnityEngine;

public class TextureMono_OpenUrl : MonoBehaviour
{
    
    public void OpenUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return;
        Application.OpenURL(url);
    }
}
