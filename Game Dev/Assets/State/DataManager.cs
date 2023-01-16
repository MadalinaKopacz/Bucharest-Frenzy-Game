using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }
    private GameData gameData;
    private List<IDataManager> dataManagerObjects;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of DataManager");
        }

        this.gameData = new GameData();
        instance = this;
    }

    private void Start()
    {
        dataManagerObjects = FindAllDataObjects();
        gameData.sceneIdx = SceneManager.GetActiveScene().buildIndex;
    }

    private List<IDataManager> FindAllDataObjects()
    {
        IEnumerable<IDataManager> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataManager>();
        return new List<IDataManager>(dataObjects);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SaveGame();
        } 

        if (Input.GetKey(KeyCode.L))
        {
            LoadGame();
        } 
    }

    public void LoadGame()
    {
        if (this.gameData == null)
        {
            Debug.LogError("No game data init.");
            NewGame();
        }
        else 
        {
            if (FileManager.LoadFromFile("GameSave.dat", out var json))
            {
                gameData.LoadFromJson(json);
                if (gameData.sceneIdx == 0 || gameData.sceneIdx == 1)
                {
                    // saved in menu
                    NewGame();
                }
                else
                {
                    SceneManager.LoadScene(gameData.sceneIdx);
                    // Make sure scene is loaded first
                    SceneManager.sceneLoaded += OnSceneLoaded;
                }
            } else {
                // could not load file
                // make new game
                NewGame();
            }
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameData.sceneIdx == scene.buildIndex)
        {
            dataManagerObjects = FindAllDataObjects();
            foreach (IDataManager dataObject in dataManagerObjects)
            {
                dataObject.LoadData(gameData);
            }
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SaveGame()
    {
        gameData.sceneIdx = SceneManager.GetActiveScene().buildIndex;

        foreach (IDataManager dataObject in dataManagerObjects)
        {
            dataObject.SaveData(ref gameData);
        }

        FileManager.WriteToFile("GameSave.dat", gameData.ToJson());
    }

    public void NewGame()
    {
        SceneManager.LoadScene(2); // load first scene
        this.gameData = new GameData();
        foreach (IDataManager dataObject in dataManagerObjects)
        {
            dataObject.LoadData(gameData);
        }
        SaveGame();
    }

}