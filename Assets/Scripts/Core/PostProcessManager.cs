using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core
{
    public class PostProcessManager : MonoBehaviour
    {
        public static PostProcessManager Instance;

        [SerializeField] private Volume _volume;
        private Vignette _vignette;
        private Bloom _bloom;
        
        private Tween _vignetteStartTween;
        private Tween _vignetteEndTween;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);

            if (_volume)
            {
                _volume.profile.TryGet(out _vignette);
                _volume.profile.TryGet(out _bloom);
            }
        }

        public void ActivateVignette(float intensity, Color color, float duration)
        {
            _vignette.color.value = color;
           
            if (_vignetteStartTween == null)
            {
                _vignetteStartTween = DOVirtual.Float(0, intensity, 0.5f, value =>
                {
                    Debug.Log(value.ToString());
                    _vignette.intensity.value = value;
                });
            }
            else
            {
                _vignetteStartTween.Restart();
            }
            
            if (duration > -1)
            {
                if (_vignetteEndTween == null)
                {
                    _vignetteEndTween = DOVirtual.DelayedCall(duration, () =>
                    {
                        DOVirtual.Float(_vignette.intensity.value, 0, 0.2f, value =>
                        {
                            _vignette.intensity.value = value;
                        });
                    });
                }
                else
                {
                    _vignetteEndTween.Restart();
                }
            }
        }

        public void ActivateBloom(float intensity, Color color, float duration)
        {
            _bloom.intensity.value = intensity;
            _bloom.tint.value = color;
            
            if (duration > -1)
            {
                DOVirtual.DelayedCall(duration, () =>
                {
                    DOVirtual.Float(_bloom.intensity.value, 0, 0.2f, value =>
                    {
                        _bloom.intensity.value = value;
                    });
                });
            }
        }
    }
}
