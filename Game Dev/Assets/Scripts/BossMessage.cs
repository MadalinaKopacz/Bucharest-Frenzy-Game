using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMessage : MonoBehaviour
{
    [SerializeField] private GameObject bossM;

    public void startBattle()
    {
        bossM.SetActive(false);
    }
}
