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

    void Update()
    {
        Camera = Camera.main;
    }

    void LoadLevelFromScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
        Camera = Camera.main;
    }

    void NextLevel()
    {
        ++currentLevelIndex;
        if (currentLevelIndex >= levelScenes.Count)
        {
            currentLevelIndex = 0;
        }
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
