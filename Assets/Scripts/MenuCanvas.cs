using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] GameObject mainTitleScreen;
    [SerializeField] GameObject levelSelectScreen;
    [SerializeField] private Animator transitionAnimator;

    private void Start()
    {
        if (GameManager.Instance != null && CanvasScript.Instance != null)
        {
            GameManager.Instance.gameObject.SetActive(false);
            CanvasScript.Instance.gameObject.SetActive(false);
        }
    }
    public IEnumerator LoadScene(string sceneName)
    {
        transitionAnimator.CrossFade("FadeIn", 0);
        yield return new WaitForSeconds(0.33f);

        SceneManager.LoadScene(sceneName);

        if (GameManager.Instance != null && CanvasScript.Instance != null)
        {
            GameManager.Instance.gameObject.SetActive(true);
            CanvasScript.Instance.gameObject.SetActive(true);
        }
    }

    public void LoadTitleScreen()
    {
        mainTitleScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
    }

    public void LoadLevelSelectScreen()
    {
        mainTitleScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void StartGame()
    {
        GameManager.Instance.levelNumber = 0;
        StartCoroutine(GameManager.Instance.LoadNextLevel());
    }

    public void PlayLavaTheme()
    {
        MusicManager.Instance.LoadNewSong(16);
    }
}
