using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CharacterSelection characterSelection;

    public bool isGameStarted;
    public bool isGameOver;
    public int levelIndex = 1;

    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform spawnPoint;
    private GameObject spawnedPlayer;
    public int selectedCharacterIndex;

    [SerializeField] private TMP_Text pauseText;
    private bool isPaused;

    [SerializeField] private TMP_Text specialPowerText;

    [Header("Score & Combo System")]
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private GameObject killPointPrefab;

    private int totalScore = 0;
    private int comboCount = 0;
    private float comboTimer = 0f;
    [SerializeField] private float comboDuration = 3f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isGameOver = false;
        isPaused = false;
        isGameStarted = false;
        Time.timeScale = 1f;

        FindUIElements();

        ResetCombo();
        totalScore = 0;
        UpdateUI();
    }
    void FindUIElements()
    {
        GameObject pauseObj = GameObject.FindWithTag("PauseText");
        if (pauseObj != null)
        {
            pauseText = pauseObj.GetComponent<TMP_Text>();
            pauseText.gameObject.SetActive(false);
        }

        GameObject specialObj = GameObject.FindWithTag("SpecialPowerText");
        if (specialObj != null) specialPowerText = specialObj.GetComponent<TMP_Text>();

        GameObject pointObj = GameObject.FindWithTag("PointText");
        if (pointObj != null) pointText = pointObj.GetComponent<TMP_Text>();

        GameObject comboObj = GameObject.FindWithTag("ComboText");
        if (comboObj != null) comboText = comboObj.GetComponent<TMP_Text>();
    }
    void Update()
    {
        HandlePauseInput();

        if (!isGameStarted || isGameOver) return;

        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }
    void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseText == null)
        {
            GameObject pauseObj = GameObject.FindWithTag("PauseText");
            if (pauseObj != null)
                pauseText = pauseObj.GetComponent<TMP_Text>();
            else
                return;
        }

        if (pauseText != null)
            pauseText.gameObject.SetActive(isPaused);

        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }




    #region Player Spawning
    private void SpawnPlayer(int index)
    {
        if (spawnPoint == null)
        {
            GameObject sp = GameObject.Find("SpawnPoint");
            if (sp != null) spawnPoint = sp.transform;
        }

        spawnedPlayer = Instantiate(
            characterPrefabs[index],
            spawnPoint.position,
            Quaternion.identity
        );

        IInitializableAbility abilityWithUI = spawnedPlayer.GetComponent<IInitializableAbility>();

        if (abilityWithUI != null)
        {
            abilityWithUI.SetUI(specialPowerText);
        }
    }
    public void RespawnPlayer()
    {
        if (spawnedPlayer != null)
            Destroy(spawnedPlayer);

        SpawnPlayer(selectedCharacterIndex);
    }
    public Transform GetPlayer()
    {
        return spawnedPlayer != null ? spawnedPlayer.transform : null;
    }
    #endregion





    #region Audio Manager
    public void StartGame(int selectedCharacterIndex)
    {
        this.selectedCharacterIndex = selectedCharacterIndex;

        isGameStarted = true;

        SpawnPlayer(selectedCharacterIndex);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(levelIndex);
        }
    }
    public void LevelUp()
    {
        levelIndex++;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(levelIndex);
        }
    }
    #endregion


    #region  Score & Combo System
    public void EnemyKilled(Vector3 enemyPosition, int basePoint)
    {
        comboCount++;
        comboTimer = comboDuration;

        totalScore += basePoint;

        UpdateUI();

        SpawnKillPointText(enemyPosition, basePoint);
    }

    void ResetCombo()
    {
        comboCount = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (pointText != null)
            pointText.text = totalScore + "PTS";

        if (comboText != null)
            comboText.text = comboCount + "X";
    }

    void SpawnKillPointText(Vector3 spawnPos, int pointAmount)
    {
        if (killPointPrefab == null) return;

        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas bulunamadı!");
            return;
        }

        Vector2 screenPos = Camera.main.WorldToScreenPoint(spawnPos);

        GameObject textObj = Instantiate(killPointPrefab, canvas.transform);
        textObj.transform.position = screenPos;

        TMP_Text tmpText = textObj.GetComponent<TMP_Text>();
        if (tmpText != null)
            tmpText.text = "+" + pointAmount;

        Destroy(textObj, 1f);
    }
    #endregion

    #region Character Unlock System
    public void CheckCharacterUnlock()
    {
        if (levelIndex >= 4)
            UnlockCharacter(1);

        if (levelIndex >= 7)
            UnlockCharacter(2);
    }
    void UnlockCharacter(int index)
    {
        CharacterSelection selection =
            FindAnyObjectByType<CharacterSelection>(FindObjectsInactive.Include);

        if (selection != null)
        {
            selection.Unlock(index);
        }
    }
    public void ResetToCharacterSelection()
    {
        if (spawnedPlayer != null)
            Destroy(spawnedPlayer);

        isGameStarted = false;
        isGameOver = false;
        isPaused = false;
        totalScore = 0;
        ResetCombo();
        Time.timeScale = 1f;

        CharacterSelection charSelect =
            FindAnyObjectByType<CharacterSelection>(FindObjectsInactive.Include);
        if (charSelect != null)
        {
            charSelect.gameObject.SetActive(true);
            charSelect.ResetSelection();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    #endregion
}