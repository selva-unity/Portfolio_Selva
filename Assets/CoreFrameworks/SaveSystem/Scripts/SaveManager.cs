using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Tooltip("Sub-folder under persistentDataPath where saves are stored.")]
    public string folderName = "Saves";

    public string RootPath => Path.Combine(Application.persistentDataPath, folderName);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Directory.CreateDirectory(RootPath);
    }

    #region Static convenience wrappers (like ObjectPoolManager.Spawn/Recycle)

    public static void Save<T>(T data, string key = null)
        => Instance.SaveInternal(data, key);

    public static T Load<T>(string key = null)
        => Instance.LoadInternal<T>(key);

    public static bool TryLoad<T>(string key, out T data)
        => Instance.TryLoadInternal(key, out data);

    public static bool Exists<T>(string key = null)
        => Instance.ExistsInternal<T>(key);

    public static void Delete<T>(string key = null)
        => Instance.DeleteInternal<T>(key);

    public static string GetPath<T>(string key = null)
        => Instance.GetFilePath<T>(key);

    #endregion

    #region Instance implementation

    /// <summary>
    /// Save any serializable object as JSON.
    /// key:
    ///   - If null/empty -> uses typeof(T).Name as filename.
    ///   - Otherwise -> typeof(T).Name + "_" + key.
    /// </summary>
    public void SaveInternal<T>(T data, string key = null)
    {
        if (data == null)
        {
            Debug.LogWarning("SaveManager.Save: data is null");
            return;
        }

        string path = GetFilePath<T>(key);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // Wrapper solves JsonUtility requirement that root must be a class
            var wrapper = new Wrapper<T> { value = data };
            string json = JsonUtility.ToJson(wrapper, true); // prettyPrint for debugging

            File.WriteAllText(path, json);
#if UNITY_EDITOR
            Debug.Log($"[SaveManager] Saved {typeof(T).Name} to {path}");
#endif
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveManager.Save: Failed to save at {path}\n{e}");
        }
    }

    /// <summary>
    /// Load JSON into object of type T. Throws if file missing/corrupt.
    /// Prefer TryLoad in gameplay code.
    /// </summary>
    public T LoadInternal<T>(string key = null)
    {
        string path = GetFilePath<T>(key);

        if (!File.Exists(path))
            throw new FileNotFoundException($"SaveManager.Load: No file at {path}");

        string json = File.ReadAllText(path);
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.value;
    }

    /// <summary>
    /// Safe load. Returns false if file missing or invalid.
    /// </summary>
    public bool TryLoadInternal<T>(string key, out T data)
    {
        string path = GetFilePath<T>(key);

        if (!File.Exists(path))
        {
            data = default;
            return false;
        }

        try
        {
            string json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            data = wrapper.value;
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"SaveManager.TryLoad: Failed to load at {path}\n{e}");
            data = default;
            return false;
        }
    }

    public bool ExistsInternal<T>(string key = null)
    {
        return File.Exists(GetFilePath<T>(key));
    }

    public void DeleteInternal<T>(string key = null)
    {
        string path = GetFilePath<T>(key);
        if (File.Exists(path))
        {
            File.Delete(path);
#if UNITY_EDITOR
            Debug.Log($"[SaveManager] Deleted save at {path}");
#endif
        }
    }

    /// <summary>
    /// Build full path for this type + key.
    /// Example for PlayerData, key "slot1":
    ///   {persistentDataPath}/Saves/PlayerData_slot1.json
    /// </summary>
    public string GetFilePath<T>(string key = null)
    {
        string baseName = typeof(T).Name;
        if (!string.IsNullOrEmpty(key))
            baseName += "_" + key;

        return Path.Combine(RootPath, baseName + ".json");
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T value;
    }

    #endregion
}