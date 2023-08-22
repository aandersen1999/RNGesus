using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Action<bool> OnPause;

    public int gameTurn;
    public int playerTurn;
    public bool gameStarted;
    public bool gamePaused = false;
    [SerializeField] private GameObject playerGroup;
    public GameObject PlayerGroup { set { playerGroup = value; } }
    [SerializeField] public CanvasScript canvas;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<PlayerScript> players = new();
    private Dictionary<PlayerColor, PlayerScript> playersByColor = new();
    [SerializeField] private List<int> diceValues = new();
    [SerializeField] private Color red, blue, green, pink;

    [SerializeField] private Tilemap map;
    public Tilemap Map { get { return map; } set { map = value; } }
    [SerializeField] private List<TileData> tileDatas;
    private Dictionary<TileBase, TileData> dataFromTiles;

    private bool someoneWon = false;

    [SerializeField] private List<SceneReference> levels;
    public int levelNumber = 0;
    public int deadPlayers = 0;

    public GameObject dummyPrefab;
    private GameObject camera;
    private bool shakingCamera;
    [SerializeField] private Animator transitionAnimator;
    public Animator TransitionAnimator { set { transitionAnimator = value; } }

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        if (setToDestroy)
            return;

        players.Capacity = 4;
        dataFromTiles = new();

        foreach (TileData tileData in tileDatas)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        transitionAnimator = GameObject.Find("TransitionCanvas").transform.GetChild(0).GetComponent<Animator>();
        transitionAnimator.CrossFade("FadeOut", 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartLevel());
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }

        if (shakingCamera)
        {
            if (camera == null)
            {
                camera = GameObject.Find("Main Camera"); //Replace that with something else
            }
            camera.transform.position = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), 1 + UnityEngine.Random.Range(-0.1f, 0.1f), -10);
        }

        if (transitionAnimator == null)
        {
            transitionAnimator = GameObject.Find("TransitionCanvas").transform.GetChild(0).GetComponent<Animator>();
        }
    }

    public void StartGame()
    {
        if (gameStarted)
            return;

        foreach (GameObject dice in canvas.dices)
        {
            diceValues.Add(dice.GetComponent<DiceScript>().diceValue);
        }

        gameStarted = true;

        PassTurn();
    }

    public IEnumerator RestartLevel()
    {
        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.33f);

        ResetValues();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetValues()
    {
        someoneWon = false;
        gameStarted = false;
        gameTurn = 0;
        playerTurn = 0;
        deadPlayers = 0;
        diceValues.Clear();
    }

    public void PassTurn()
    {
        if (deadPlayers == players.Count)
        {
            StartCoroutine(LoadNextLevel());
            return;
        }
        if (someoneWon)
            return;

        PlayerScript currentPlayer = players[playerTurn];
        if (gameTurn < diceValues.Count)
        {
            CanvasScript.Instance.dices[gameTurn].GetComponent<DiceScript>().diceAnimator.CrossFade("DiceAnimation", 0);
            StartCoroutine(currentPlayer.Timer(diceValues[gameTurn]));
        }
        else
            TickGameOver();

        gameTurn++;

        if (gameTurn < diceValues.Count)
        {
            do
            {
                playerTurn++;
                playerTurn %= players.Count;
            }
            while (!players[playerTurn].gameObject.activeSelf);
        }
    }

    public void SetPlayer(PlayerScript player)
    {
        playersByColor[player.Color] = player;
        switch (player.Color)
        {
            case PlayerColor.Red:
                players[0] = player;
                break;
            case PlayerColor.Blue:
                players[1] = player;
                break;
            case PlayerColor.Green:
                players[2] = player;
                break;
            case PlayerColor.Yellow:
                players[3] = player;
                break;
        }
    }

    public PlayerScript GetPlayer(PlayerColor color)
    {
        return playersByColor[color];
    }

    public void TickGameOver()
    {
        StartCoroutine(CameraShake());
        someoneWon = true;
    }

    public void SetPlayerGroup(GameObject playerGroup)
    {
        this.playerGroup = playerGroup;
        //Get all available players
        players = new List<PlayerScript>();

        for (int i = 0; i < this.playerGroup.transform.childCount; i++)
        {
            PlayerScript currentPlayer = this.playerGroup.transform.GetChild(i).GetComponent<PlayerScript>();

            if (currentPlayer.gameObject.activeSelf)
            {
                players.Add(currentPlayer);
            }
        }
    }

    public IEnumerator LoadNextLevel()
    {

        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.33f);

        ResetValues();
        if(levelNumber != levels.Count)
        {
            SceneManager.LoadScene(levels[levelNumber]);
            MusicManager.Instance.LoadNewSong(levelNumber);
            levelNumber++;
        }
            
        else
        {
            SceneManager.LoadScene(0);
            MusicManager.Instance.LoadNewSong(0);
            
        }
        
    }

    public IEnumerator LoadLevel(SceneReference scene)
    {
        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.33f);

        ResetValues();
        SceneManager.LoadScene(scene);
        //MusicManager.Instance.LoadNewSong(scene);
    }

    public IEnumerator LoadLevel(int sceneID)
    {
        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.33f);

        ResetValues();
        SceneManager.LoadScene(sceneID);
        MusicManager.Instance.LoadNewSong(0);
    }

    public IEnumerator LoadLevelOnNumber(int sceneID)
    {
        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.33f);

        ResetValues();
        levelNumber = sceneID + 1;
        SceneManager.LoadScene(levels[sceneID]);
        MusicManager.Instance.LoadNewSong(sceneID);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            if (!gamePaused)
            {
                Time.timeScale = 0;
                if(MusicManager.Instance != null)
                    MusicManager.Instance.PauseMusic();
                gamePaused = true;
                OnPause?.Invoke(gamePaused);
            }
        }
        else
        {
            if (gamePaused)
            {
                Time.timeScale = 1;
                if (MusicManager.Instance != null)
                    MusicManager.Instance.UnpauseMusic();
                gamePaused = false;
                OnPause?.Invoke(gamePaused);
            }
        }
        
    }


    public IEnumerator CameraShake()
    {
        shakingCamera = true;
        yield return new WaitForSeconds(0.3f);
        shakingCamera = false;
        camera.transform.position = new Vector3(0, 1, -10);
    }
}
