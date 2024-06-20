using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBar;

    private int keysCollected;
    private int maxHealth;
    private int currentHealth;

    // Метод для инициализации значений
    public void Initialize(int maxHealth, int initialHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = initialHealth;
        UpdateHealthUI();
        UpdateKeysUI();
    }

    
    public void UpdateKeys(int keys)
    {
        keysCollected = keys;
        UpdateKeysUI();
    }

    
    public void UpdateHealth(int health)
    {
        currentHealth = health;
        UpdateHealthUI();
    }

    
    private void UpdateKeysUI()
    {
        keysText.text = "Keys: " + keysCollected + "/5";
    }

    // Внутренний метод для обновления UI здоровья
    private void UpdateHealthUI()
    {
        healthText.text = "Health: " + currentHealth;
        if (currentHealth> maxHealth)
        {
            maxHealth= currentHealth;
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
        else
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }        
    }
}
