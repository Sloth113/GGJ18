﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextBar : MonoBehaviour {

    /*[SerializeField] */private Text powerCounterText;
    /*[SerializeField] */private Text healthCounterText;

    private PowerSourceHealth health;


	// Use this for initialization
	void Start () {
        powerCounterText = GameObject.Find("PowerText").GetComponent<Text>();
        healthCounterText = GameObject.Find("HealthText").GetComponent<Text>();
        health = GameObject.FindObjectOfType<PowerSourceHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if (health == null)
        {
            //Debug.Log("health is null");
            health = GameObject.FindObjectOfType<PowerSourceHealth>();
        }


        if (powerCounterText != null)
        {
            if(GameManager.Instance.currentPowerReq > GameManager.Instance.power)
            {
                powerCounterText.color = Color.red;
            }
            else
            {
                powerCounterText.color = Color.green;
            }
            //powerCounterText.text = "" + GameManager.Instance.power.ToString() + "-" + GameManager.Instance.currentPowerReq.ToString();
            powerCounterText.text = "" +(GameManager.Instance.power-GameManager.Instance.currentPowerReq).ToString();
        }
        if (health != null && healthCounterText != null)
        {
            healthCounterText.text = "Health: " + health.ShowCurrentHealth().ToString();
            //Debug.Log("health stuff called");
        }
    }
}
