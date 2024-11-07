using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardCanvas : UICanvas
{
    // Start is called before the first frame update
    public void BackButton()
    {
        UIManager.Instance.CloseUIDirectly<LeaderBoardCanvas>();
        UIManager.Instance.OpenUI<HomeCanvas>();    
    }
}
