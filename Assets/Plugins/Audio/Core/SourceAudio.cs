using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Plugins.Audio.Core
{
    public class SourceAudio : MonoBehaviour
    {
        [SerializeField] private AudioProviderType _provider = AudioProviderType.Unity;
        [SerializeField] private float _volume = 1;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _mute;
        [SerializeField] private float _pitch = 1;
        [SerializeField] private AudioMixerGroup _output;
        
        [SerializeField] private bool _bypassEffects = false;
        [SerializeField] private bool _bypassListenerEffects = false;
        [SerializeField] private bool _bypassReverbZones = false;
        
        [SerializeField] private float _stereoPan = 0;
        [SerializeField] private float _spatialBlend = 0;
        [SerializeField] private float _reverbZoneMix = 1;

        [SerializeField] private float _dopplerLevel = 1;
        [SerializeField] private float _spread = 0;
        [SerializeField] private AudioRolloffMode _volumeRolloff = AudioRolloffMode.Logarithmic;
        [SerializeField] private float _minDistance = 1;
        [SerializeField] private float _maxDistance = 500;
        
        private AudioProvider _currentProvider;
        
        public string CurrentKey { get; private set; }

        public event Action OnFinished;

        public float Volume
        {
            get => _currentProvider.Volume;
            set
            {
                _volume = value;
                _currentProvider.Volume = value;
            }
        }

        public bool Mute
        {
            get => _currentProvider.Mute;
            set
            {
                _mute = value;
                _currentProvider.Mute = value;
            }
        }

        public bool Loop
        {
            get => _currentProvider.Loop;
            set
            {
                _loop = value;
                _currentProvider.Loop = value;
            }
        }

        public float Pitch
        {
            get => _currentProvider.Pitch;
            set
            {
                _pitch = Mathf.Clamp(value, -0.5f, 3);
                _currentProvider.Pitch = _pitch;
            }
        }

        public float Time
        {
            get => _currentProvider.Time;
            set => _currentProvider.Time = value;
        }

        public bool IsPlaying => _currentProvider.IsPlaying;

        public void Play(string key, float time = 0)
        {
            CurrentKey = key;
            _currentProvider.Play(key, time);
        }

        public void PlayOneShot(string key)
        {
            _currentProvider.PlayOneShot(key);
        }

        public void Stop()
        {
            CurrentKey = string.Empty;
            _currentProvider.Stop();
        }

        public void Pause()
        {
            _currentProvider.Pause();
        }

        public void UnPause()
        {
            _currentProvider.UnPause();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_currentProvider != null)
            {
                RefreshData();
            }   
        }
#endif

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (_provider == AudioProviderType.Unity)
            {
                _currentProvider = new StreamingAudioProvider(this);
            }
            else if (_provider == AudioProviderType.JS)
            {
                _currentProvider = new JsAudioProvider(this);
            }
#else
            _currentProvider = new UnityAudioProvider(this);
#endif

            RefreshData();
        }

        private void RefreshData()
        {
            AudioSettings settings = new AudioSettings()
            {
                volume = _volume,
                mute = _mute,
                loop = _loop,
                pitch = _pitch,
                mixerGroup = _output,
                
                bypassEffects = _bypassEffects,
                bypassListenerEffects = _bypassListenerEffects,
                bypassReverbZones = _bypassReverbZones,

                stereoPan = _stereoPan,
                spatialBlend = _spatialBlend,
                reverbZoneMix = _reverbZoneMix,

                dopplerLevel = _dopplerLevel,
                spread = _spread,
                volumeRolloff = _volumeRolloff,
                minDistance = _minDistance,
                maxDistance = _maxDistance,
            };

            _currentProvider.RefreshSettings(settings);
        }

        private void OnDestroy()
        {
            _currentProvider.Dispose();
        }

        private void OnEnable()
        {
            AudioManagement.Instance.OnPause += OnAudioPaused;
            AudioManagement.Instance.OnUnpause += OnAudioUnpaused;
        }

        private void OnDisable()
        {
            AudioManagement.Instance.OnPause -= OnAudioPaused;
            AudioManagement.Instance.OnUnpause -= OnAudioUnpaused;
        }

        private void Update()
        {
            _currentProvider.Update();
        }

        private void OnAudioPaused()
        {
            _currentProvider.OnGlobalAudioPaused();
        }

        private void OnAudioUnpaused()
        {
            _currentProvider.OnGlobalAudioUnpaused();
        }

        [Serializable]
        public struct AudioSettings
        {
            public float volume;
            public bool loop;
            public bool mute;
            public float pitch;
            public AudioMixerGroup mixerGroup;

            public bool bypassEffects;
            public bool bypassListenerEffects;
            public bool bypassReverbZones;

            public float stereoPan;
            public float spatialBlend;
            public float reverbZoneMix;

            public float dopplerLevel;
            public float spread;
            public AudioRolloffMode volumeRolloff;
            public float minDistance;
            public float maxDistance;
        }

        public void ClipFinished()
        {
            AudioManagement.Instance.Log("Audio clip finished: " + CurrentKey);

            OnFinished?.Invoke();
        }
    }
}
