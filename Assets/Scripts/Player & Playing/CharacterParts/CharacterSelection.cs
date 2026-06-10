using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI stateText;

    [Header("Characters")]
    [SerializeField] private CharacterData[] characters;
    private int currentIndex;
    public int SelectedCharacter { get; private set; }

    public AudioClip characterSelectionSound;
    public AudioClip transitionSound;


    void Start()
    {
        panel.SetActive(true);

        currentIndex = 0;

        UpdateUI();
    }

    void Update()
    {
        if (GameManager.Instance.isGameStarted)
            return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeCharacter(1);
            AudioSource.PlayClipAtPoint(
                 transitionSound,
                 Camera.main.transform.position
             );
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeCharacter(-1);
            AudioSource.PlayClipAtPoint(
                 transitionSound,
                 Camera.main.transform.position
             );
        }

        if (
     Input.GetKeyDown(KeyCode.Return) ||
     Input.GetKeyDown(KeyCode.KeypadEnter)
 )
        {
            ConfirmSelection();
        }
    }

    void ChangeCharacter(int direction)
    {
        currentIndex += direction;

        if (currentIndex >= characters.Length)
            currentIndex = 0;

        if (currentIndex < 0)
            currentIndex = characters.Length - 1;

        UpdateUI();
    }

    void UpdateUI()
    {
        characterImage.sprite =
            characters[currentIndex].characterSprite;

        stateText.text =
            characters[currentIndex].isUnlocked
            ? "Opened"
            : "Locked";
    }

    void ConfirmSelection()
    {
        if (!characters[currentIndex].isUnlocked)
            return;

        SelectedCharacter =
            currentIndex;

        AudioSource.PlayClipAtPoint(
                  characterSelectionSound,
                  Camera.main.transform.position
              );

        panel.SetActive(false);

        GameManager.Instance.StartGame(currentIndex);
    }
    public void Unlock(int index)
    {
        characters[index].isUnlocked = true;
    }
    public void ResetSelection()
    {
        panel.SetActive(true);
        currentIndex = 0;
        UpdateUI();
    }
}