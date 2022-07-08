using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    public static UIMenuManager Instance;

    [SerializeField] int menuScene = 1;
    [SerializeField] int sceneToStart = 2;
    [Space]
    //[Space]
    //[SerializeField] int musicToChangeTo = 0;
    [Space]
    [SerializeField] GameObject tint;
    [SerializeField] GameObject menuMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        Instance = this;
        LoadManager.Instance.LoadScene(menuScene, LoadSceneMode.Single);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !SystemManager.Instance.isPaused && LoadManager.Instance.currentScene != 1)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && SystemManager.Instance.isPaused && LoadManager.Instance.currentScene != 1)
        {
            UnPause();
        }
    }

    public void Pause()
    {
        SystemManager.Instance.isPaused = true;
        SystemManager.Instance.isCameraLocked = true;
        SystemManager.Instance.isMovementLocked = true;

        ShowPauseMenu();
    }

    public void UnPause()
    {
        SystemManager.Instance.isPaused = false;
        SystemManager.Instance.isCameraLocked = false;
        SystemManager.Instance.isMovementLocked = false;

        HidePauseMenu();
    }

    public void ShowMenu()
    {
        menuMenu.SetActive(true);
    }

    public void HideMenu()
    {
        menuMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        optionsMenu.SetActive(true);
        tint.SetActive(true);
    }

    public void HideOptionsMenu()
    {
        optionsMenu.SetActive(false);
        tint.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
        tint.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        tint.SetActive(false);
    }

    // public void PlayNewMusic()
    // {
    //     playMusic.FadeUp(fastFadeIn);
    //     playMusic.PlaySelectedMusic(musicToChangeTo);
    // }

    public void StartAndStopGame()
    {
        StartCoroutine(OnStartAndStopGame());
    }

    IEnumerator OnStartAndStopGame()
    {
        if (LoadManager.Instance.currentScene == 1)
        {
            StartCoroutine(LoadManager.Instance.FadeAndLoadScene(LoadManager.FadeDirection.In, sceneToStart, LoadSceneMode.Single));
            yield return new WaitForSeconds(1f);
            HideMenu();
        }
        else
        {
            StartCoroutine(LoadManager.Instance.FadeAndLoadScene(LoadManager.FadeDirection.In, menuScene, LoadSceneMode.Single));
            yield return new WaitForSeconds(1f);
            ShowMenu();
            HidePauseMenu();
            UnPause();
        }
    }

    public void Quit()
    {
        StartCoroutine(OnQuit());
    }

    IEnumerator OnQuit()
    {
        yield return LoadManager.Instance.Fade(LoadManager.FadeDirection.In);

#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
