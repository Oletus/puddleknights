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

    public void PreviousLevel()
    {
        --currentLevelIndex;
        WrapIndex.Wrap(ref currentLevelIndex, levelScenes);
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
}
