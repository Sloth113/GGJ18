using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private enum State
    {
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
    //Tower prefabs
    public GameObject m_gunTower;
    public GameObject m_aoeTower;
    public GameObject m_extTower;
    public GameObject m_spltTower;

    //Enemy prefabs
    public GameObject m_speedyEnemy;
    public GameObject m_normalEnemy;
    public GameObject m_tankEnemy;

    //Level info
    public PowerSource source;
    private GameObject m_powerSource;
    public List<Tower> towers;
    private bool powerDirty;     // Dirty flag for recalculating power supply
    private List<Wave> m_waveInfo;
    private Transform m_enemySpawn;

    //Input
    private GameObject m_selectedTower;
    private bool m_connect;

    //State and other 
    private Stack<State> m_crurentState;
    [Header("UI Elements")]
    [Tooltip("UI prefabs")]
    public GameObject m_titleMenuUI;
    public GameObject m_levelSelectUI;
    public GameObject m_pauseMenuUI;
    public GameObject m_inGameUI;
    public GameObject m_settingsUI;
    public GameObject m_overUI;
    public GameObject m_eventSystem;

    //Enemies in game stuff
    private List<GameObject> m_enemiesInPlay;
    private float m_spawnTimer = 0.0f;
    private bool m_roundStart = false;
    private int m_waveIndex = 0;
    private int m_spawnIndex = 0;
    private GameObject m_newestEnemy;


    void Awake()
    {
        if (GameManager.m_instance == null)
        {
            GameManager.m_instance = this;
            m_instance.m_crurentState = new Stack<State>();
            m_instance.towers = new List<Tower>();
            m_instance.m_waveInfo = new List<Wave>();
            m_enemiesInPlay = new List<GameObject>();

            DontDestroyOnLoad(this.gameObject);
        }
        else if (GameManager.m_instance != this)
        {
            Destroy(this.gameObject);
        }
    }
            // Use this for initialization
    void Start () {
        m_crurentState.Push(State.Title);

        //Disable/Set not to destroy
        m_pauseMenuUI.SetActive(false);
        m_inGameUI.SetActive(false);
        m_settingsUI.SetActive(false);
        m_levelSelectUI.SetActive(false);
        m_overUI.SetActive(false);
        m_titleMenuUI.SetActive(true);
        //Set menus not to destroy
        DontDestroyOnLoad(m_titleMenuUI);
        DontDestroyOnLoad(m_pauseMenuUI);
        DontDestroyOnLoad(m_inGameUI);
        DontDestroyOnLoad(m_settingsUI);
        DontDestroyOnLoad(m_levelSelectUI);
        DontDestroyOnLoad(m_overUI);
        DontDestroyOnLoad(m_eventSystem);
    }
	
	// Update is called once per frame
	void Update () {
		switch (m_crurentState.Peek()) {
            case State.Title:

                break;
            case State.InGame:
                if (Input.GetKeyDown(KeyCode.M))
                {
                    LevelToOver();
                }
                //SPAWNER
                if (!m_roundStart)
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_roundStart = true;
                        m_spawnIndex = 0;
                        m_waveIndex = 0;
                    }
                }
                else
                {
                    if (m_waveIndex < m_waveInfo.Count)
                    {
                        //Round started
                        if (m_spawnTimer > m_waveInfo[m_waveIndex].spawnTimer)
                        {
                            //Spawn the thing
                            m_spawnTimer = 0;
                            if (m_waveInfo[m_waveIndex].spawnOrder[m_spawnIndex] == Enemies.Normal)
                                m_newestEnemy = Instantiate<GameObject>(m_normalEnemy, m_enemySpawn.position, Quaternion.identity);
                            else if (m_waveInfo[m_waveIndex].spawnOrder[m_spawnIndex] == Enemies.Speedy)
                                m_newestEnemy = Instantiate<GameObject>(m_speedyEnemy, m_enemySpawn.position, Quaternion.identity);
                            else if (m_waveInfo[m_waveIndex].spawnOrder[m_spawnIndex] == Enemies.Tank)
                                m_newestEnemy = Instantiate<GameObject>(m_tankEnemy, m_enemySpawn.position, Quaternion.identity);

                            m_spawnIndex++;
                            m_enemiesInPlay.Add(m_newestEnemy);
                            m_newestEnemy.GetComponent<RobotNavigation>().endDestination = m_powerSource;
                            //put wave count in here 
                            if (m_spawnIndex >= m_waveInfo[m_waveIndex].spawnOrder.Count)
                            {
                                m_waveIndex++;
                                m_spawnIndex = 0;
                            }
                        }
                        else
                        {
                            //increase spawn timer
                            m_spawnTimer += Time.deltaTime;
                        }
                    }else if(m_enemiesInPlay.Count <= 0)
                    {
                        //All enemies dead and finished spawning
                        LevelToOver();
                    }

                    m_enemiesInPlay.RemoveAll(item => item == null);

                }
                
                break;
            case State.Pause:

                break;
            case State.Settings:

                break;
            case State.GameOver:

                break;
            default:
                break;       
        }

    }

    public void LoadLevel(GameObject powerSource, Transform enemySpawn, List<Wave> waveInfo)
    {
        //Set defaults 
        m_spawnIndex = 0;
        m_waveIndex = 0;
        m_spawnTimer = 0;
        m_enemiesInPlay.Clear();
        towers.Clear();
        m_roundStart = false;

        source = powerSource.GetComponent<PowerSource>();
        m_powerSource = powerSource;
        m_waveInfo = waveInfo;
        m_enemySpawn = enemySpawn;
    }

    //
    public void MenuToLevelSelect()
    {
        m_titleMenuUI.SetActive(false);
        m_levelSelectUI.SetActive(true);
    }
    public void LevelSelectToMenu()
    {
        m_titleMenuUI.SetActive(true);
        m_levelSelectUI.SetActive(false);
    }
    public void LevelSelectToInGame()
    {
        m_levelSelectUI.SetActive(false);
        m_crurentState.Push(State.InGame);
        //LOAD SCENE
        SceneManager.LoadScene(1);//level 1? 
    }
    public void LevelSelectToInGame(int level)
    {
        m_levelSelectUI.SetActive(false);
        m_inGameUI.SetActive(true);
        m_crurentState.Push(State.InGame);
        //LOAD SCENE
        SceneManager.LoadScene(level);//level 1? 
    }
    public void LevelToPause()
    {
        m_crurentState.Push(State.Pause);
        m_pauseMenuUI.SetActive(true);
        m_inGameUI.SetActive(false);
    }
    public void PauseToMenu()
    {
        m_pauseMenuUI.SetActive(false);
        m_titleMenuUI.SetActive(true);
        m_crurentState.Pop();//Pause
        m_crurentState.Pop();//Ingame
        SceneManager.LoadScene(0);
        
    }
    public void PauseToInGame()
    {
        m_inGameUI.SetActive(true);
        m_pauseMenuUI.SetActive(false);
        m_crurentState.Pop();//Pause
    }
    public void LevelToOver()
    {
        m_inGameUI.SetActive(false);
        m_overUI.SetActive(true);
        m_crurentState.Pop();//Ingame
        m_crurentState.Push(State.GameOver);
    }
    public void MenuToSettings()
    {
        m_pauseMenuUI.SetActive(false);
        m_titleMenuUI.SetActive(false);
        m_settingsUI.SetActive(true);
        m_crurentState.Push(State.Settings);
    }
    public void ExitSettings()
    {
        m_settingsUI.SetActive(false);
        m_crurentState.Pop();
        if(m_crurentState.Peek() == State.Pause)
        {
            m_pauseMenuUI.SetActive(true);
        }else if(m_crurentState.Peek() == State.Title)
        {
            m_titleMenuUI.SetActive(true);
        }
    }
    public void OverToMenu()
    {
        m_overUI.SetActive(false);
        m_titleMenuUI.SetActive(true);
        m_crurentState.Pop();//Over
        SceneManager.LoadScene(0);

    }
    public void MenuToQuit()
    {
        Application.Quit();
    }
    //Andrews power graph stuff
    public void CalculatePowerGraph()
    {
        foreach (Tower tower in towers)
        {
            tower.visited = false;
            tower.inStack = false;
        }
        source.CalculateChildren();
    }

    public void SupplyPower(float power)
    {
        foreach (Tower tower in towers)
        {
            tower.ResetPower();
        }
        source.AddPower(power);
    }

    public void SetDirtyPower()
    {
        powerDirty = true;
    }
}
