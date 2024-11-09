using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchChecker : MonoBehaviour
{
    [SerializeField] private GridSpawn gridSpawn;
    [SerializeField] private GameObject swordPrefab;
    private int minMatchCount = 3; // Số lượng tối thiểu để tạo match
    public int offsetX;
    public int offsetY;
    public Transform centerGrid; 

    // Class để lưu thông tin về một match tìm được
    public class MatchInfo 
    {
        public List<Cell> matchedCells = new List<Cell>();
        public FruitType fruitType;
    }


    public void Update()
    {
        
    }
    
    public List<MatchInfo> CheckMatches() 
    {
        List<MatchInfo> allMatches = new List<MatchInfo>();
        List<Cell> cells = gridSpawn.cellPositions;
        int rows = gridSpawn.rows;
        int columns = gridSpawn.columns;

        // Kiểm tra match theo hàng
        for (int row = 0; row < rows; row++) 
        {
            for (int col = 0; col < columns - 2; col++) 
            {
                int cellIndex = row * columns + col;
                MatchInfo horizontalMatch = CheckHorizontalMatch(cells, cellIndex, columns);
                if (horizontalMatch != null) 
                {
                    allMatches.Add(horizontalMatch);
                }
            }
        }

        // Kiểm tra match theo cột
        for (int row = 0; row < rows - 2; row++) 
        {
            for (int col = 0; col < columns; col++) 
            {
                int cellIndex = row * columns + col;
                MatchInfo verticalMatch = CheckVerticalMatch(cells, cellIndex, columns, rows);
                if (verticalMatch != null) 
                {
                    allMatches.Add(verticalMatch);
                }
            }
        }

        return allMatches;
    }

    private MatchInfo CheckHorizontalMatch(List<Cell> cells, int startIndex, int columns) 
    {
        Cell startCell = cells[startIndex];
        if (!startCell.fruitInside) return null;

        Cell startFruit = startCell.GetComponent<Cell>();
        if (startFruit == null) return null;

        List<Cell> matchedCells = new List<Cell> { startCell };
        FruitType fruitType = startFruit.fruitType;

        // Kiểm tra các ô bên phải
       for (int i = 1; i < columns - (startIndex % columns); i++) 
       { 
            Cell nextCell = cells[startIndex + i]; 
            if (!nextCell.fruitInside) break; 
            Cell nextFruit = nextCell.GetComponent<Cell>(); 
            if (nextFruit == null || nextFruit.fruitType != fruitType) break; 
            matchedCells.Add(nextCell); 
        }
     

        if (matchedCells.Count >= minMatchCount) 
        {
            
            return new MatchInfo 
            {
                matchedCells = matchedCells,
                fruitType = fruitType
            };
        }

        return null;
    }

    private MatchInfo CheckVerticalMatch(List<Cell> cells, int startIndex, int columns, int rows) 
    {
        Cell startCell = cells[startIndex];
        if (!startCell.fruitInside) return null;

        Cell startFruit = startCell.GetComponent<Cell>();
        if (startFruit == null) return null;

        List<Cell> matchedCells = new List<Cell> { startCell };
        FruitType fruitType = startFruit.fruitType;

        // Kiểm tra các ô phía dưới 
        for (int i = 1; i < rows - (startIndex / columns); i++) 
        { 
            int nextIndex = startIndex + (i * columns); 
            if (nextIndex >= cells.Count) break; 
            Cell nextCell = cells[nextIndex]; 
            if (!nextCell.fruitInside) break; 
            Cell nextFruit = nextCell.GetComponent<Cell>(); 
            if (nextFruit == null || nextFruit.fruitType != fruitType) break; 
            matchedCells.Add(nextCell); 
        }

        if (matchedCells.Count >= minMatchCount) 
        {
            return new MatchInfo 
            {
                matchedCells = matchedCells,
                fruitType = fruitType
            };
        }

        return null;
    }

    // Phương thức để kiểm tra match sau khi đặt một trái cây mới
    public bool CheckMatchAfterPlacement() 
    {
        List<MatchInfo> matches = CheckMatches();
        if (matches.Count > 0) 
        {
            // Xử lý các match tìm được
            foreach (var match in matches) 
            {
                for(int i = 0; i < match.matchedCells.Count;i++)
                {
                    match.matchedCells[i].RemoveFruit();         /// clear fruit
                }
                CreateSwordEffect(match);
            }
            return true;
        }
        return false;
    }




    private void CreateSwordEffect(MatchInfo match) 
    { 
        if (match.matchedCells.Count == 0) return; 
        // Xác định loại match (ngang hoặc dọc) 
        bool isHorizontal = (match.matchedCells[1].transform.position.y == match.matchedCells[0].transform.position.y); 
        Vector3 startPosition; 
        Vector3 endPosition; 
        Quaternion swordRotation;
        if (isHorizontal) 
        { 
            // Match ngang 
            startPosition = new Vector2(centerGrid.position.x-offsetX,match.matchedCells[0].transform.position.y); 
            endPosition = new Vector2(centerGrid.position.x+offsetX,match.matchedCells[0].transform.position.y); 
            swordRotation = Quaternion.Euler(0, 0, -90);
        } 
        else 
        {   
            // Match dọc 
            startPosition = new Vector2(match.matchedCells[0].transform.position.x,centerGrid.position.y + offsetY); 
            endPosition =  new Vector2(match.matchedCells[0].transform.position.x,centerGrid.position.y - offsetY); 
            // Xoay thanh kiếm theo hướng dọc 
            swordRotation = Quaternion.Euler(0, 0, 180);
        } 
        // Tạo thanh kiếm và đặt vị trí ban đầu 
        GameObject sword = Instantiate(swordPrefab, startPosition, Quaternion.identity); 
        // Di chuyển thanh kiếm từ vị trí bắt đầu đến vị trí kết thúc 
        sword.GetComponent<SwordE>().MoveSword(startPosition, endPosition); 
        sword.transform.rotation = swordRotation;
    }







}
