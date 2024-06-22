using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private GameUIManager uiManager;
    [SerializeField] private Player player;

    void Start()
    {
        // Подписываемся на события игрока
        player.OnHealthChanged += HandleHealthChanged;
        player.OnKeysChanged += HandleKeysChanged;
        player.OnPlayerDeath += HandlePlayerDeath;

        // Инициализируем UI здоровья и ключей
        uiManager.Initialize(player.Health, player.KeysCollected);
    }

    void HandleHealthChanged(int health)
    {
        // Обновляем UI здоровья
        uiManager.UpdateHealth(health);

        // Пример обработки смерти игрока (можно реализовать дополнительные действия)
        if (health <= 0)
        {
            HandlePlayerDeath();
        }
    }

    void HandleKeysChanged(int keys)
    {
        // Обновляем UI ключей
        uiManager.UpdateKeys(keys);
    }

    void HandlePlayerDeath()
    {
        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}