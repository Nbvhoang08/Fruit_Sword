using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingCanvas : UICanvas
{
    public Canvas  canvas;
    private void OnEnable()
    {
        StartCoroutine(LoadGamePlayScene());
        canvas.sortingOrder = 1000;
    }
    private void OnDisable() 
    {
        canvas.sortingOrder = -100;
    }
    private IEnumerator LoadGamePlayScene()
    {
        yield return new WaitForSeconds(2f);

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "GamePlay")
        {
            SceneManager.LoadScene("Home");
            yield return null;
            yield return null;
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<HomeCanvas>();
        }
        else if (currentScene == "Home")
        {
            SceneManager.LoadScene("GamePlay");
            yield return null;
               
            yield return null;
            UIManager.Instance.CloseAll();
            UIManager.Instance.OpenUI<GamPlayCanvas>();
        }    
    }
}
