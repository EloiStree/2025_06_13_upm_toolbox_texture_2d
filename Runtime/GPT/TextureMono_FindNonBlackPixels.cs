
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TextureUtility
{
    public class TextureMono_FindNonBlackPixels : MonoBehaviour
    {
        [System.Serializable]
        public class PixelPositionEvent : UnityEvent<Vector2Int[]> { }

        public ComputeShader computeShader;

        public PixelPositionEvent OnPixelsFound = new PixelPositionEvent();

        private ComputeBuffer resultBuffer;
        private ComputeBuffer countBuffer;
        public RenderTexture sourceTexture;

        private int kernel;
        private const int THREADS_X = 8;
        private const int THREADS_Y = 8;
        private int maxPixels;

        public Vector2Int[] foundPixels;

        public Texture_WatchAndDateTimeObserver timeToProcess;

        void Start()
        {
            kernel = computeShader.FindKernel("CSMain");
        }

        public void SetRenderTexture(RenderTexture tex)
        {
            if (!tex.enableRandomWrite)
            {
                Debug.LogError("RenderTexture must have enableRandomWrite set to true.");
                return;
            }

            sourceTexture = tex;
            maxPixels = tex.width * tex.height;

            // Init or reallocate buffers
            resultBuffer?.Release();
            resultBuffer = new ComputeBuffer(maxPixels, sizeof(int) * 2, ComputeBufferType.Append);

            countBuffer?.Release();
            countBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }

        public bool m_useUpdate = true;

        void Update()
        {
            if (sourceTexture == null || computeShader == null)
                return;

            if (m_useUpdate)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            timeToProcess.StartCounting();
            RunComputeShader();

            timeToProcess.StopCounting();
        }

        void RunComputeShader()
        {
            resultBuffer.SetCounterValue(0);

            computeShader.SetTexture(kernel, "Source", sourceTexture);
            computeShader.SetBuffer(kernel, "ResultBuffer", resultBuffer);

            int groupsX = Mathf.CeilToInt(sourceTexture.width / (float)THREADS_X);
            int groupsY = Mathf.CeilToInt(sourceTexture.height / (float)THREADS_Y);
            computeShader.Dispatch(kernel, groupsX, groupsY, 1);

            // Copy count to CPU buffer
            ComputeBuffer.CopyCount(resultBuffer, countBuffer, 0);
            int[] countArray = { 0 };
            countBuffer.GetData(countArray);
            int count = countArray[0];

            if (count > 0)
            {
                var rawResults = new int[count * 2];
                resultBuffer.GetData(rawResults, 0, 0, count * 2);

                Vector2Int[] vectorResults = new Vector2Int[count];
                for (int i = 0; i < count; i++)
                {
                    vectorResults[i] = new Vector2Int(rawResults[i * 2], rawResults[i * 2 + 1]);
                }

                foundPixels = vectorResults;
                OnPixelsFound.Invoke(vectorResults);
            }
            else
            {
                foundPixels = new Vector2Int[0];
                OnPixelsFound.Invoke(new Vector2Int[0]);
            }
        }

        void OnDestroy()
        {
            resultBuffer?.Release();
            countBuffer?.Release();
        }
    }
}
