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



    void Awake()
    {
        InitializeFruits();
    }
    void Start()
    {
        ActivateRandomFruitAtFixedPosition();
    }
    public void Update(){

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
               
                // Add to management lists
                allFruits.Add(fruit);
                inactiveFruits.Add(fruit);
            }
        }
    }
    

    // Active ở vị trí cố định
    public GameObject ActivateRandomFruitAtFixedPosition()
    {
        if (inactiveFruits.Count == 0 || activeFruits.Count >= 63)
        {
            Debug.LogWarning("No inactive fruits available!");
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
            Debug.LogWarning("No inactive fruits available!");
            return ;
        }

        // Get random fruit from inactive list
        int rdIndex =  Random.Range(0,inactiveFruits.Count);
        Debug.Log("" + rdIndex + " " + randomIndex);
        GameObject selectedFruit = inactiveFruits[rdIndex];
        selectedFruit.GetComponent<Fruit>().Actived = false;
        // Calculate random position
        int randomIndexPositon = Random.Range(0,gridSpawn.emptyCells.Count);
        Vector2 randomPosition = gridSpawn.emptyCells[randomIndexPositon].gameObject.transform.position;

        // Set position and activate
        selectedFruit.transform.position = randomPosition;
        selectedFruit.SetActive(true);

        // Update lists
        inactiveFruits.RemoveAt(rdIndex);
        
        activeFruits.Add(selectedFruit);

    }

    // Deactivate a fruit
    public void DeactivateFruit(GameObject fruit)
    {
        if (activeFruits.Contains(fruit))
        {
            fruit.SetActive(false);
            activeFruits.Remove(fruit);
            inactiveFruits.Add(fruit);
        }
    }

    // Helper method to get counts
    public void GetPoolStatus()
    {
        Debug.Log($"Total fruits: {allFruits.Count}");
        Debug.Log($"Active fruits: {activeFruits.Count}");
        Debug.Log($"Inactive fruits: {inactiveFruits.Count}");
    }
}
