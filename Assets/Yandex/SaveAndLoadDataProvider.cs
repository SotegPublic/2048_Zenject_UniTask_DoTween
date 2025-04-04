using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class SaveAndLoadDataProvider : ILoadDataProvider, ISaveProvider
{
#if UNITY_EDITOR
    private ILoadMediator _loadMediator;

    public SaveAndLoadDataProvider(ILoadMediator Mediator)
    {
        _loadMediator = Mediator;
    }
#endif

    [DllImport("__Internal")]
    private static extern void SaveExtern(string playerDate);

    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string playerDate);

    [DllImport("__Internal")]
    private static extern int HasKey(string key);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

    [DllImport("__Internal")]
    private static extern void LoadExtern();

    private const string LOCAL_KEY = "localSave";

    public void SaveGameExtern(string saveData)
    {
#if !UNITY_EDITOR
        SaveExtern(saveData);
#else
        SaveJson(saveData);
#endif
    }

    public void SaveGameLocal(string saveData)
    {
#if !UNITY_EDITOR
        SaveToLocalStorage(LOCAL_KEY, saveData);
#else
        SaveJson(saveData);
#endif
    }

    private void SaveJson(string saveData)
    {
        string path = Path.Combine(Application.dataPath, "Resources", "playerData.json");
        File.WriteAllText(path, saveData);
        //AssetDatabase.Refresh();
    }

    public bool TryLoadLocal(out string jsonStr)
    {
#if !UNITY_EDITOR
        if (HasKey(LOCAL_KEY) == 1)
        {
            jsonStr = LoadFromLocalStorage(LOCAL_KEY);
            return true;
        }
        else
        {
            jsonStr = null;
            return false;
        }
#else
        if(TryLoadFromFile(out var json))
        {
            jsonStr = json;
            return true;
        }

        jsonStr = null;
        return false;
#endif
    }

    private bool TryLoadFromFile(out string json)
    {
        string fileName = "playerData";
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);


        if (textAsset != null)
        {
            json = textAsset.text;
            return true;
        }
        else
        {
            json = null;
            return false;
        }
    }

    public void LoadFromCloud()
    {
#if !UNITY_EDITOR
        LoadExtern();
#else
        if(TryLoadFromFile(out var json))
        {
            _loadMediator.WhenJsonLoad(json);
        }
        else
        {
            _loadMediator.WhenJsonLoad("");
        }
#endif
    }
}
