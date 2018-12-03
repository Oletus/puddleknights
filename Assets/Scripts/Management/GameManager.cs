using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LPUnityUtils;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<string> levelScenes;

    private int currentLevelIndex = 0;

    [System.NonSerialized] public Level Level;

    [System.NonSerialized] public Camera Camera;

    [SerializeField] private GameObject TitleUI;

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
    }

    private void Start()
    {
        AudioManager.instance.Play("thememusic");
    }

    void Update()
    {
        Camera = Camera.main;
    }

    void LoadLevelFromScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
        Camera = Camera.main;
    }

    public void PreviousLevel(bool allowLastLevel)
    {
        --currentLevelIndex;
        WrapIndex.Wrap(ref currentLevelIndex, levelScenes);
        if ( !allowLastLevel && currentLevelIndex == levelScenes.Count - 1 )
        {
            currentLevelIndex = levelScenes.Count - 2;
        }
        LoadLevelFromScene(levelScenes[currentLevelIndex]);
    }

    public void NextLevel(bool allowLastLevel)
    {
        ++currentLevelIndex;
        WrapIndex.Wrap(ref currentLevelIndex, levelScenes);
        if (!allowLastLevel && currentLevelIndex == levelScenes.Count - 1)
        {
            currentLevelIndex = 0;
        }
        LoadLevelFromScene(levelScenes[currentLevelIndex]);
    }

    private IEnumerator NextLevelAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NextLevel(true);
    }

    public void OnLevelWin()
    {
        StartCoroutine(NextLevelAfter(1.0f));
    }
}
