using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverCanvas : UICanvas
{
    // Start is called before the first frame update    public Text ScoreText;
    public GameManager gameManager;
    public Text ScoreText;
    public Canvas canvas;

    void OnEnable()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
         if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        } 
        UIManager.Instance.CloseUIDirectly<GamPlayCanvas>();
        canvas.sortingOrder = 1000;
    }
    void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }else
        {
            ScoreText.text = gameManager.score.ToString();
        }
    }
     public void HomeButton()
    {
        
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<LoadingCanvas>();
        SoundManager.Instance.PlayVFXSound(2);
    }

    public void RetryBtn()
    {
        StartCoroutine(ReLoad());
        SoundManager.Instance.PlayVFXSound(2);
        
    }
    IEnumerator ReLoad()
    {
        yield return new WaitForSeconds(0.2f);
        ReloadCurrentScene();
        UIManager.Instance.OpenUI<GamPlayCanvas>();
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.CloseUIDirectly<GameOverCanvas>();
        canvas.sortingOrder = -100;
    }
    
    public void ReloadCurrentScene()
    {
        // Lấy tên của scene hiện tại 
        string currentSceneName = SceneManager.GetActiveScene().name;
        //Tải lại scene hiện tại
        SceneManager.LoadScene(currentSceneName);
      
    }

    public void RecordBtn()
    {
        //UIManager.Instance.CloseUI<GameOverCanvas>(0.2f);
        UIManager.Instance.OpenUI<SavePointCanvas>();
        SoundManager.Instance.PlayVFXSound(2);
    }


}
