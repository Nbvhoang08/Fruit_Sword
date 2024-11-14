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
    public bool Slash = true;
    // Class để lưu thông tin về một match tìm được
    public class MatchInfo 
    {
        public List<Cell> matchedCells = new List<Cell>();
        public FruitType fruitType;
    }
    public void Update()
    {
        if(CheckMatches().Count != 0 )
        {
            if(Slash)
            {
                Slash = false;
                CheckMatchAfterPlacement();
            }
            
        }else
        {
            Slash = true; // Đặt lại biến Slash khi không có match
        }
        
    }
    
    public List<MatchInfo> CheckMatches() 
    {
        List<MatchInfo> allMatches = new List<MatchInfo>();
        List<Cell> cells = gridSpawn.cellPositions;
        int rows = gridSpawn.rows;
        int columns = gridSpawn.columns;
        bool[,] processedCells = new bool[rows, columns]; // Đánh dấu các ô đã được xử lý

        // Kiểm tra match theo hàng
        for (int row = 0; row < rows; row++) 
        {
            for (int col = 0; col < columns - 2; col++) 
            {
                int cellIndex = row * columns + col;
                if (!processedCells[row, col]) 
                {
                    MatchInfo horizontalMatch = CheckHorizontalMatch(cells, cellIndex, columns);
                    if (horizontalMatch != null) 
                    {
                        // Đánh dấu các ô đã được match
                        foreach (var cell in horizontalMatch.matchedCells) 
                        {
                            int cellRow = cell.transform.GetSiblingIndex() / columns;
                            int cellCol = cell.transform.GetSiblingIndex() % columns;
                            processedCells[cellRow, cellCol] = true;
                        }
                        allMatches.Add(horizontalMatch);
                    }
                }
            }
        }
        
     

        // Reset processed cells cho việc kiểm tra theo cột
        processedCells = new bool[rows, columns];
        for (int row = 0; row < rows; row++) 
        {
            for (int col = 0; col < columns; col++) 
            {
                if (processedCells[row, col]) 
                {
                    Debug.Log($"Ô tại hàng {row}, cột {col} đã được đánh dấu.");
                }
            }
        }

        // Kiểm tra match theo cột
        for (int row = 0; row < rows - 2; row++) 
        {
            for (int col = 0; col < columns; col++) 
            {
                int cellIndex = row * columns + col;
                if (!processedCells[row, col]) 
                {
                    MatchInfo verticalMatch = CheckVerticalMatch(cells, cellIndex, columns, rows);
                    if (verticalMatch != null) 
                    {
                        // Đánh dấu các ô đã được match
                        foreach (var cell in verticalMatch.matchedCells) 
                        {
                            int cellRow = cell.transform.GetSiblingIndex() / columns;
                            int cellCol = cell.transform.GetSiblingIndex() % columns;
                            processedCells[cellRow, cellCol] = true;
                        }
                        allMatches.Add(verticalMatch);
                    }
                   
                }
            }
        }
    

        return allMatches;
    }

    private MatchInfo CheckHorizontalMatch(List<Cell> cells, int startIndex, int columns) 
    {
        Cell startCell = cells[startIndex];
        if (!startCell.fruitInside) return null ;

        FruitType fruitType = startCell.fruitType;
        List<Cell> matchedCells = new List<Cell> { startCell };
    
        // Tìm tất cả các ô khớp nhau theo hàng ngang
        for (int i = 1; i < columns - (startIndex % columns); i++) 
        {
            Cell nextCell = cells[startIndex + i];
            if (!nextCell.fruitInside || nextCell.fruitType != fruitType) break;
            matchedCells.Add(nextCell);
        }

        // Chỉ trả về nếu đủ điều kiện match
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

        FruitType fruitType = startCell.fruitType;
        List<Cell> matchedCells = new List<Cell> { startCell };

        // Tìm tất cả các ô khớp nhau theo cột dọc
        for (int i = 1; i < rows - (startIndex / columns); i++) 
        {
            int nextIndex = startIndex + (i * columns);
            if (nextIndex >= cells.Count) break;
        
            Cell nextCell = cells[nextIndex];
            if (!nextCell.fruitInside || nextCell.fruitType != fruitType) break;
            matchedCells.Add(nextCell);
        }

        // Chỉ trả về nếu đủ điều kiện match
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
            // Xử dụng for ngược để có thể xóa an toàn trong quá trình lặp
            for(int i = matches.Count - 1; i >= 0; i--)
            {
                var match = matches[i];
            
                // Clear fruits
                foreach(var cell in match.matchedCells)
                {
                    cell.RemoveFruit();
                }
            
                // Tạo hiệu ứng
                CreateSwordEffect(match);
                //StartCoroutine(Sword(match));
            
                // Xóa match khỏi danh sách
                matches.RemoveAt(i);
            }
            return true;
        }
        return false;
    }

    IEnumerator Sword(MatchInfo match)
    {
        yield return new WaitForSeconds(0.3f);
        CreateSwordEffect(match);

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
        sword.GetComponent<SwordE>().gameManager = GetComponent<GameManager>(); 
        sword.transform.rotation = swordRotation;
    }







}
