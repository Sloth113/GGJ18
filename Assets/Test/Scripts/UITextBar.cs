using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextBar : MonoBehaviour {

    /*[SerializeField] */private Text powerCounterText;
    /*[SerializeField] */private Text healthCounterText;

    private LevelLoadInfo levelInfo;
    private PowerSourceHealth health;


	// Use this for initialization
	void Start () {
        powerCounterText = GameObject.Find("PowerText").GetComponent<Text>();
        healthCounterText = GameObject.Find("HealthText").GetComponent<Text>();
        levelInfo = GameObject.FindObjectOfType<LevelLoadInfo>();
        health = GameObject.FindObjectOfType<PowerSourceHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if (levelInfo == null)
        {
            Debug.Log("levelInfo is null");
            levelInfo = GameObject.FindObjectOfType<LevelLoadInfo>();
        }


        if (health == null)
        {
            Debug.Log("health is null");
            health = GameObject.FindObjectOfType<PowerSourceHealth>();
        }


        if (levelInfo != null && powerCounterText != null)
        {
            powerCounterText.text = "Power: " + levelInfo.maxPower.ToString();
            Debug.Log("Power Counter Text called");
        }
        if (health != null && healthCounterText != null)
            healthCounterText.text = "Health: " + health.ShowCurrentHealth().ToString();
	}
}
