using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int requiredKeys = 5;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float loweringSpeed = 2f;
    [SerializeField] private Animator animatorLock;

    private bool isUnlocked = false;
    private bool isLowering = false;

    void Start()
    {
        animatorLock = GetComponent<Animator>();
    }


    public void TryOpenDoor(int keysCollected)
    {
        if (keysCollected >= requiredKeys)
        {
            isUnlocked = true;
            animatorLock.SetTrigger("Open");
        }
    }

    void Update()
    {
        if (isLowering)
        {
            float step = loweringSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, step);

            if (Vector3.Distance(transform.position, targetPosition.position) < 0.001f)
            {
                isLowering = false;
            }
        }
    }


    public void OnUnlockAnimationComplete()
    {
        isLowering = true;
    } 
}
