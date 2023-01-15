using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public struct PlayerData {
    [SerializeField] public Vector3 position;
    [SerializeField] public int hp;
    [SerializeField] public int gold;
    [SerializeField] public int damagePerHit;
    [SerializeField] public float speed;
    [SerializeField] public float jumpSize;

    // Values for powerup 
    [SerializeField] public bool inPowerup;
    [SerializeField] public int restoreDamagePerHit;
    [SerializeField] public float restoreSpeed;
    [SerializeField] public float restoreJump;
    [SerializeField] public float timeLeft;
};


[System.Serializable]
public class GameData
{
    [SerializeField] public PlayerData playerData;
    [SerializeField] public int sceneIdx;
    [SerializeField] public List<string> PowerupManager; // list of all powerup ids
    [SerializeField] public List<string> CoinManager; // list of all coin ids
    [SerializeField] public List<string> RatIds;
    [SerializeField] public List<Vector3> RatPositions;

   
    public GameData()
    {
        PowerupManager = new List<string>();
        CoinManager = new List<string>();
        RatIds = new List<string>();
        RatPositions = new List<Vector3>();
        playerData = new PlayerData();
        sceneIdx = 0;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }

    public static GameObject FindGameObjectInScene(string name)
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.name == name)
            {
                return go;
            }
        }

        return null;
    }
}

