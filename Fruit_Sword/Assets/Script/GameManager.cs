using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class FruitPrefabInfo
    {
        public GameObject prefab;
        public int amount;
        
    }

    [Header("Fruit Prefabs Setup")]
    [SerializeField] private FruitPrefabInfo[] fruitPrefabs;
    [SerializeField] private Transform fixedSpawnPosition;
    [SerializeField] private Vector3 randomSpawnAreaMin;
    [SerializeField] private Vector3 randomSpawnAreaMax;

    // Lists to manage all fruits
    private List<GameObject> allFruits = new List<GameObject>();
    [SerializeField] private List<GameObject> inactiveFruits = new List<GameObject>();
    [SerializeField] private List<GameObject> activeFruits = new List<GameObject>();

    [SerializeField] private GridSpawn gridSpawn;
    private int randomIndex;
    private MatchChecker checker;
    public int score;
    private bool isGameOver;


    void Awake()
    {
        InitializeFruits();
        score = 0;
        isGameOver = false;
    }
    void Start()
    {
        ActivateRandomFruitAtFixedPosition();
        if(checker == null)
        {
            checker =GetComponent<MatchChecker>();
        }
    }
    public void Update()
    {
        // Kiểm tra tất cả các trái cây trong danh sách activeFruits 
        for (int i = activeFruits.Count - 1; i >= 0; i--) 
        { 
            GameObject fruit = activeFruits[i]; 
            if (!fruit.activeSelf) 
            { 
                // Xóa khỏi danh sách activeFruits 
                activeFruits.RemoveAt(i); 
                // Thêm vào danh sách inactiveFruits 
                if (!inactiveFruits.Contains(fruit)) 
                {  
                    inactiveFruits.Add(fruit); 
                }
            } 
        } 
        // Đảm bảo tất cả các trái cây không hoạt động trong allFruits có trong inactiveFruits 
        foreach (var fruit in allFruits) 
        { 
            if (!fruit.activeSelf && !inactiveFruits.Contains(fruit)) 
            { 
                inactiveFruits.Add(fruit); 
            }
        }

 
        if (activeFruits.Count >= 64 && !isGameOver)
        {
            StartCoroutine(CheckGameOverCondition());
        }
    


    }

    private IEnumerator CheckGameOverCondition()
    {
        yield return new WaitForSeconds(1);

        if (activeFruits.Count >= 63)
        {
            isGameOver = true;
            UIManager.Instance.OpenUI<GameOverCanvas>();
        }
    }

    private void InitializeFruits()
    {
        // Create all fruit instances based on their amounts
        foreach (FruitPrefabInfo fruitInfo in fruitPrefabs)
        {
            for (int i = 0; i < fruitInfo.amount; i++)
            {
                GameObject fruit = Instantiate(fruitInfo.prefab);
                fruit.SetActive(false);
                fruit.name = fruit.name + " " + i;
                // Add to management lists
                allFruits.Add(fruit);
                inactiveFruits.Add(fruit);
            }
        }
    }


    // Active ở vị trí cố định
    public GameObject ActivateRandomFruitAtFixedPosition()
    {
        if (inactiveFruits.Count == 0 || activeFruits.Count >= 64)
        {
            return null;
        }

        // Get random fruit from inactive list
        randomIndex = Random.Range(0, inactiveFruits.Count);
        GameObject selectedFruit = inactiveFruits[randomIndex];
        selectedFruit.GetComponent<Fruit>().Actived = true;
        // Set position and activate
        selectedFruit.transform.position = transform.position;
        selectedFruit.SetActive(true);

        // Update lists
        inactiveFruits.RemoveAt(randomIndex);
        activeFruits.Add(selectedFruit);

        return selectedFruit;
    }

    // Active ở vị trí ngẫu nhiên
    public void ActivateRandomFruitAtRandomPosition()
    {
        if (inactiveFruits.Count == 0 || activeFruits.Count >= 63)
        {
            return ;
        }

        // Get random fruit from inactive list
        int rdIndex =  Random.Range(0,inactiveFruits.Count);
        GameObject selectedFruit = inactiveFruits[rdIndex];
        selectedFruit.GetComponent<Fruit>().Actived = false;
        // Calculate random position
        if (gridSpawn.emptyCells.Count == 0)
        {
            return ;
        }
        int randomIndexPositon = Random.Range(0,gridSpawn.emptyCells.Count-1);
        Vector2 randomPosition = gridSpawn.emptyCells[randomIndexPositon].gameObject.transform.position;

        // Set position and activate
        selectedFruit.transform.position = randomPosition;
        selectedFruit.SetActive(true);

        // Update lists
        inactiveFruits.RemoveAt(rdIndex);
        
        activeFruits.Add(selectedFruit);
        
    }
   
}
