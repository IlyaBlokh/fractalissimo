using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class FractalFlameJobs : MonoBehaviour
    {
        [Header("Fractal Settings")] 
        public int width = 512;
        public int height = 512;
        public int iterations = 5_000_000;
        public int warmupIterations = 100_000;
        public TransformConfig config;

        [Header("Output")] public RawImage rawImage;

        private Texture2D _texture;
        private Color32[] _pixels;

        private NativeArray<TransformData> _transformArray;
        private NativeArray<float3> _colorAccum;
        private NativeArray<int> _hitCount;
        private NativeArray<Color32> _pixelArray;
        private NativeArray<float4> _bounds;

        private JobHandle _jobHandle;
        private bool _jobRunning;
        private float _lastHash;

        private void Start()
        {
            InitializeTexture();
            ScheduleJob();
        }

        private void Update()
        {
            // Detect parameter changes by hashing transforms
            int currentHash = GetTransformsHash();
            if (!Mathf.Approximately(currentHash, _lastHash))
            {
                Debug.Log($"New transforms hash: {currentHash}");
                _lastHash = currentHash;
                if (_jobRunning) _jobHandle.Complete();
                ScheduleJob();
            }

            if (_jobRunning && _jobHandle.IsCompleted)
            {
                _jobHandle.Complete();

                // Normalize color accumulators into pixel array
                for (int i = 0; i < _pixelArray.Length; i++)
                {
                    int hits = _hitCount[i];
                    if (hits > 0)
                    {
                        float3 col = _colorAccum[i] / hits; // average color
                        col = math.sqrt(col); // gamma correction (optional glow look)
                        _pixelArray[i] = new Color(col.x, col.y, col.z, 1f);
                    }
                    else
                    {
                        _pixelArray[i] = new Color32(0, 0, 0, 255);
                    }
                }

                _pixelArray.CopyTo(_pixels);
                _texture.SetPixels32(_pixels);
                _texture.Apply();

                rawImage.texture = _texture;
                _jobRunning = false;
            }
        }

        private void InitializeTexture()
        {
            _texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            _pixels = new Color32[width * height];
            rawImage.texture = _texture;
        }

        private void ScheduleJob()
        {
            if (_transformArray.IsCreated) _transformArray.Dispose();
            if (_colorAccum.IsCreated) _colorAccum.Dispose();
            if (_hitCount.IsCreated) _hitCount.Dispose();
            if (_pixelArray.IsCreated) _pixelArray.Dispose();
            if (_bounds.IsCreated) _bounds.Dispose();

            _transformArray = new NativeArray<TransformData>(config.Transforms, Allocator.TempJob);
            _colorAccum = new NativeArray<float3>(width * height, Allocator.TempJob);
            _hitCount = new NativeArray<int>(width * height, Allocator.TempJob);
            _pixelArray = new NativeArray<Color32>(width * height, Allocator.TempJob);
            _bounds = new NativeArray<float4>(1, Allocator.TempJob);

            // First job: compute bounds
            var boundsJob = new BoundsJob
            {
                iterations = warmupIterations,
                transforms = _transformArray,
                bounds = _bounds
            };

            // Second job: render using bounds
            var renderJob = new FractalJob
            {
                width = width,
                height = height,
                iterations = iterations,
                transforms = _transformArray,
                colorAccum = _colorAccum,
                hitCount = _hitCount,
                bounds = _bounds
            };

            _jobHandle = boundsJob.Schedule();
            _jobHandle = renderJob.Schedule(_jobHandle);

            _jobRunning = true;
        }

        private int GetTransformsHash()
        {
            const int w = 31;
            int hash = 17;
            unchecked
            {
                foreach (TransformData t in config.Transforms)
                {
                    // probability
                    hash = hash * w + t.probability.GetHashCode();

                    // matrix components
                    hash = hash * w + t.a.x.GetHashCode();
                    hash = hash * w + t.a.y.GetHashCode();
                    hash = hash * w + t.b.x.GetHashCode();
                    hash = hash * w + t.b.y.GetHashCode();

                    hash = hash * w + t.c.x.GetHashCode();
                    hash = hash * w + t.c.y.GetHashCode();
                    hash = hash * w + t.d.x.GetHashCode();
                    hash = hash * w + t.d.y.GetHashCode();

                    // color
                    hash = hash * w + t.color.r.GetHashCode();
                    hash = hash * w + t.color.g.GetHashCode();
                    hash = hash * w + t.color.b.GetHashCode();
                    hash = hash * w + t.color.a.GetHashCode();
                }

                return hash;
            }
        }


        private void OnDestroy()
        {
            if (_jobRunning)
                _jobHandle.Complete();

            if (_transformArray.IsCreated) _transformArray.Dispose();
            if (_colorAccum.IsCreated) _colorAccum.Dispose();
            if (_hitCount.IsCreated) _hitCount.Dispose();
            if (_pixelArray.IsCreated) _pixelArray.Dispose();
        }
    }
}
