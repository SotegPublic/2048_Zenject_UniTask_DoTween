public interface ISoundManager : IInitable
{
    public void PlayBackground();
    public void PlayMergeSound();
    public void SwitchSound(bool isOff);
}