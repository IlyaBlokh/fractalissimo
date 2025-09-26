using Code.Data;
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
        public Fractal fractal;

        [Header("Output")] 
        public RawImage rawImage;
        
        [Header("Animation Settings")]
        public bool animateTransforms = true;
        public float animationSpeed = 0.5f;
        public float animationOffset = 0.0005f;

        private Texture2D _texture;
        private Color32[] _pixels;
        private TransformData[] _currentTransforms;
        private TransformData[] _targetTransforms;

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
            int n = fractal.Data().Length;
            _currentTransforms = new TransformData[n];
            _targetTransforms = new TransformData[n];
            for (int i = 0; i < n; i++)
            {
                _currentTransforms[i] = fractal.Data()[i];
                _targetTransforms[i] = fractal.Data()[i];
            }
            
            InitializeTexture();
            AllocateMemory();
            ScheduleJobs();
        }

        private void Update()
        {
            if (animateTransforms)
                AnimateTransforms();

            float currentHash = _currentTransforms.Hash();

            if (!Mathf.Approximately(currentHash, _lastHash) && !_jobRunning)
            {
                _lastHash = currentHash;
                ScheduleJobs();
                _jobRunning = true;
            }

            if (!_jobRunning || !_jobHandle.IsCompleted) return;
            
            _jobHandle.Complete();

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


        private void InitializeTexture()
        {
            _texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            _pixels = new Color32[width * height];
            rawImage.texture = _texture;
        }
        
        private void AnimateTransforms()
        {
            for (int i = 0; i < _targetTransforms.Length; i++)
            {
                _targetTransforms[i].a += new float2(Mathf.Sin(Time.time), Mathf.Cos(Time.time)) * animationOffset;
                _targetTransforms[i].b += new float2(Mathf.Cos(Time.time), Mathf.Sin(Time.time)) * animationOffset;
                _targetTransforms[i].c += new float2(Mathf.Sin(Time.time), Mathf.Cos(Time.time)) * animationOffset;
                _targetTransforms[i].d += new float2(Mathf.Cos(Time.time), Mathf.Sin(Time.time)) * animationOffset;
            }

            for (int i = 0; i < _currentTransforms.Length; i++)
            {
                _currentTransforms[i].a = math.lerp(_currentTransforms[i].a, _targetTransforms[i].a, Time.deltaTime * animationSpeed);
                _currentTransforms[i].b = math.lerp(_currentTransforms[i].b, _targetTransforms[i].b, Time.deltaTime * animationSpeed);
                _currentTransforms[i].c = math.lerp(_currentTransforms[i].c, _targetTransforms[i].c, Time.deltaTime * animationSpeed);
                _currentTransforms[i].d = math.lerp(_currentTransforms[i].d, _targetTransforms[i].d, Time.deltaTime * animationSpeed);
            }
        }

        private void ScheduleJobs()
        {
            AllocateMemory();
            
            var boundsJob = new BoundsJob
            {
                iterations = warmupIterations,
                transforms = _transformArray,
                bounds = _bounds
            };

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

        private void AllocateMemory()
        {
            DisposeMemory();

            _transformArray = new NativeArray<TransformData>(_currentTransforms, Allocator.TempJob);
            _colorAccum = new NativeArray<float3>(width * height, Allocator.TempJob);
            _hitCount = new NativeArray<int>(width * height, Allocator.TempJob);
            _pixelArray = new NativeArray<Color32>(width * height, Allocator.TempJob);
            _bounds = new NativeArray<float4>(1, Allocator.TempJob);
        }

        private void DisposeMemory()
        {
            if (_transformArray.IsCreated) _transformArray.Dispose();
            if (_colorAccum.IsCreated) _colorAccum.Dispose();
            if (_hitCount.IsCreated) _hitCount.Dispose();
            if (_pixelArray.IsCreated) _pixelArray.Dispose();
            if (_bounds.IsCreated) _bounds.Dispose();
        }

        private void OnDestroy()
        {
            if (_jobRunning) _jobHandle.Complete();
            DisposeMemory();
        }
    }
}
