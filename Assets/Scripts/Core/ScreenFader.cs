using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class ScreenFader  : MonoBehaviour
    {
        public static ScreenFader Instance;
        [SerializeField] private Image _screenFader;
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
            DontDestroyOnLoad(this);
        }

        public void FadeIn(float duration, Action callback = null)
        {
            _screenFader.DOFade(1, duration).OnComplete(() => callback?.Invoke());
        }

        public void FadeOut(float duration, Action callback = null)
        {
            _screenFader.DOFade(0, duration).OnComplete(() => callback?.Invoke());
        }
    }
}
