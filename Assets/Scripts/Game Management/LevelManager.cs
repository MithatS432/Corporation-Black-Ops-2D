using UnityEngine;
using System.Collections;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public int maxLevel = 10;

    public GameObject[] levels;
    private int remainingEnemies;

    public AudioClip levelClearSound;

    [Header("UI")]
    [SerializeField] private TMP_Text floorClearText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartLevel(currentLevel);
    }

    public void RegisterEnemies(int count)
    {
        remainingEnemies = count;
    }

    public void EnemyKilled()
    {
        remainingEnemies--;

        Debug.Log("Enemy killed! Remaining: " + remainingEnemies);

        if (remainingEnemies <= 0)
        {
            StartCoroutine(LevelCompleteRoutine());
        }
    }

    IEnumerator LevelCompleteRoutine()
    {
        AudioSource.PlayClipAtPoint(
             levelClearSound,
             Camera.main.transform.position
         );

        if (floorClearText != null)
            floorClearText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (floorClearText != null)
            floorClearText.gameObject.SetActive(false);

        NextLevel();
    }

    void NextLevel()
    {
        currentLevel++;

        if (currentLevel > maxLevel)
        {
            Debug.Log("Game Finished - Tüm level'lar tamamlandı!");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetToCharacterSelection();
            }
            return;
        }

        StartLevel(currentLevel);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.levelIndex = currentLevel;
            GameManager.Instance.CheckCharacterUnlock();
            GameManager.Instance.ResetToCharacterSelection();
        }
    }

    void StartLevel(int level)
    {
        for (int i = 0; i < levels.Length; i++)
            levels[i].SetActive(i == level - 1);

        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        RegisterEnemies(enemies.Length);

        Debug.Log("Enemy Count: " + enemies.Length);

        ResetCamera();
    }
    void ResetCamera()
    {
        if (Camera.main == null) return;

        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
    }
}