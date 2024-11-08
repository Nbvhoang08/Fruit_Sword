using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
   public bool fruitInside;
    public delegate void FruitStatusChanged(Cell cell, bool isEmpty);
    public event FruitStatusChanged OnFruitStatusChanged;

    private Vector2 cellPosition;
    public Cell cell;
    private float timeInsideCell = 0f;
    [SerializeField] private bool isObjectInside = false;
    private const float MINIMUM_TIME = 0.5f;

    [SerializeField]private GameObject currentFruit;
    private float stayDuration = 0.1f; // Thời gian tối thiểu fruit phải ở trong ô
    private float stayTimer;

    private void Start()
    {
        fruitInside = false;
        cellPosition = transform.position;
        cell = transform.GetComponent<Cell>();
    }

    private void Update()
    {
        if (isObjectInside)
        {
            timeInsideCell += Time.deltaTime;
            if (timeInsideCell >= MINIMUM_TIME && !fruitInside)
            {
                SetFruitInside(true);
            }
        }
        else
        {
            SetFruitInside(false);
        }

        // Kiểm tra nếu fruit hiện tại không còn active
        if (currentFruit != null && !currentFruit.activeInHierarchy)
        {
            isObjectInside = false;
            currentFruit = null;
            stayTimer = 0;
            Debug.Log("Fruit became inactive");
        }
    }

    public void SetFruitInside(bool value)
    {
        if (fruitInside != value)
        {
            fruitInside = value;
            OnFruitStatusChanged?.Invoke(cell, !fruitInside);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("fruit"))
        {
            // Reset timer khi có fruit mới vào
            stayTimer = 0;
            currentFruit = other.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Kiểm tra nếu không có fruit hoặc fruit hiện tại đã bị tắt
        if (currentFruit == null || !currentFruit.activeInHierarchy)
        {
            isObjectInside = false;
            currentFruit = null;
            stayTimer = 0;
            Debug.Log("UnActive");
            return;
        }

        // Nếu có fruit và là fruit đang theo dõi
        if (other != null && other.CompareTag("fruit") && other.gameObject == currentFruit)
        {
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("fruit") && other.gameObject == currentFruit && !fruitInside)
        {
            isObjectInside = false;
            currentFruit = null;
            stayTimer = 0;
        }
    }
}

