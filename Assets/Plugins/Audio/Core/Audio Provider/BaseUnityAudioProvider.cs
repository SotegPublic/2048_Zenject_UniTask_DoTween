using UnityEngine;

namespace Plugins.Audio.Core
{
    public abstract class BaseUnityAudioProvider : AudioProvider
    {
        protected AudioSource CreateAudioSource(SourceAudio sourceAudio)
        {
            GameObject gameObject = new GameObject("Source");

            if (AudioManagement.Instance.CanDebug == false)
            {
                gameObject.hideFlags = HideFlags.HideAndDontSave;
            }
            else
            {
                gameObject.hideFlags = HideFlags.NotEditable;
            }
            
            gameObject.transform.SetParent(sourceAudio.transform);
            
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localEulerAngles = Vector3.zero;
            
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            return source;
        }
    }
}