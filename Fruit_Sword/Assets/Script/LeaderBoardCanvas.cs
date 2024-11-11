using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardCanvas : UICanvas
{
    // Start is called before the first frame update
    public GameObject InforPrefab;
    public Transform Parent;
    private void OnEnable()
    {
        showLeaderBoard();
    }
    
    public void BackButton()
    {
        UIManager.Instance.CloseUIDirectly<LeaderBoardCanvas>();
        UIManager.Instance.OpenUI<HomeCanvas>();    
        SoundManager.Instance.PlayVFXSound(2);
    }


    public void showLeaderBoard()
    {
        // Clear existing leaderboard entries
        foreach (Transform child in Parent)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("leaderBoard");
        // Load player data from PlayerPrefs
        string json = PlayerPrefs.GetString("PlayerDataList", "{}");
        PlayerDataListWrapper playerDataListWrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);

        if (playerDataListWrapper != null && playerDataListWrapper.playerDataList != null)
        {
            // Sort the player data list by score in descending order
            playerDataListWrapper.playerDataList.Sort((x, y) => y.playerScore.CompareTo(x.playerScore));

            // Display top 10 players
            for (int i = 0; i < Mathf.Min(10, playerDataListWrapper.playerDataList.Count); i++)
            {
                PlayerData playerData = playerDataListWrapper.playerDataList[i];

                // Instantiate a new InforPrefab
                GameObject newEntry = Instantiate(InforPrefab, Parent);

                // Set the text values for Rank, Name, and Score
                Text[] texts = newEntry.GetComponentsInChildren<Text>();
                texts[0].text = (i + 1).ToString(); // Rank
                texts[1].text = playerData.playerName; // Name
                texts[2].text = playerData.playerScore.ToString(); // Score
            }
        }
    
    }


    



}
