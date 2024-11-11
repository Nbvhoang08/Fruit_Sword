using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeCanvas : UICanvas
{
    // Start is called before the first frame update
    public  void PlayBtn()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<LoadingCanvas>();
        SoundManager.Instance.PlayVFXSound(2);

    }
    public void RankButton()
    {
        UIManager.Instance.CloseUIDirectly<HomeCanvas>();
        UIManager.Instance.OpenUI<LeaderBoardCanvas>();
        SoundManager.Instance.PlayVFXSound(2);
    }

    

}

