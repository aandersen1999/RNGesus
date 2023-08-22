using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : Singleton<CanvasScript>
{
    public List<RectTransform> dicePositions;
    [SerializeField] private GameObject dicePositionGroup;
    public List<GameObject> dices;
    [SerializeField] GameObject diceGroup;
    public Sprite[] diceSprites;

    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        GameManager.Instance.canvas = this;
        pauseMenu.SetActive(false);

        //Get all available dices
        dices = new List<GameObject>();

        for (int i = 0; i < diceGroup.transform.childCount; i++)
        {
            GameObject currentDice = diceGroup.transform.GetChild(i).gameObject;

            if (currentDice.activeSelf)
            {
                dices.Add(currentDice);
            }
        }
        //Get all available positions
        dicePositions = new List<RectTransform>();

        for (int i = 0; i < dicePositionGroup.transform.childCount; i++)
        {
            RectTransform currentDicePosition = dicePositionGroup.transform.GetChild(i).GetComponent<RectTransform>();

            if (currentDicePosition.gameObject.activeSelf)
            {
                dicePositions.Add(currentDicePosition);
            }
        }
        //Make dices align to the center
        

        SortDices(null);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPause += OnGamePause;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPause -= OnGamePause;
    }

    private void OnGUI()
    {
        SortDices(null);
    }

    private void OnGamePause(bool pause)
    {
        pauseMenu.SetActive(pause);
    }

    public void SortDices(GameObject heldDice)
    {
        dices.Sort((left, right) => left.transform.position.x.CompareTo(right.transform.position.x));

        for (int i = 0; i < dices.Count; i++)
        {
            if (dices[i] != heldDice)
            {
                dices[i].GetComponent<DiceScript>().targetPosition = dicePositions[i].position.x;
            }
        }
    }

    public void SortDices()
    {
        dices.Sort((left, right) => left.transform.position.x.CompareTo(right.transform.position.x));

        for (int i = 0; i < dices.Count; i++)
        {
            dices[i].GetComponent<DiceScript>().targetPosition = dicePositions[i].position.x;
        }
    }

    public void OnPressPlay()
    {
        GameManager.Instance.StartGame();
    }

    public void OnRestartLevel()
    {
        StartCoroutine(GameManager.Instance.RestartLevel());
    }

    public void OnPressPause()
    {
        GameManager.Instance.PauseGame(true);
    }

    public void OnPressResume()
    {
        GameManager.Instance.PauseGame(false);
    }

    public void OnPressMenu()
    {
        StartCoroutine(GameManager.Instance.LoadLevel(0));
        GameManager.Instance.PauseGame(false);
    }
}