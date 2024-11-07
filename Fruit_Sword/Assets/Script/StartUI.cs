using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    // Start is called before the first frame update
    public CanvasScaler canvasScaler;
    public Canvas canvas;

    void Start()
    {
        UIManager.Instance.OpenUI<HomeCanvas>();
        canvasScaler = GetComponent<CanvasScaler>();
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        ConfigureCanvasScaler(SceneManager.GetActiveScene().name);

        // Đảm bảo chế độ render của Canvas luôn là ScreenSpace
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        if (canvas.worldCamera == null)
        {
            // Tìm Main Camera trong scene nếu renderCamera bị null
            canvas.worldCamera = Camera.main;
        }
    }

    void ConfigureCanvasScaler(string sceneName)
    {
        if (sceneName == "Home")
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1f; // Ưu tiên chiều cao
        }
        else if (sceneName == "GamePlay")
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
        else
        {
            Debug.LogWarning("Unknown Scene: " + sceneName + ". Using default CanvasScaler settings.");
        }
    }
}
