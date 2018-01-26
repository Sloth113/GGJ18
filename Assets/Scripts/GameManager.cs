using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private enum State
    {
        Intro,
        Title,
        InGame,
        Pause,
        Settings,
        GameOver
    }
    //Singleton approach
    private static GameManager m_instance = null;
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gm = new GameObject();
                gm.AddComponent<GameManager>();
                m_instance = gm.GetComponent<GameManager>();
            }
            return m_instance;
        }
    }


    //private List<Tower> m_allTowers;
    


    void Awake()
    {
        if (GameManager.m_instance == null)
        {
            GameManager.m_instance = this;
        }
        else if (GameManager.m_instance != this)
        {
            Destroy(this.gameObject);
        }
    }
            // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
