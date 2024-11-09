using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordE : MonoBehaviour
{
    public float speed = 5f; 
    public void MoveSword(Vector3 startPosition, Vector3 endPosition) 
    { 
        StartCoroutine(MoveSwordRoutine(startPosition, endPosition)); 
    } 
    private IEnumerator MoveSwordRoutine(Vector3 startPosition, Vector3 endPosition) 
    { 
        float elapsedTime = 0f; 
        float totalDistance = Vector3.Distance(startPosition, endPosition); 
        while (elapsedTime < totalDistance / speed) 
        { 
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime * speed) / totalDistance); 
            elapsedTime += Time.deltaTime; 
            yield return null; 
        } 
        // Đảm bảo thanh kiếm ở vị trí kết thúc 
        transform.position = endPosition; 
        // Bạn có thể thêm hiệu ứng phá hủy hoặc ẩn thanh kiếm sau khi nó di chuyển xong 
        Destroy(gameObject); 
        }
}
