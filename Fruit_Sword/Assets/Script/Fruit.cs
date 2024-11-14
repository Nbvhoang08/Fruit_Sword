using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Fruit : MonoBehaviour
{
    // Start is called before the first frame update
    public int score;
    public bool canMove;
    public Vector2 targetPos;
    public bool Actived;
    public FruitType type ;
    [SerializeField] private float speed ;
    [SerializeField] public GameManager gameManager;
    
    public bool IsDespawn = false;
    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.FindObjectOfType<GameManager>();   
        }
    }
    void Start()
    {
        canMove = false;
    }
  
    void OnDisable()
    {
        //Actived = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Actived && !canMove)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            // Khoảng cách cho các hướng thẳng (trên, dưới, trái, phải  )
            float straightDistance = 0.6f;
            // Khoảng cách cho các hướng chéo (theo định lý Pytago)
            float diagonalDistance = 1f;

            // Định nghĩa 8 hướng
            Vector2[] directions = new Vector2[]
            {
                Vector2.up,            // Trên
                Vector2.down,          // Dưới
                Vector2.left,          // Trái
                Vector2.right,         // Phải
                new Vector2(1, 1).normalized,    // Phải trên
                new Vector2(1, -1).normalized,   // Phải dưới
                new Vector2(-1, 1).normalized,   // Trái trên
                new Vector2(-1, -1).normalized   // Trái dưới
            };

            bool hasFruit = false;
            RaycastHit2D[] hits;

            for (int i = 0; i < directions.Length; i++)
            {
                // Xác định khoảng cách dựa vào hướng (thẳng hay chéo)
                float distance = i < 4 ? straightDistance : diagonalDistance;
    
                // Thực hiện raycast theo hướng
                hits = Physics2D.RaycastAll(clickPosition, directions[i], distance);
    
                // Debug để vẽ các tia raycast
                Debug.DrawRay(clickPosition, directions[i] * distance, Color.red, 1f);
    
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && hit.collider.CompareTag("fruit"))
                    {
                        hasFruit = true;
                        break;
                    }
                }
    
                if (hasFruit) break;
            }
            if (!hasFruit)
            {
                Vector3 potentialTargetPos = clickPosition;
                potentialTargetPos.x = Mathf.Round(potentialTargetPos.x / 2) * 2;
                potentialTargetPos.y = Mathf.Round(potentialTargetPos.y / 2) * 2;

                // Kiểm tra giới hạn tọa độ x và y
                if (potentialTargetPos.x >= -12 && potentialTargetPos.x <= 4 && potentialTargetPos.y >= -6 && potentialTargetPos.y <= 6)
                {
                    targetPos = potentialTargetPos;
                    canMove = true;
                }
            }
        }
        if (canMove)
        {
            Move();
           
        }
    }

    public void Move()
    {   
        if(onTarget())
        {
            Actived = false ;
            canMove = false ;
            gameManager.ActivateRandomFruitAtFixedPosition();
            gameManager.ActivateRandomFruitAtRandomPosition();
            //StartCoroutine(RadomFruit());
            
            
        }else
        {
            transform.position = Vector2.Lerp(transform.position, targetPos,speed*Time.deltaTime);
        }
        
    }
    private bool onTarget()
    {
        return Vector2.Distance(transform.position, targetPos) < 0.01f;
    }

    IEnumerator RadomFruit(){
        yield return new WaitForSeconds(0.2f);
        gameManager.ActivateRandomFruitAtRandomPosition();
        
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Sword"))
        {
            if(IsDespawn)
            {
                IsDespawn = false ;
                gameObject.SetActive(false);
            }
            other.GetComponent<SwordE>().TakeScore(score);
            
        }
    }



}
