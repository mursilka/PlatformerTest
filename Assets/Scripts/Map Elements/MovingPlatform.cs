using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float speed = 2f;

    private Vector3 targetPosition;
    private bool movingToEndPoint;

    private void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
        targetPosition = endPoint.position;
        movingToEndPoint = true;
    }

    private void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        // Движение платформы к целевой позиции
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Проверка достижения целевой позиции
        if (transform.position == targetPosition)
        {
            // Смена целевой позиции
            if (movingToEndPoint)
            {
                targetPosition = startPoint.position;
            }
            else
            {
                targetPosition = endPoint.position;
            }
            movingToEndPoint = !movingToEndPoint;
        }
    }
}