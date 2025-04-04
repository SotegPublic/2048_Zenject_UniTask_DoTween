using Plugins.Audio.Core;
using UnityEngine;
using Zenject;

public class SoundManager : MonoBehaviour, ISoundManager, IGetAdvClosedNotification, IGetAdvNotification, IGetRewardedAdvClosedNotification, IGetRewardedAdvNotification
{
    [SerializeField] private SourceAudio _backgroundSource;
    [SerializeField] private SourceAudio _mergeEffectSource;

    private IPlayerModel playerModel;

    [Inject]
    public void Construct(IPlayerModel model)
    {
        playerModel = model;
    }

    public void Init()
    {
        if (playerModel.IsSoundOff)
        {
            SwitchSound(true);
        }
    }

    public void OnAdvClose()
    {
        if (playerModel.IsSoundOff)
            return;

        SwitchSound(false);
    }

    public void OnAdvShow()
    {
        if (playerModel.IsSoundOff)
            return;

        SwitchSound(true);
    }

    public void OnRewardedAdvClose(int rewardID)
    {
        if (playerModel.IsSoundOff)
            return;

        SwitchSound(false);
    }

    public void OnRewardedAdvShow(int rewardID)
    {
        if (playerModel.IsSoundOff)
            return;

        SwitchSound(true);
    }

    public void PlayBackground()
    {
        _backgroundSource.Play("background");
    }

    public void PlayMergeSound()
    {
        if (_mergeEffectSource.IsPlaying)
            return;

        _mergeEffectSource.Play("merge");
    }

    public void SwitchSound(bool isOff)
    {
        var volume = isOff ? 0 : 1;

        AudioListener.volume = volume;
        _mergeEffectSource.Volume = volume;
        _backgroundSource.Volume = volume;
    } 
}
