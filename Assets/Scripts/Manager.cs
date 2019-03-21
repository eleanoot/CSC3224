using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private float DEFAULT_TIME = 90f;
    public static Manager instance = null;
    private static GameObject gameOverImage;

    private static Text gameOverText;

    public float levelTimeInSeconds;
    public static Text timerText;
    public static GameObject restartButton;

    Coroutine timerCoroutine;
    private bool timerRunning;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
            

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        gameOverImage = GameObject.Find("GameOver");
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        restartButton = GameObject.Find("RestartButton");
        // Prevent the UI assignment being reset.
        restartButton.GetComponent<Button>().onClick.AddListener(instance.RestartGame);
        gameOverImage.SetActive(false);
        restartButton.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {
        timerCoroutine = StartCoroutine(levelTimer());
        timerRunning = true;
    }

    IEnumerator levelTimer()
    {
        float counter = levelTimeInSeconds;

        while (counter > 0)
        {
            counter -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(counter / 60F);
            int seconds = Mathf.FloorToInt(counter - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);


            timerText.text = niceTime;
            yield return null;
        }

        timerText.text = string.Format("{0:0}:{1:00}", 0, 0);
        GameOver(false);

    }

    public void ResetTimer(float newTime)
    {
        StopCoroutine(timerCoroutine);
        levelTimeInSeconds = newTime;
        timerCoroutine = StartCoroutine(levelTimer());
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    // Initialises the game for each room that's loaded. 
    public void InitGame()
    {
        
    }

    public void RestartGame()
    {
        // Clear all player stats and items. 
        Stats.Reset();
        // Unpause the game.
        Time.timeScale = 1;
        
        // Restart the scene.
        SceneManager.LoadScene("Runner");
        // Restart timer.
        ResetTimer(DEFAULT_TIME);

    }

    // Called when time is up or when the player is out of health.
    public void GameOver(bool isPlayerDead)
    {
        // Freeze the scene. 
        Time.timeScale = 0;
        gameOverImage.SetActive(true);
        restartButton.SetActive(true);
        if (isPlayerDead)
        {
            gameOverText.text = "Maybe next time...\nYou cleared " + (Stats.RoomCount - 1) + " rooms";
        }
        else
        {
            gameOverText.text = "That's far enough for now...\nYou cleared " + (Stats.RoomCount - 1) + " rooms";
        }
        
    }
}
