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

    void Awake()
    {
        if (!EnforceSingleton(false))
        {
            return;
        }

        bool currentSceneIsLevel = false;
        for (int i = 0; i < levelScenes.Count; ++i )
        {
            if ( levelScenes[i] == SceneManager.GetActiveScene().name )
            {
                currentLevelIndex = i;
                currentSceneIsLevel = true;
            }
        }

        if ( !currentSceneIsLevel )
        {
            LoadLevelFromScene(levelScenes[currentLevelIndex]);
        }
    }

    void LoadLevelFromScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
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
}
