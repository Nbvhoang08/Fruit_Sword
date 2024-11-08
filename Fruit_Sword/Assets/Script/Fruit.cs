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
    [SerializeField] private float speed ;
    [SerializeField] public GameManager gameManager;
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
    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        Actived = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Actived && !canMove)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(clickPosition);

            if (hitCollider == null || !hitCollider.CompareTag("fruit"))
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
            StartCoroutine(RadomFruit());
            
            
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



}
