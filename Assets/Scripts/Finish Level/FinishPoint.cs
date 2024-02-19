using UnityEngine;
using UnityEngine.UI;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private Image[] starsForLvlComplete;
    [SerializeField] private Sprite goldenStarSprite;
    private int starsAquired;
    private PlayerController playerController;
    

    private void Update()
    {
        playerController = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            levelCompleteUI.SetActive(true);

            if (LevelMenu.currentLevel == LevelMenu.unlockedLevel)
            {
                LevelMenu.unlockedLevel ++;
                PlayerPrefs.SetInt("UnlockedLevels",LevelMenu.unlockedLevel);
            }

            starsAquired = CalculateFinalStars(playerController.cherries);

            // Update UI golden stars images for level completion
            for (int j = 0; j < starsForLvlComplete.Length; j++)
            {
                if (j < starsAquired)
                {
                    // Ensure the goldenStarsForLvlComplete array is not null
                    if (starsForLvlComplete[j] != null)
                    {
                        // Change the sprite of the Image component to goldenStarSprite
                        starsForLvlComplete[j].sprite = goldenStarSprite;
                    }
                    else
                    {
                        Debug.LogError("Golden star UI image for level completion is null at index " + j);
                    }
                }
            }

            if (starsAquired > PlayerPrefs.GetInt("stars" + LevelMenu.currentLevel.ToString(), 0))
            {
                PlayerPrefs.SetInt("stars" + LevelMenu.currentLevel.ToString(), starsAquired);
            }

        }
    }

    private int CalculateFinalStars(int count)
    {
        int finalStars;

        if (count >= 18 && count < 30) 
        {
            finalStars = 2;
        }
        else if (count == 30)
        {
            finalStars = 3;
        }
        else
        {
            finalStars = 1;
        }
        Debug.Log("CalculateFinalStars finalStars");
        Debug.Log(finalStars);
        return finalStars;
    }

}
