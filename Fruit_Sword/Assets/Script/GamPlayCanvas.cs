using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamPlayCanvas : UICanvas
{
    // Start is called before the first frame update
    public Sprite OnVolume;
    public Sprite OffVolume;

    [SerializeField] private Image buttonImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void RetryButton()
    {
        StartCoroutine(ReLoad());
    }
    
    public void HomeButton()
    {
        
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<LoadingCanvas>();
        
    }
    
    public void SoundButton()
    {
        SoundManager.Instance.TurnOn = !SoundManager.Instance.TurnOn;
        UpdateButtonImage();
    }

    IEnumerator ReLoad()
    {
        yield return new WaitForSeconds(0.3f);
        ReloadCurrentScene();
    }
    
    public void ReloadCurrentScene()
    {
        // Lấy tên của scene hiện tại 
        string currentSceneName = SceneManager.GetActiveScene().name;
        //Tải lại scene hiện tại
        SceneManager.LoadScene(currentSceneName);
    }
    private void UpdateButtonImage()
    {
        if (SoundManager.Instance.TurnOn)
        {
            buttonImage.sprite = OnVolume;
        }
        else
        {
            buttonImage.sprite = OffVolume;
        }
    }


}
