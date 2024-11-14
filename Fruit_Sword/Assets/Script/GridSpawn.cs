using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawn : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private GameObject prefabToSpawn;
     public int rows = 5;
    public int columns = 5;
    [SerializeField] private float spacing = 1f;
    [SerializeField] private bool centerGrid = true;

    [Header("Spawn Settings")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private Transform parent;

    public List<Cell> cellPositions = new List<Cell>();
    public  List<Cell> emptyCells = new List<Cell>();

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnGrid();
        }
    }

   public void SpawnGrid()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogError("No prefab assigned to spawn!");
            return;
        }

        // Clear existing objects if any
        ClearGrid();

        // Calculate grid dimensions
        float totalWidth = (columns - 1) * spacing;
        float totalHeight = (rows - 1) * spacing;

        // Calculate starting position
        Vector3 startPos = transform.position;
        if (centerGrid)
        {
            startPos.x -= totalWidth / 2;
            startPos.y -= totalHeight / 2;
        }

        // Spawn objects
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 spawnPosition = new Vector3(
                    startPos.x + col * spacing,
                    startPos.y + row * spacing,
                    startPos.z
                );

                GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                spawnedObject.name = "( " + col + " , " + row +" )";

                // Set parent if specified
                if (parent != null)
                    spawnedObject.transform.SetParent(parent);
                else
                    spawnedObject.transform.SetParent(transform);

                Cell cell = spawnedObject.GetComponent<Cell>();
                cellPositions.Add(cell);

                // Check if the cell is empty and add to emptyCells list
                
                if (cell != null && !cell.fruitInside)
                {
                    emptyCells.Add(cell);
                    cell.OnFruitStatusChanged += UpdateEmptyCells;
                }
            }
        }
    }

    public void ClearGrid()
    {
        // Remove all existing child objects
        Transform parentTransform = parent != null ? parent : transform;
        while (parentTransform.childCount > 0)
        {
            DestroyImmediate(parentTransform.GetChild(0).gameObject);
        }

        // Clear lists
        cellPositions.Clear();
        emptyCells.Clear();
    }

      // Method to update emptyCells list
    private void UpdateEmptyCells(Cell cell,bool isObjectInside)
    {
        
        if (!emptyCells.Contains(cell) && !isObjectInside)
        {
            emptyCells.Add(cell);
           

        }
        else if (isObjectInside && emptyCells.Contains(cell))
        {
            emptyCells.Remove(cell);
        }
    }









    // Helper method to update grid at runtime
    public void UpdateGridSettings(int newRows, int newColumns, float newSpacing)
    {
        rows = newRows;
        columns = newColumns;
        spacing = newSpacing;
        SpawnGrid();
    }

    // Editor gizmos to visualize grid
    private void OnDrawGizmos()
    {
        if (!enabled) return;

        float totalWidth = (columns - 1) * spacing;
        float totalHeight = (rows - 1) * spacing;

        Vector3 startPos = transform.position;
        if (centerGrid)
        {
            startPos.x -= totalWidth / 2;
            startPos.y -= totalHeight / 2;
        }

        Gizmos.color = Color.yellow;
        
        // Draw horizontal lines
        for (int row = 0; row < rows; row++)
        {
            Vector3 lineStart = new Vector3(startPos.x, startPos.y + row * spacing, startPos.z);
            Vector3 lineEnd = new Vector3(startPos.x + totalWidth, startPos.y + row * spacing, startPos.z);
            Gizmos.DrawLine(lineStart, lineEnd);
        }

        // Draw vertical lines
        for (int col = 0; col < columns; col++)
        {
            Vector3 lineStart = new Vector3(startPos.x + col * spacing, startPos.y, startPos.z);
            Vector3 lineEnd = new Vector3(startPos.x + col * spacing, startPos.y + totalHeight, startPos.z);
            Gizmos.DrawLine(lineStart, lineEnd);
        }
    }
}
