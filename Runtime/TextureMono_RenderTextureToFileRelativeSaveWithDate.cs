using System;
using UnityEngine;

namespace Eloi.TextureUtility {

    public class TextureMono_RenderTextureToFileRelativeSaveWithDate : MonoBehaviour
    {
        public RenderTexture m_renderTexture;
        public int m_width = -1;
        public int m_height = -1;
        public int m_pixelCounts = -1;
        public int m_bytesCountsRGBA = -1;
        public int m_bytesCountsRGB = -1;
        public string m_dateFormat = "yyyy_MM_dd_HH_mm_ss";
        public string m_permanentPathRelativeFolder = "SavedImages";

        [ContextMenu("Remove Image Folder")]
        public void RemoveImageFolder()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, m_permanentPathRelativeFolder);
            if (System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path, true);
            }
        }

        [ContextMenu("Open Peristent Data Folder")]
        public void OpenPeristentDataFolder()
        {
            UnityEngine.Application.OpenURL(Application.persistentDataPath);
        }
        public void SetRenderTexture(RenderTexture renderTexture)
        {
            m_renderTexture = renderTexture;
            if (m_renderTexture != null)
            {
                m_width = m_renderTexture.width;
                m_height = m_renderTexture.height;
                m_pixelCounts = m_width * m_height;
                m_bytesCountsRGBA = m_pixelCounts * 4;
                m_bytesCountsRGB = m_pixelCounts * 3;
            }
            else
            {
                m_width = -1;
                m_height = -1;
                m_pixelCounts = -1;
                m_bytesCountsRGBA = -1;
                m_bytesCountsRGB = -1;
            }
        }
        public bool m_mipmap = false;
        public bool m_linear = false;

        [ContextMenu("Open Permanent Folder")]
        public void OpenPermanentFolder()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, m_permanentPathRelativeFolder);
            if (System.IO.Directory.Exists(path))
            {
                UnityEngine.Application.OpenURL(path);
            }
            else
            {
                Debug.LogWarning($"The directory {path} does not exist.");
            }
        }

        [ContextMenu("Save as TGA")]
        public void SaveAsTGA()
        {
            if (m_renderTexture == null || m_renderTexture.width < 1)
                return;
            Texture2D texture = new Texture2D(m_width, m_height, TextureFormat.RGB24, m_mipmap, m_linear);
            RenderTexture.active = m_renderTexture;
            texture.ReadPixels(new Rect(0, 0, m_width, m_height), 0, 0);
            texture.Apply();
            byte[] bytes = texture.EncodeToTGA();
            string date = System.DateTime.Now.ToString(m_dateFormat);
            string fileName = $"{date}.tga";
            string path = System.IO.Path.Combine(Application.persistentDataPath, m_permanentPathRelativeFolder, fileName);
            RenderTexture.active = null;
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            System.IO.File.WriteAllBytes(path, bytes);
            Destroy(texture);
        }
        [ContextMenu("Save as JPEG")]
        public void SaveAsJPEG() {
            if (m_renderTexture == null || m_renderTexture.width < 1)
                return;

            Texture2D texture = new Texture2D(m_width, m_height, TextureFormat.RGB24,m_mipmap,m_linear );
            RenderTexture.active = m_renderTexture;
            texture.ReadPixels(new Rect(0, 0, m_width, m_height), 0, 0);
            texture.Apply();
            byte[] bytes = texture.EncodeToJPG();
            string date = System.DateTime.Now.ToString(m_dateFormat);
            string fileName = $"{date}.jpg";
            string path = System.IO.Path.Combine(Application.persistentDataPath, m_permanentPathRelativeFolder, fileName);
            RenderTexture.active = null;
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            System.IO.File.WriteAllBytes(path, bytes);
            Destroy(texture);
        }

        [ContextMenu("Save as PNG")]
        public void SaveAsPNG()
        {
            if (m_renderTexture == null || m_renderTexture.width < 1)
                return;
            Texture2D texture = new Texture2D(m_width, m_height, TextureFormat.RGB24, m_mipmap, m_linear);
            RenderTexture.active = m_renderTexture;
            texture.ReadPixels(new Rect(0, 0, m_width, m_height), 0, 0);
            texture.Apply();
            byte[] bytes = texture.EncodeToPNG();
            string date = System.DateTime.Now.ToString(m_dateFormat);
            string fileName = $"{date}.png";
            string path = System.IO.Path.Combine(Application.persistentDataPath, m_permanentPathRelativeFolder, fileName);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            System.IO.File.WriteAllBytes(path, bytes);
            RenderTexture.active = null;
            Destroy(texture);
        }

        [ContextMenu("Save as Color32")]
        public void SaveAsColor32() {
            if (m_renderTexture == null || m_renderTexture.width < 1)
                return;
            byte[] bytes = new byte[m_bytesCountsRGB+8];
            int width = m_width;
            int height = m_height;
            BitConverter.GetBytes(width).CopyTo(bytes, 0);
            BitConverter.GetBytes(height).CopyTo(bytes, 4);
            Texture2D texture = new Texture2D(m_width, m_height, TextureFormat.RGB24, m_mipmap, m_linear);
            RenderTexture.active = m_renderTexture;
            texture.ReadPixels(new Rect(0, 0, m_width, m_height), 0, 0);
            texture.Apply();
            Color32[] color32Array = texture.GetPixels32();
            for (int i = 0; i < color32Array.Length; i++)
            {
                bytes[i + 8] = color32Array[i].r;
                bytes[i + 8 + m_pixelCounts] = color32Array[i].g;
                bytes[i + 8 + m_pixelCounts * 2] = color32Array[i].b;
            }
            string date = System.DateTime.Now.ToString(m_dateFormat);
            string fileName = $"{date}.color32";
            string path = System.IO.Path.Combine(Application.persistentDataPath, m_permanentPathRelativeFolder, fileName);
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            System.IO.File.WriteAllBytes(path, bytes);
            RenderTexture.active = null;
            Destroy(texture);
        }
    }

}