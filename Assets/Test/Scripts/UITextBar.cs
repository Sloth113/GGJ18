using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextBar : MonoBehaviour {

    [SerializeField] private Text powerCounterText;
    [SerializeField] private Text healthCounterText;

    private LevelLoadInfo levelInfo;
    private PowerSourceHealth health;


	// Use this for initialization
	void Start () {
        levelInfo = FindObjectOfType<LevelLoadInfo>().GetComponent<LevelLoadInfo>();
        health = FindObjectOfType<PowerSourceHealth>().GetComponent<PowerSourceHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if (levelInfo != null && powerCounterText != null)
            powerCounterText.text = "Power: " + levelInfo.maxPower.ToString();
        if (health != null && healthCounterText != null)
            healthCounterText.text = "Health: " + health.ShowCurrentHealth().ToString();
	}
}
