using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogCutscene : MonoBehaviour
{
    [System.Serializable]
    public class DialogData
    {
        [TextArea(3, 5)]
        public string dialog;

        public Sprite background;

        [Header("Optional")]
        public GameObject[] objectsToEnable;

        public GameObject[] objectsToDisable;
    }

        [Header("UI")]
    public GameObject cutscenePanel;
    public Image backgroundImage;
    public TextMeshProUGUI dialogText;

    [Header("Fade")]
    public Image fadePanel;              // Image hitam fullscreen
    public float fadeDuration = 0.5f;    // Lama fade
    public float blackScreenDuration = 0.5f;

    [Header("Dialog")]
    public DialogData[] dialogs;

    [Header("After Cutscene")]
    public GameObject[] objectsToEnable;

    private int currentDialog = 0;
    private bool isTransition = false;

    void Start()
    {
        // Jika cutscene sudah pernah dimainkan
        if (PlayerPrefs.GetInt("IntroPlayed", 0) == 1)
        {
            SkipCutscene();
            return;
        }

        cutscenePanel.SetActive(true);

        // Pastikan fade transparan
        Color c = fadePanel.color;
        c.a = 0;
        fadePanel.color = c;

        ShowDialog();
    }

    void Update()
    {
        if (!cutscenePanel.activeSelf)
            return;

        if (isTransition)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(NextDialogRoutine());
        }
    }

    void ShowDialog()
    {
        DialogData data = dialogs[currentDialog];

        dialogText.text = data.dialog;
        backgroundImage.sprite = data.background;

        // Aktifkan object (opsional)
        if (data.objectsToEnable != null)
        {
            foreach (GameObject obj in data.objectsToEnable)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        // Nonaktifkan object (opsional)
        if (data.objectsToDisable != null)
        {
            foreach (GameObject obj in data.objectsToDisable)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    IEnumerator NextDialogRoutine()
    {
        isTransition = true;

        // Fade ke hitam
        yield return StartCoroutine(Fade(0f, 1f));

        // Tahan layar hitam
        yield return new WaitForSeconds(blackScreenDuration);

        currentDialog++;

        if (currentDialog >= dialogs.Length)
        {
            yield return StartCoroutine(EndCutsceneRoutine());
            yield break;
        }

        // Ganti dialog dan background
        ShowDialog();

        // Fade kembali
        yield return StartCoroutine(Fade(1f, 0f));

        isTransition = false;
    }

    IEnumerator EndCutsceneRoutine()
    {
        // Tahan hitam sebentar sebelum masuk gameplay
        yield return new WaitForSeconds(blackScreenDuration);

        PlayerPrefs.SetInt("IntroPlayed", 1);
        PlayerPrefs.Save();

        SkipCutscene();

        yield return StartCoroutine(Fade(1f, 0f));

        isTransition = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;

        Color color = fadePanel.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            fadePanel.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadePanel.color = color;
    }

    void SkipCutscene()
    {
        cutscenePanel.SetActive(false);

        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}