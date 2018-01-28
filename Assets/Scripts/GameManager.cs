using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    private enum State
    {
        Title,
        InGame,
        Pause,
        Settings,
        GameOver,
        Chicken
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
    public GameObject blast;

    //Enemy prefabs
    public GameObject m_speedyEnemy;
    public GameObject m_normalEnemy;
    public GameObject m_tankEnemy;

    //Level info
    public PowerSource source;
    private GameObject m_powerSource;
    public float power;
    public float maxPower;
    public List<Tower> towers;
    public List<AttackTower> attackTowers;
    private float chickenCountdown;
    private bool chicken;
    private bool powerDirty;     // Dirty flag for recalculating power supply
    private List<Wave> m_waveInfo;
    private Transform m_enemySpawn;
    [SerializeField] Vector3 towerPlacementOffset;
    public float placedTowerOpacity;
    public bool m_tutorial = true;

    //Input
    [SerializeField]private GameObject m_selectedTower;
    public GameObject selectedTower { get { return m_selectedTower; } }
    private bool m_connect;
    private bool m_deleteWait;

    //State and other 
    private Stack<State> m_crurentState;
    [Header("UI Elements")]
    [Tooltip("UI prefabs")]
    public GameObject m_titleMenuUI;
    public GameObject m_levelSelectUI;
    public GameObject m_pauseMenuUI;
    public GameObject m_inGameUI;
    public GameObject m_ingameInfo;
    public GameObject m_inGameStart;
    public GameObject m_settingsUI;
    public GameObject m_overUI;
    public GameObject m_overDead;
    public GameObject m_overWin;
    public GameObject m_eventSystem;
    

    //Enemies in game stuff
    private List<GameObject> m_enemiesInPlay;
    private float m_spawnTimer = 0.0f;
    private bool m_roundStart = false;
    private int m_waveIndex = 0;
    private int m_spawnIndex = 0;
    private bool m_building;
    private GameObject m_newestEnemy;
    public float currentPowerReq;


    void Awake()
    {
        if (GameManager.m_instance == null)
        {
            GameManager.m_instance = this;
            m_instance.m_crurentState = new Stack<State>();
            m_instance.towers = new List<Tower>();
            m_instance.m_waveInfo = new List<Wave>();
            m_enemiesInPlay = new List<GameObject>();
            m_selectedTower = null;
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
	
    //TODO before selecting new building to construct, check it can be afforded

	// Update is called once per frame
	void Update () {
        switch (m_crurentState.Peek()) {
            

            case State.Title:

                break;
            case State.InGame:

                if (m_deleteWait)
                {
                    m_selectedTower = null;
                    m_deleteWait = false;
                    m_ingameInfo.SetActive(false);
                }
                //SPAWNER
                if (m_roundStart)
                {
                    if (m_waveIndex < m_waveInfo.Count)
                    {
                        //Round started
                        if (m_spawnTimer > m_waveInfo[m_waveIndex].spawnTimer && m_spawnIndex < m_waveInfo[m_waveIndex].spawnOrder.Count)
                        {
                            //Spawn the thing
                            m_spawnTimer = 0;
                           // Debug.Log("WAVE: " + m_waveIndex + " SPAWN:" + m_spawnIndex);
                            if (m_waveInfo[m_waveIndex].spawnOrder[m_spawnIndex] == Enemies.Normal)
                                m_newestEnemy = Instantiate<GameObject>(m_normalEnemy, m_enemySpawn.position, Quaternion.identity);
                            else if (m_waveInfo[m_waveIndex].spawnOrder[m_spawnIndex] == Enemies.Speedy)
                                m_newestEnemy = Instantiate<GameObject>(m_speedyEnemy, m_enemySpawn.position, Quaternion.identity);
                            else if (m_waveInfo[m_waveIndex].spawnOrder[m_spawnIndex] == Enemies.Tank)
                                m_newestEnemy = Instantiate<GameObject>(m_tankEnemy, m_enemySpawn.position, Quaternion.identity);

                            m_spawnIndex++;
                            m_enemiesInPlay.Add(m_newestEnemy);
                            m_newestEnemy.GetComponent<RobotNavigation>().endDestination = m_powerSource;
                            
                        }
                        else
                        {
                            //increase spawn timer
                            m_spawnTimer += Time.deltaTime;
                        }
                        //put wave count in here 
                        if (m_spawnIndex >= m_waveInfo[m_waveIndex].spawnOrder.Count && m_spawnTimer >= m_waveInfo[m_waveIndex].waveCooldown)
                        {
                            m_waveIndex++;
                            m_spawnIndex = 0;
                        }
                    }
                    else if(m_enemiesInPlay.Count <= 0)
                    {
                        //All enemies dead and finished spawning
                        LevelToOver();

                        m_overDead.SetActive(false);
                        m_overWin.SetActive(true);
                    }

                    m_enemiesInPlay.RemoveAll(item => item == null);

                }
                // Building
                if(m_selectedTower != null)
                {
                    if (m_building)
                    {
                        m_selectedTower.transform.position = GetMouseToGroundPlanePoint() + towerPlacementOffset;
                        m_selectedTower.GetComponent<Tower>().rangeCircle.gameObject.SetActive(true);
                        
                        if (Input.GetMouseButtonDown(0))
                        {
                            Vector3 mousePos = Input.mousePosition;
                            Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
                            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                            RaycastHit hit;
                           // int layerMask = 1 << 8;
                           // layerMask = ~layerMask;
                            if (Physics.SphereCast(mouseRay, 5, out hit, Mathf.Infinity,1, QueryTriggerInteraction.Collide)) {
                              // if (Physics.SphereCast(mouseRay, 5, out hit)) { 
                                Debug.Log(hit.transform.name);
                                if (hit.transform.tag == "Ground")
                                {
                                    Vector3 castPoint = hit.point;
                                    
                                    castPoint.y += (m_selectedTower.GetComponent<Renderer>().bounds.size.y / 2);
                                    m_selectedTower.transform.position = castPoint;
                                    // Activate tower
                                    m_selectedTower.GetComponent<Tower>().enabled = true;
                                    // Add tower to list
                                    towers.Add(m_selectedTower.GetComponent<Tower>());
                                    // Check if tower is an attack tower and add to list of attack towers
                                    AttackTower newTower = m_selectedTower.GetComponent<AttackTower>();
                                    if (newTower != null)
                                        attackTowers.Add(newTower);
                                    // Subtract cost of building
                                    power = Mathf.Max(0, power - m_selectedTower.GetComponent<Tower>().cost);
                                    SetDirtyPower();
                                    m_selectedTower = null;
                                    m_building = false;
                                    m_ingameInfo.SetActive(false);
                                }
                                else
                                {
                                    Destroy(m_selectedTower.gameObject);
                                    m_selectedTower = null;
                                    m_building = false;
                                    m_ingameInfo.SetActive(false);
                                }
                            }
                            /*
                            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit trst;
                            if (Physics.SphereCast(mouseRay, 5, out trst)){
                                Debug.Log(trst.transform.tag);
                            }

                            RaycastHit[] hits = Physics.SphereCastAll(mouseRay, 5, Mathf.Infinity, LayerMask.NameToLayer("Tower"), QueryTriggerInteraction.UseGlobal);//Might need to change the 0 
                            bool collided = false;
                            Debug.Log(hits.Length);
                            foreach (RaycastHit hit in hits)
                            {
                                //Check for collisions
                                collided = true;
                            }
                            
                            if (!collided)
                            {
                                m_selectedTower = null;
                            }
                            else
                            {
                                Destroy(m_selectedTower.gameObject);
                                m_selectedTower = null;
                            }*/
                        }
                    }
                    else 
                    {
                        Tower l_selectedTower = m_selectedTower.GetComponent<Tower>();
                        Tower mouseoverTower = GetTowerUnderMouse();
                        //Set connections
                        if (Input.GetMouseButtonUp(0))
                        {
                            if (mouseoverTower == null)
                            {
                                // Unselect tower if clicking elsewhere on map
                                m_deleteWait = true;


                            } else
                            {
                                if(l_selectedTower is IConnector && mouseoverTower != l_selectedTower)
                                {
                                    // Try making connection
                                    if((l_selectedTower as IConnector).MakeConnection(mouseoverTower))
                                    {
                                        // If connection succeeds, unselect tower and make power dirty
                                        m_selectedTower = null;
                                        SetDirtyPower();
                                        m_ingameInfo.SetActive(false);
                                    }
                                }
                            }
                        }
                        
                    }
                } else if (Input.GetMouseButtonDown(0))
                {
                    // Select clicked tower
                    Tower newSelection = GetTowerUnderMouse();
                    if(newSelection != null)
                    {
                        m_selectedTower = newSelection.gameObject;
                        m_ingameInfo.SetActive(true);
                        Text info = m_ingameInfo.GetComponentInChildren<Text>();
                        info.text = m_selectedTower.GetComponent<Tower>().GetInfoText();
                    }
                   
                }
                if (((m_powerSource.GetComponent<PowerSourceHealth>().ShowCurrentHealth())) <= 0)
                {
                    LevelToOver();
                    //Display dead
                    m_powerSource.GetComponent<PowerSource>().DestroyTower();
                    m_overDead.SetActive(true);
                    m_overWin.SetActive(false);
                }

                // Update power graph if dirty
                if (powerDirty)
                {
                    UpdatePowerGraph();
                }
                
                break;
            case State.Pause:

                break;
            case State.Settings:

                break;
            case State.GameOver:

                break;
            case State.Chicken:
                if (chicken)
                {
                    chickenCountdown -= Time.deltaTime;

                    if(chickenCountdown <= 0)
                    {
                        chicken = false;
                        SceneManager.LoadScene("secret");
                    }
                }
                break;
            default:
                break;       
        }

    }

   

    public void LoadLevel(LevelLoadInfo level)
    {
        //Set defaults 
        m_spawnIndex = 0;
        m_waveIndex = 0;
        m_spawnTimer = 0;
        m_enemiesInPlay.Clear();
        towers.Clear();
        m_roundStart = false;

        source = level.powerSource.GetComponent<PowerSource>();
        towers.Add(source);
        m_powerSource = level.powerSource;
        maxPower = level.maxPower;
        power = maxPower;
        UpdatePowerGraph();
        m_waveInfo = level.waveInfo;
        m_enemySpawn = level.enemySpawn;
        m_inGameStart.SetActive(true);

        m_crurentState.Push(State.InGame);
        
    }

    //Menu travesal
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
        //LOAD SCENE
        if (level == 1)
            m_tutorial = true;
        else
            m_tutorial = false;
        SceneManager.LoadScene(level);//level 1? 
    }
    public void LevelToPause()
    {
        m_crurentState.Push(State.Pause);
        m_pauseMenuUI.SetActive(true);
        m_inGameUI.SetActive(false);
        Time.timeScale = 0;
    }
    public void PauseToMenu()
    {
        m_pauseMenuUI.SetActive(false);
        m_titleMenuUI.SetActive(true);
        m_crurentState.Pop();//Pause
        m_crurentState.Pop();//Ingame
        SceneManager.LoadScene(0);
        Time.timeScale = 1;

    }
    public void PauseToInGame()
    {
        Time.timeScale = 1;
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


    public void xyxxy(Tower grue)
    {
        m_inGameUI.SetActive(false);
        m_crurentState.Pop();
        m_crurentState.Push(State.Chicken);
        GameObject beam5 = Object.Instantiate(grue.particleBeam);
        beam5.GetComponent<ConnectionParticleBeam>().target = source;
        beam5.GetComponent<ConnectionParticleBeam>().parent = grue;
        beam5.GetComponent<ConnectionParticleBeam>().Orient();
        chickenCountdown = 5;
        chicken = true;
        foreach(Tower t in towers)
        {
            if (t.inStack)
            {
                Object.Instantiate(blast, t.beamEndpoint.transform);
            }
        }
        foreach(GameObject enemy in m_enemiesInPlay)
        {
            enemy.GetComponent<RobotNavigation>().OnHit(200);
        }
    }

    public void unxyxxy()
    {
        m_titleMenuUI.SetActive(true);
        m_crurentState.Pop();
        SceneManager.LoadScene(0);
    }

    //Tower build buttons
    public void ExtendTower()
    {
        float cost = m_extTower.GetComponent<Tower>().cost;
        if (m_selectedTower == null && cost <= power)
        {
            m_ingameInfo.SetActive(true);
            Text info = m_ingameInfo.GetComponentInChildren<Text>();
            info.text = "Build Cost : " + cost + " Running Cost: " + 0;
            m_selectedTower = Instantiate<GameObject>(m_extTower);
            m_selectedTower.GetComponent<Tower>().enabled = false;
            m_building = true;
        }
        else if(m_building)
        {
            m_ingameInfo.SetActive(false);
            Destroy(m_selectedTower.gameObject);
            m_selectedTower = null;
            m_building = false;
        }
    }
    public void SplitterTower()
    {
        float cost = m_spltTower.GetComponent<Tower>().cost;
        if (m_selectedTower == null && cost <= power)
        {
            m_ingameInfo.SetActive(true);
            Text info = m_ingameInfo.GetComponentInChildren<Text>();
            info.text = "Build Cost : " + cost + " Running Cost: " + 0;
            m_selectedTower = Instantiate<GameObject>(m_spltTower);
            m_selectedTower.GetComponent<Tower>().enabled = false;
            m_building = true;
        }
        else if(m_building)
        {
            m_ingameInfo.SetActive(false);
            Destroy(m_selectedTower.gameObject);
            m_selectedTower = null;
            m_building = false;
        }
    }
    public void AOETower()
    {
        float cost = m_aoeTower.GetComponent<Tower>().cost;
        if (m_selectedTower == null && cost <= power)
        {
            m_ingameInfo.SetActive(true);
            Text info = m_ingameInfo.GetComponentInChildren<Text>();
            info.text = "Build Cost : " + cost + " Running Cost: " + m_aoeTower.GetComponent<AttackTower>().minPower;
            m_selectedTower = Instantiate<GameObject>(m_aoeTower);
            m_selectedTower.GetComponent<Tower>().enabled = false;
            m_building = true;
        }
        else if (m_building)
        {
            m_ingameInfo.SetActive(false);
            Destroy(m_selectedTower.gameObject);
            m_selectedTower = null;
            m_building = false;
        }
    }
    public void GunTower()
    {
        float cost = m_gunTower.GetComponent<Tower>().cost;
        if (m_selectedTower == null && cost <= power)
        {
            m_ingameInfo.SetActive(true);
            Text info = m_ingameInfo.GetComponentInChildren<Text>();
            info.text = "Build Cost : " + cost + " Running Cost: " + m_gunTower.GetComponent<AttackTower>().minPower;
            m_selectedTower = Instantiate<GameObject>(m_gunTower);
            m_selectedTower.GetComponent<Tower>().enabled = false;
            m_building = true;
        }
        else if (m_building)
        {
            m_ingameInfo.SetActive(false);
            Destroy(m_selectedTower.gameObject);
            m_selectedTower = null;
            m_building = false;
        }
    }
    //
    public void DeleteTower()
    {
        Debug.Log(m_selectedTower);
        if (m_selectedTower != null && m_selectedTower != m_powerSource)
        {
            if (!m_building)
            {
                //Give code
                if(m_roundStart)
                power += m_selectedTower.GetComponent<Tower>().cost / 2;
                else
                    power += m_selectedTower.GetComponent<Tower>().cost;
                towers.Remove(m_selectedTower.GetComponent<Tower>());
                SetDirtyPower();
            } else
            {
                m_building = false;
            }
            m_ingameInfo.SetActive(false);
            foreach (ConnectionParticleBeam b in m_selectedTower.GetComponent<Tower>().beams)
            {
                Destroy(b.gameObject);
            }
            
            DestroyImmediate(m_selectedTower.gameObject);
        }
    }

    public void StartWaves()
    {
        //Check tutorial
        Debug.Log(m_tutorial + " " + currentPowerReq + " " + towers.Count);
        if (m_tutorial && currentPowerReq == 0)
        {
            //Decide what info to say
            if (towers.Count < 2)
            {
                m_ingameInfo.SetActive(true);
                m_ingameInfo.GetComponentInChildren<Text>().text = "Select an offensive tower below to start";
            }
            else
            {
                m_ingameInfo.SetActive(true);
                m_ingameInfo.GetComponentInChildren<Text>().text = "Click on towers to form connections";
            }

        }
        else
        {
            m_ingameInfo.SetActive(false);
            m_tutorial = false;
            m_inGameStart.SetActive(false);
            m_roundStart = true;
            m_spawnIndex = 0;
            m_waveIndex = 0;

        }
    }

    //Andrews power graph stuff

    public void UpdatePowerGraph()
    {
        CalculatePowerGraph();
        SupplyPower(power);
        powerDirty = false;
        currentPowerReq = m_powerSource.GetComponent<PowerSource>().GetPowerReq();
        attackTowers.RemoveAll(item => item == null);
    }

    public void CalculatePowerGraph()
    {
        foreach (Tower tower in towers)
        {
            tower.visited = false;
            tower.inStack = false;
            tower.children.Clear();
            // TODO if distribution tower (And they're used) calculate its connections
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

    private Vector3 GetMouseToGroundPlanePoint()
    {

        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);

        Plane groundYPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDist = 0;

        groundYPlane.Raycast(mouseRay, out rayDist);

        Vector3 castPoint = mouseRay.GetPoint(rayDist);
        return castPoint;
    }

    // Returns the tower under the mouse, or null if none found
    public Tower GetTowerUnderMouse()
    {
        Tower hitTower = null;
        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, 1 << 8, QueryTriggerInteraction.Ignore))
        {
            hitTower = hit.collider.gameObject.GetComponent<Tower>();
        }

        return hitTower;
    }

}
