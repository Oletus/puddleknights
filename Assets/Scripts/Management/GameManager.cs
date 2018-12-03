using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPUnityUtils;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<string> levelScenes;

    private int currentLevelIndex = 0;
    private int MaxLevelReached = 0;

    [System.NonSerialized] public Level Level;

    [System.NonSerialized] public Camera Camera;

    [SerializeField] private GameObject TitleUI;

    private GameObject NextLevelButton;
    private GameObject PrevLevelButton;

    void Awake()
    {
        if (!EnforceSingleton(false))
        {
            return;
        }

        for (int i = 0; i < levelScenes.Count; ++i )
        {
            if ( levelScenes[i] == SceneManager.GetActiveScene().name )
            {
                currentLevelIndex = i;
            }
        }

        TitleUI.SetActive(true);

        LoadPlayerData();
        MaxLevelReached = Mathf.Max(MaxLevelReached, currentLevelIndex);
    }

    private void UpdatePrevAndNextLevelButtons()
    {
        if ( NextLevelButton == null )
        {
            NextLevelButton = GameObject.Find("NextButton");
            PrevLevelButton = GameObject.Find("PreviusButton");
        }
        NextLevelButton.SetActive(this.MaxLevelReached > currentLevelIndex);
        PrevLevelButton.SetActive(currentLevelIndex > 0);
    }

    private void Start()
    {
        AudioManager.instance.Play("thememusic");
        UpdatePrevAndNextLevelButtons();
    }

    void Update()
    {
        Camera = Camera.main;
    }

    void LoadLevelFromScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
        MaxLevelReached = Mathf.Max(MaxLevelReached, currentLevelIndex);
        SavePlayerData();
        UpdatePrevAndNextLevelButtons();
    }

    public void PreviousLevel()
    {
        if ( currentLevelIndex == 0 )
        {
            return;
        }
        --currentLevelIndex;
        LoadLevelFromScene(levelScenes[currentLevelIndex]);
    }

    public void NextLevel()
    {
        ++currentLevelIndex;
        WrapIndex.Wrap(ref currentLevelIndex, levelScenes);
        LoadLevelFromScene(levelScenes[currentLevelIndex]);
    }

    private IEnumerator NextLevelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NextLevel();
    }

    public void OnLevelWin()
    {
        StartCoroutine(NextLevelAfter(1.0f));
    }

    public void SavePlayerData()
    {
        PlayerPrefs.SetInt("maxLevelReached", MaxLevelReached);
        PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {
        MaxLevelReached = PlayerPrefs.GetInt("maxLevelReached", 0);
    }
}
