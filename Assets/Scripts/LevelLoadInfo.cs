using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Enemies
{
    Tank,
    Speedy,
    Normal
}
[System.Serializable]
public struct Wave
{
    public List<Enemies> spawnOrder;
    public float spawnTimer;
    public float waveCooldown;
}
public class LevelLoadInfo : MonoBehaviour {
    public List<Wave> waveInfo;
    public GameObject powerSource;
    public Transform enemySpawn;
    public float maxPower;
    // Use this for initialization
    void Start () {
        GameManager.Instance.LoadLevel(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
