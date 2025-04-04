public interface ILoadDataProvider
{
    public bool TryLoadLocal(out string jsonStr);
    public void LoadFromCloud();
}
