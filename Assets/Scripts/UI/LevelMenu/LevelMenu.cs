using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    //public Button[] buttons;//
    public LevelObject[] levelObjects;
    public static int currentLevel;
    public static int unlockedLevel;
    public Sprite goldenStarSprite;
    


    private void Awake()
    {
        LevelAndStarsUnlocked();
    }

    private void LevelAndStarsUnlocked()
    {
        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevels", 1);

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (unlockedLevel >= 1 + i)
            {
                levelObjects[i].levelButton.interactable = true;

                int stars = PlayerPrefs.GetInt("stars" + i.ToString(), 0);
                Debug.Log("stars");
                Debug.Log(stars);
                if (levelObjects[i].stars != null && levelObjects[i].stars.Length >= stars)
                {
                    for (int j = 0; j < stars; j++)
                    {
                        if (levelObjects[i].stars[j] != null)
                        {
                            levelObjects[i].stars[j].sprite = goldenStarSprite;
                        }
                    }

                }
            }
            else
            {
                levelObjects[i].levelButton.interactable = false;
            }
        }
    }

    public void OpenLevel(int levelId) 
    {
        currentLevel= levelId;
        string levelName = "Level " + currentLevel;
        SceneManager.LoadScene(levelName);
    }
}
