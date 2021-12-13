using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] ScoreManager scoreManager;
    public static LevelController instance = null;
    int sceneIndex;
    int levelComplete;

    private void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        scoreManager.NumberScene = sceneIndex;                                 // Записываем в ScoreManager ind тек сцены
    }
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
      
        levelComplete = PlayerPrefs.GetInt("LevelComplete");
    }

    public void isEndGame()
    {
        if (sceneIndex == 5)
        {
            Invoke("LoadMainMenu", 1f);                                          //Временно !!! Переделать в корутину
        }
        else
        {
            if (levelComplete < sceneIndex)
                PlayerPrefs.SetInt("LevelComplete", sceneIndex);
            // Invoke("NextLevel", 1f);                                            //Временно !!! Переделать в корутину
            NextLevel();
        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene(sceneIndex + 1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
