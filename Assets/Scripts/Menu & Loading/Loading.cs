using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Header("UI")]
    public Image progressBar;
    public TextMeshProUGUI infoText;
    public Image fadeImage;

    [Header("Settings")]
    public float loadDuration = 3f;
    public float fadeDuration = 0.8f;

    [Header("Messages")]
    [TextArea(1, 3)]
    public string[] loadingMessages =
    {
        "Syncing violence parameters...",
        "You are being simulated.",
        "Do not trust the progress bar.",
        "Memory leak detected in consciousness layer.",
        "Loading unstable timeline...",
        "Everything is fine. Probably.",
        "This is not a tutorial.",
        "The system remembers you differently.",
        "Chicken status: uncertain.",
        "Compressing moral framework...",
        "WAIT.",
        "TOO LATE.",
        "INITIALIZING BEHAVIOR LOOP..."
    };

    private int targetScene = 2;
    private AsyncOperation asyncOp;

    public void SetTargetScene(int sceneIndex)
    {
        targetScene = sceneIndex;
    }

    void Start()
    {
        progressBar.fillAmount = 0f;
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f);

        infoText.text = GetRandomMessage();

        StartCoroutine(LoadRoutine());
    }

    string GetRandomMessage()
    {
        if (loadingMessages == null || loadingMessages.Length == 0)
            return "LOADING...";

        return loadingMessages[Random.Range(0, loadingMessages.Length)];
    }

    IEnumerator LoadRoutine()
    {
        asyncOp = SceneManager.LoadSceneAsync(targetScene);
        asyncOp.allowSceneActivation = false;

        float t = 0f;

        while (t < loadDuration)
        {
            t += Time.deltaTime;

            float progress = Mathf.Clamp01(t / loadDuration);
            progressBar.fillAmount = Mathf.SmoothStep(0f, 1f, progress);

            yield return null;
        }

        progressBar.fillAmount = 1f;
        infoText.text = "READY.";

        yield return new WaitForSeconds(0.3f);

        float fadeTime = 0f;

        while (fadeTime < fadeDuration)
        {
            fadeTime += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, fadeTime / fadeDuration);
            fadeImage.color = new Color(0f, 0f, 0f, alpha);

            yield return null;
        }

        fadeImage.color = new Color(0f, 0f, 0f, 1f);

        yield return new WaitForSeconds(0.2f);

        asyncOp.allowSceneActivation = true;
    }
}