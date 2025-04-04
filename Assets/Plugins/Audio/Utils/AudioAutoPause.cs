using Plugins.Audio.Core;
using UnityEngine;

namespace Plugins.Audio.Utils
{
    public class AudioAutoPause : MonoBehaviour
    {
        private static AudioAutoPause _instance;
        
        private bool _isFocused = true;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            AppFocusHandle.OnFocus += Focus;
            AppFocusHandle.OnUnfocus += UnFocus;
        }

        private void OnDisable()
        {
            AppFocusHandle.OnFocus -= Focus;
            AppFocusHandle.OnUnfocus -= UnFocus;
        }

        private void Focus()
        {
            if (_isFocused == false)
            {
                AudioManagement.Instance.Pause();

                _isFocused = true;
            }
        }

        private void UnFocus()
        {
            if (_isFocused)
            {
                AudioManagement.Instance.Unpause();
                
                _isFocused = false;
            }
        }
    }
}