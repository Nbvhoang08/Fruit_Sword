using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool fruitInside;
    public FruitType fruitType = FruitType.none ;
    public delegate void FruitStatusChanged(Cell cell,bool isObjectInside);
    public event FruitStatusChanged OnFruitStatusChanged;

    private Vector2 cellPosition;
    public Cell cell;
    [SerializeField]private float timeInsideCell = 0f;
    [SerializeField] private bool isObjectInside = false;
    private const float MINIMUM_TIME = 0.5f;

    [SerializeField]private GameObject currentFruit;
    [SerializeField] private float stayDuration = 0.1f; // Thời gian tối thiểu fruit phải ở trong ô
    [SerializeField] private float stayTimer;

    private void Start()
    {
        fruitInside = false;
        cellPosition = transform.position;
        cell = transform.GetComponent<Cell>();
    }

    private void Update()
    {

        timeInsideCell = Mathf.Clamp(timeInsideCell, 0f, 1f);
        if (isObjectInside)
        {
            if(currentFruit != null)
            {
                fruitType = currentFruit.GetComponent<Fruit>().type;
            }
            timeInsideCell += Time.deltaTime;
            if (timeInsideCell >= MINIMUM_TIME && !fruitInside)
            {
                fruitInside = true;
            }
        }
        else
        {
            fruitType = FruitType.none;
            fruitInside = false;
        }
        SetFruitInside();
        // Kiểm tra nếu fruit hiện tại không còn active
        if (currentFruit != null && !currentFruit.activeInHierarchy)
        {
            isObjectInside = false;
            currentFruit = null;
            stayTimer = 0;
        }
    }

    public void SetFruitInside()
    {
        // if (fruitInside != value)
        // {
        //     fruitInside = value;
            
        // }
        OnFruitStatusChanged?.Invoke(cell, isObjectInside);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("fruit"))
        {
            // Reset timer khi có fruit mới vào
            stayTimer = 0;
            timeInsideCell = 0;
            if(currentFruit == null)
            {
                currentFruit = other.gameObject;
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Kiểm tra nếu không có fruit hoặc fruit hiện tại đã bị tắt
        if (currentFruit == null || !currentFruit.activeInHierarchy)
        {
            isObjectInside = false;
            currentFruit = null;
            return;
        }


        // Nếu có fruit và là fruit đang theo dõi
        if (other != null && other.CompareTag("fruit") && other.gameObject == currentFruit)
        {
            if(currentFruit == null)
            {
                currentFruit = other.gameObject;
            }
            if (other.gameObject.activeInHierarchy)
            {   
                
                stayTimer += Time.deltaTime;
                if (stayTimer >= stayDuration)
                {
                    isObjectInside = true;
                }
            }
        }
    }
    public void RemoveFruit()
    {
        if (currentFruit != null)
        {
            currentFruit.GetComponent<Fruit>().IsDespawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("fruit") && other.gameObject == currentFruit && !fruitInside)
        {
            isObjectInside = false;
            currentFruit = null;
            stayTimer = 0;
            timeInsideCell = 0;
        }
    }
}

