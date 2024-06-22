using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    private int maxHealth;
    private int currentHealth;

    // Метод для инициализации значений
    public void Initialize(int maxHealth, int keys)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateKeys(keys);
    }

    // Обновление UI для количества собранных ключей
    public void UpdateKeys(int keys)
    {
        keysText.text = "Keys: " + keys + "/5";
    }

    // Обновление UI для здоровья
    public void UpdateHealth(int health)
    {
        currentHealth = health;
        UpdateHealthUI();
    }

    // Внутренний метод для обновления UI здоровья
    private void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}