using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnLocations;
    public int enemyCount;
    public int level = 1;

    private void Start()
    {
        spawnEnemyWave(level);
    }
    private void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0){
            level++;
            spawnEnemyWave(level);
        }
    }

    private void spawnEnemyWave(int enemiesToSpawn) {
        for (int i = 0; i < enemiesToSpawn; i++ ){
            // Enemy spawns at randomly spawn locations
            Instantiate(enemyPrefab, spawnLocations[Random.Range(0,spawnLocations.Length)].transform.position, enemyPrefab.transform.rotation) ;
        }
        
    }

}