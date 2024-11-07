using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScaler : MonoBehaviour
{
     private Camera mainCamera;
    private float defaultPlaneWidth = 10f;
    private float defaultPlaneHeight = 10f;
    public float scrollSpeed = 1f;
    private Renderer bgRenderer;
    
    [SerializeField]
    private float distanceMultiplier = 1f; // Đây là tham số n trong công thức 2n*(Width/2)

    void Start()
    {
        mainCamera = Camera.main;
        bgRenderer = GetComponent<Renderer>();

        if (mainCamera == null)
        {
            Debug.LogError("Missing main camera!");
            return;
        }

        ScalePlane();
        UpdatePosition();
    }

    void Update()
    {
        ScalePlane();
        UpdatePosition();
        bgRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0f);
    }

    void ScalePlane()
    {
        if (!mainCamera.orthographic)
        {
            mainCamera.orthographic = true;
        }

        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * mainCamera.aspect;

        float scaleX = screenWidth / defaultPlaneWidth;
        float scaleZ = screenHeight / defaultPlaneHeight;

        transform.localScale = new Vector3(scaleX, 1, scaleZ);
    }

    void UpdatePosition()
    {
        // Tính toán chiều rộng thực tế của plane sau khi scale
        float actualWidth = defaultPlaneWidth * transform.localScale.x;
        
        // Áp dụng công thức 2n*(Width/2)
        float distance = 2f * distanceMultiplier * (actualWidth / 2f);
        
        Vector3 newPosition = transform.position;
        newPosition.x = distance;
        transform.position = newPosition;
    }

    public void ForceUpdateScale()
    {
        ScalePlane();
        UpdatePosition();
    }

    public void SetDefaultSize(float width, float height)
    {
        defaultPlaneWidth = width;
        defaultPlaneHeight = height;
        ScalePlane();
        UpdatePosition();
    }

    // Thêm phương thức để điều chỉnh tham số n
    public void SetDistanceMultiplier(float n)
    {
        distanceMultiplier = n;
        UpdatePosition();
    }
}
