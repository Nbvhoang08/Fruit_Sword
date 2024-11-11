using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePointCanvas : UICanvas
{
    // Start is called before the first frame update
    public InputField playerNameInput;
    public GameManager gameManager;
    public Text notificationText; 

     [SerializeField] private List<PlayerData> playerDataList = new List<PlayerData>();

   void OnEnable()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
    }

    void Awake()
    {
        // Load existing player data from PlayerPrefs
        LoadPlayerDataList();
    }

    void LoadPlayerDataList()
    {
        string json = PlayerPrefs.GetString("PlayerDataList", "{}");
        PlayerDataListWrapper playerDataListWrapper = JsonUtility.FromJson<PlayerDataListWrapper>(json);

        if (playerDataListWrapper != null && playerDataListWrapper.playerDataList != null)
        {
            playerDataList = playerDataListWrapper.playerDataList;
   
        }
    }
    void Update()
    {
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
    public void OkeBtn()
    {
        string PlayerName = playerNameInput.text;
        SavePlayerData(PlayerName);
        SoundManager.Instance.PlayVFXSound(2);

    }
    void SavePlayerData(string playerName)
    {
       int playerScore = gameManager.score; // Assuming GameManager has a method to get the current score

        // Check if the player name already exists in the list
        foreach (PlayerData playerData in playerDataList)
        {
            if (playerData.playerName == playerName)
            {
                notificationText.text = "Player name already exists.";
                return;
            }
        }

        // Create a new PlayerData object
        PlayerData newPlayerData = new PlayerData(playerName, playerScore);

        // Add the new player data to the list
        playerDataList.Add(newPlayerData);

        // Convert the list to JSON and save it to PlayerPrefs
        string json = JsonUtility.ToJson(new PlayerDataListWrapper { playerDataList = playerDataList });
        PlayerPrefs.SetString("PlayerDataList", json);

        // Optionally, save the data immediately
        PlayerPrefs.Save();

        Debug.Log("Player data saved: " + playerName + " - " + playerScore);
        notificationText.text = "";
        playerNameInput.text = "";
        UIManager.Instance.CloseUI<SavePointCanvas>(0.3f);
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerScore;

    public PlayerData(string name, int score)
    {
            playerName = name;
            playerScore = score;
    }
}

[System.Serializable]
public class PlayerDataListWrapper
{
    public List<PlayerData> playerDataList;
}

