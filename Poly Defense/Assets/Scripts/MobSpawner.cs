﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(Spawn());
        }
        
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            foreach (Transform t in spawnPoints)
            {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ServerEnemy"), t.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(5f);
        }
    }
}