using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public string id;
    
    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        id = System.Guid.NewGuid().ToString();
    }
}
