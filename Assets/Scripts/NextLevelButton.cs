using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    
    private void OnEnable()
    {
        //  Check Next Scene, kalau tidak ada sembunyikan button "Next"
        var currentScene = SceneManager.GetActiveScene();
        int currentLevel = int.Parse(currentScene.name.Split("Level ")[1]);
        int nextLevel = currentLevel + 1;
        
        var nextBuildIndex = SceneUtility.GetBuildIndexByScenePath("Level " + nextLevel);

        if (nextBuildIndex == -1)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void NextLevel()
    {
        var currentScene = SceneManager.GetActiveScene();
        int currentLevel = int.Parse(currentScene.name.Split("Level ")[1]);
        int nextLevel = currentLevel + 1;

        SceneManager.LoadScene("Level " + nextLevel);
    }

}
