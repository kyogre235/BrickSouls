using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
 public static GameManager instance;

    static public int score = 0;
    static public int lives = 5;
    public Brick[] blocks;
    public int blockCount = 0;
    public int enemyCount = 0;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public GameObject gameScreen;   
    public GameObject resetScreen;
    public GameObject victoryScreen;
    public bool isPaused = false;
    public bool isGameOver = false;
    public GameObject pauseMenuPanel;
    public GameObject padObject;
    public GameObject ballObject;
    

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        blocks = FindObjectsOfType<Brick>(false);
        blockCount = blocks.Length;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (pauseMenuPanel != null)
    {
        pauseMenuPanel.SetActive(false);
    }   
        if (resetScreen != null)
        {
            resetScreen.SetActive(false);
        }
        if (victoryScreen != null) 
        {
            victoryScreen.SetActive(false);
        }
    
    // Aseguramos que el tiempo esté corriendo (escala 1)
    Time.timeScale = 1f;
    isPaused = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
{
    if (scene.name == "Juego") 
    {
        
        gameScreen = GameObject.Find("KillMenu");   
        resetScreen = GameObject.Find("KillMenu");
        pauseMenuPanel = GameObject.Find("Pausa");
        victoryScreen = GameObject.Find("VictoryMenu");

        GameObject scoreObj = GameObject.Find("ScoreText");
        if (scoreObj != null) scoreText = scoreObj.GetComponent<TMP_Text>();

        GameObject livesObj = GameObject.Find("LivesText");
        if (livesObj != null) livesText = livesObj.GetComponent<TMP_Text>();

        
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (resetScreen != null) resetScreen.SetActive(false);
        if (gameScreen != null) gameScreen.SetActive(false);
        if (victoryScreen != null) victoryScreen.SetActive(false);
        
        
        blocks = FindObjectsOfType<Brick>(false);
        blockCount = blocks.Length;
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        GameObject resetBtnObj = GameObject.Find("Reset");
        if (resetBtnObj != null)
        {
            Button btn = resetBtnObj.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(ResetGame);
        }
    }
}

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Puntos: {score}";
        livesText.text = $"Vidas: {lives}";
        if (Input.GetKeyDown(KeyCode.P))
        {
            //ButtonManager.OnPauseClick();
            Debug.Log("pausa");
        }
    }
    

    public void BlockDestroy()
    {
        blockCount--;
        score += 100;
        CheckWinCondition();
    }

    public void EnemyDestroy()
    {
        enemyCount--;
        score += 200; 
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        // Solo ganas si ya no hay bloques Y ya no hay enemigos
        if(blockCount <= 0 && enemyCount <= 0)
        {
            Debug.Log("You win!");
            WinGame();
        }
    }

    public void WinGame()
    {
        // Desactivamos las pelotas para que no sigan rebotando
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
        
        // Activamos la pantalla de victoria en lugar de la de reinicio
        if (victoryScreen != null) 
        {
            victoryScreen.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void LoseLifes()
    {
        lives--;
        if(lives <= 0)
        {
            Debug.Log("Game Over!");
            EndGame();
        }
    }

    public void EndGame()
    {
        //GameObject.Find("Player").SetActive(false);
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            ball.SetActive(false);
        }
        resetScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResetGame()
    {
        score = 0;
        lives = 3;


        Time.timeScale = 1f; 

        // Recarga la escena activa actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false;

    }

    public void TogglePause()
{
    isPaused = !isPaused;

    if (isPaused)
    {
        Time.timeScale = 0f; // Congela el tiempo
        pauseMenuPanel.SetActive(true);
    }
    else
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        pauseMenuPanel.SetActive(false);
    }
}
}
