using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject restartTextObj;

    private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;
    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;

        restartTextObj = GameObject.FindWithTag("DeathText");

        if (restartTextObj != null)
            restartTextObj.SetActive(false);
    }
    private void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void GetDamage()
    {
        if (isDead) return;

        currentHealth--;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int amount)
    {
        if (currentHealth >= 2 || isDead)
            return;

        currentHealth += amount;
    }
    private void Die()
    {
        isDead = true;

        if (GameManager.Instance != null)
            GameManager.Instance.isGameOver = true;

        if (restartTextObj != null)
            restartTextObj.SetActive(true);

        Time.timeScale = 0f;
    }
}