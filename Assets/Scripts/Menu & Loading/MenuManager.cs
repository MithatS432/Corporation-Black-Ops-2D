using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button charachtersButton;
    public Button howToPlayButton;
    public Button quitButton;
    public Button backButton;
    private bool isTransitioning = false;

    [Header("UI")]
    public TextMeshProUGUI titleText;
    public float glitchInterval = 2f;
    public float glitchDuration = 0.15f;
    public float moveDistance = 30f;
    private Vector3 originalTitlePos;
    private Color originalTitleColor;
    private string originalTitle;

    public Image fadePanel;

    [Header("Panels")]
    public GameObject charachtersPanel;
    public GameObject howToPlayPanel;

    [Header("Fade Settings")]
    [Tooltip("Duration of the fade in seconds")]
    public float fadeDuration = 0.8f;


    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        charachtersButton.onClick.AddListener(CharactersPanel);
        howToPlayButton.onClick.AddListener(HowToPlayPanel);
        quitButton.onClick.AddListener(ExitGame);
        backButton.onClick.AddListener(BackToMainMenu);

        backButton.gameObject.SetActive(false);
        fadePanel.gameObject.SetActive(false);

        CloseAllPanels();

        originalTitlePos = titleText.rectTransform.localPosition;
        originalTitleColor = titleText.color;
        originalTitle = titleText.text;

        StartCoroutine(HorrorTitleEffect());
    }

    private void StartGame()
    {
        if (isTransitioning)
            return;

        isTransitioning = true;

        SetButtonsInteractable(false);

        fadePanel.gameObject.SetActive(true);

        StartCoroutine(FadeOutAndLoadScene(
            SceneManager.GetActiveScene().buildIndex + 1));
    }

    private void CharactersPanel()
    {
        CloseAllPanels();
        backButton.gameObject.SetActive(true);
        charachtersPanel.SetActive(true);
    }

    private void HowToPlayPanel()
    {
        CloseAllPanels();
        backButton.gameObject.SetActive(true);
        howToPlayPanel.SetActive(true);
    }

    private void BackToMainMenu()
    {
        backButton.gameObject.SetActive(false);
        CloseAllPanels();
    }

    private void CloseAllPanels()
    {
        charachtersPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    private void ExitGame()
    {
        if (isTransitioning)
            return;

        isTransitioning = true;

        SetButtonsInteractable(false);

        fadePanel.gameObject.SetActive(true);

        StartCoroutine(FadeOutAndExit());
    }

    private void SetButtonsInteractable(bool state)
    {
        startButton.interactable = state;
        charachtersButton.interactable = state;
        howToPlayButton.interactable = state;
        quitButton.interactable = state;
        backButton.interactable = state;
    }


    #region Fade Coroutines
    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        Color color = fadePanel.color;
        color.a = 1f;
        fadePanel.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);

            fadePanel.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        fadePanel.color = new Color(0f, 0f, 0f, 0f);
    }
    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        float elapsedTime = 0f;

        Color color = fadePanel.color;
        color.a = 0f;
        fadePanel.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            fadePanel.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        SceneManager.LoadScene(sceneIndex);
    }
    private IEnumerator FadeOutAndExit()
    {
        float elapsedTime = 0f;

        Color color = fadePanel.color;
        color.a = 0f;
        fadePanel.color = color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        fadePanel.color = new Color(0f, 0f, 0f, 1f);

#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator HorrorTitleEffect()
    {
        string[] scaryMessages = { "LOOK BEHIND", "STAY AWAY", "DIE", "HELP ME", "IT'S HERE" };

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, glitchInterval + 2f));

            titleText.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));

            if (Random.value > 0.6f)
            {
                titleText.text = scaryMessages[Random.Range(0, scaryMessages.Length)];
                titleText.color = Color.red;
            }

            Vector3 randomOffset = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), 0f);
            titleText.rectTransform.localPosition = originalTitlePos + randomOffset;

            titleText.enabled = true;

            yield return new WaitForSeconds(glitchDuration);

            titleText.text = originalTitle;
            titleText.rectTransform.localPosition = originalTitlePos;
            titleText.color = originalTitleColor;
        }
    }
    #endregion
}