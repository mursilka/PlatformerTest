using UnityEngine;

public class PlayerInput : IPlayerInput
{
    public bool IsJump { get; private set; }
    public float Horizontal { get; private set; }

    public void CustomUpdate()
    {
        // Чтение горизонтального ввода (влево/вправо)
        Horizontal = Input.GetAxis("Horizontal");

        // Проверка на прыжок
        IsJump = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

    }
}