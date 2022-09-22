/*
    Author MrVastayan.
    Free to use and redistribute as you please, no credit required but always appreciated.
*/

using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace BlindsidedGames
{
    public class SaveSystem : MonoBehaviour
    {
        [SerializeField] private bool usePlayerPrefs;

        [Space(10)] private readonly string fileName = "SaveName";
        private readonly string fileExtension = ".fileType";
        private string _jsonData;

        [Space(10)]
        public SaveData saveData;


        private void Start()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            Load();
            InvokeRepeating(nameof(Save), 60, 60); // Auto-save once per minute.
        }
        
        private void OnApplicationQuit()
        {
            Save();
        }

        #region MobileAutoSave/Load

#if !UNITY_EDITOR
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
#if UNITY_IOS
            Load();
#elif UNITY_ANDROID
            Load();
#endif
        }
        if (!focus)
        {
            Save();
        }
    }
#endif

        #endregion
        
        #region SaveMethods

        public void Save() {
            var json = JsonUtility.ToJson(saveData); // Convert our data to a json string.
            if (!usePlayerPrefs) // If we aren't using PlayerPrefs, then save to the persistant data path.
            {
                File.WriteAllText(Application.persistentDataPath + "/" + fileName + fileExtension, json);
            }
            else
            {
                PlayerPrefs.SetString(fileName, json);
                PlayerPrefs.Save();
            }
        }

        public void Load()
        {
            // Create instances of our Data. If we load successfully these get overwritten.
            saveData = new SaveData();
            saveData.exampleSubClass = new ExampleSubClass();
            // Set Date started and ignore localization.
            saveData.dateStarted = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            if (!usePlayerPrefs)
            {
                // Check if a file exists and stop here if it doesn't.
                if (!File.Exists(Application.persistentDataPath + "/" + fileName + fileExtension))
                {
                    Debug.Log($"No SaveData found in:{Application.persistentDataPath}/");
                    return;
                }

                // Load the file to a string.
                _jsonData = File.ReadAllText(Application.persistentDataPath + "/" + fileName + fileExtension);
            }
            else
            {
                if (string.IsNullOrEmpty(PlayerPrefs.GetString(fileName)))
                {
                    Debug.Log("No SaveData found in PlayerPrefs");
                    return;
                }

                _jsonData = PlayerPrefs.GetString(fileName);
            }

            // Convert our data from json and pass it back to the class.
            saveData = JsonUtility.FromJson<SaveData>(_jsonData);
        }

        #endregion

        #region WipeMethods

        // Wipe SaveData by overriding it.
        [ContextMenu("WipeSaveData")]
        public void WipeSaveData()
        {
            saveData = new SaveData();
            saveData.exampleSubClass = new ExampleSubClass();
            saveData.dateStarted = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        }

        [ContextMenu("WipeExampleData")]
        public void WipeExampleData()
        {
            saveData.exampleSubClass = new ExampleSubClass();
        }

        #endregion
        
        #region DataContainers

        [Serializable]
        public class SaveData
        {
            public string dateStarted;
            public int exampleInteger;
            public ExampleSubClass exampleSubClass;
        }

        [Serializable]
        public class ExampleSubClass
        {
            public string someString;
            public int testInteger;
        }

        #endregion

        #region Singleton class: SaveSystem

        public static SaveSystem ss;
        private void Awake()
        {
            if (ss == null)
            {
                ss = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}