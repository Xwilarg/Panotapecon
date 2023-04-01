using UnityEngine;

namespace MiniJamGame16.Systems
{
    public class AudioSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundsSource;

        public static AudioSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit() {
            Instance = null;
            Destroy(gameObject);
        }

        public void PlayMusic(AudioClip clip) {
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void PlaySound(AudioClip clip, float vol = 1) {
            _soundsSource.PlayOneShot(clip, vol);
        }
    }
}