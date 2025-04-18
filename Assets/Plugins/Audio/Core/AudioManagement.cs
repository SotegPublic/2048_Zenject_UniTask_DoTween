using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Plugins.Audio.Core
{
    public class AudioManagement : MonoBehaviour
    {
        public static AudioManagement Instance { get; private set; }
        public bool CanDebug => _configuration.CanDebug;

        private Dictionary<string, AudioClip> _cechAudio = new Dictionary<string, AudioClip>();
        private AudioConfiguration _configuration;
        private AudioDatabase _database;
        
        public event Action OnPause;
        public event Action OnUnpause;
        
        public bool IsAudioPause { get; private set; }
        private float _volume;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            GameObject instance = new GameObject("Audio Management");
            Instance = instance.AddComponent<AudioManagement>();

            Application.runInBackground = true;
            DontDestroyOnLoad(instance); 
        }

        private void Awake()
        {
#if UNITY_WEBGL
            WebAudio.Init();
#endif
            _configuration = AudioConfiguration.GetInstance();
            _database = _configuration.GetDatabase();
            _database.Initialize();
            IsAudioPause = false;

            PreloadAudio();
        }

        public IEnumerator GetClip(string key, Action<AudioClip> result)
        {
            if (_cechAudio.TryGetValue(key, out AudioClip clip))
            {
                result.Invoke(clip);
            }
            else 
            {
                AudioData audioData = _database.GetData(key);
                
#if UNITY_EDITOR || !UNITY_WEBGL
                result.Invoke(audioData.Clip);
                yield break;
#endif
                string path = Application.streamingAssetsPath + "/Audio/" + audioData.FolderPath + audioData.Name;
                yield return LoadClip(path, audioData.Key, result);
            }
        }

        private void PreloadAudio()
        {
            if (_database == null)
            {
                return;
            }

#if UNITY_EDITOR || !UNITY_WEBGL
            foreach (AudioData audioData in _database.Items)
            {
                if (audioData.Preload == false)
                {
                    continue;
                }
                
                _cechAudio[audioData.Key] = audioData.Clip;
            }
            
            return;
#endif

            foreach (AudioData audioData in _database.Items)
            {
                if (audioData.Preload == false)
                {
                    continue;
                }
                
                string path = Application.streamingAssetsPath + "/Audio/" + audioData.FolderPath + audioData.Name;
                StartCoroutine(LoadClip(path, audioData.Key));
            }
        }

        private IEnumerator LoadClip(string path, string key, Action<AudioClip> result = null)
        {
            float startTime = Time.time;

            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
                    _cechAudio[key] = audioClip;

                    result?.Invoke(audioClip);
                    
                    Log("Audio clip loaded: " + key + " time: " + (Time.time - startTime));
                }
            }
        }

        public string GetClipPath(string key)
        {
            AudioData audioData = _database.GetData(key);

            return audioData.FolderPath + audioData.Name;
        }

        public void Log(string message)
        {
            if (CanDebug == true)
            {
                Debug.Log(message);
            }
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
        
        public void Pause()
        {
            if (IsAudioPause == true)
            {
                return;
            }
            
            AudioListener.pause = true;
#if UNITY_WEBGL
            WebAudio.Mute(true);
#endif
            IsAudioPause = true;
            
            OnPause?.Invoke();
            Log("Pause Audio");
        }
        
        public void Unpause()
        {
            if (IsAudioPause == false)
            {
                return;
            }
            
            AudioListener.pause = false;

#if UNITY_WEBGL
            WebAudio.Mute(false);
#endif
            
            IsAudioPause = false;

            OnUnpause?.Invoke();
            Log("UnPause Audio");
        }

        public void SetVolume(float value)
        {
            _volume = Mathf.Clamp01(value);

            AudioListener.volume = _volume;

#if UNITY_WEBGL
            WebAudio.SetVolume(_volume);
#endif
        }

        public float GetVolume() => _volume;
    }
}
