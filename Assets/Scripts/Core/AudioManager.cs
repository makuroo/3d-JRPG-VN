using Data.SfxData;
using DG.Tweening;
using UnityEngine;

namespace Core
{
   public class AudioManager : MonoBehaviour
   {
      [SerializeField] private AudioSource _bgmAudioSource;
      [SerializeField] private SfxDatabase _sfxDatabase;
      [SerializeField] private AudioSource[] _audioSources;
  
      public static AudioManager Instance { get; private set; }

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
      }

      public AudioSource PlaySfx(string id, Vector2 position, float volume = 1f, float pitch = 1f)
      {
         foreach (var audioSource in _audioSources)
         {
            if(audioSource.isPlaying) continue;
            audioSource.clip = _sfxDatabase.GetClipById(id);
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.transform.position = position;
            audioSource.PlayOneShot(audioSource.clip);
            return audioSource;
         }
         return null;
      }

      public void Crossfade(string id, float duration)
      {
         _bgmAudioSource.DOFade(0,duration).OnComplete(() =>
         {
            _bgmAudioSource.Stop();
            _bgmAudioSource.clip = _sfxDatabase.GetClipById(id);
            _bgmAudioSource.Play();
            _bgmAudioSource.DOFade(.2f, duration);
         });
      }
   }
}
