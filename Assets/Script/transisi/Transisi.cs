using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transisi : MonoBehaviour
{
    public GameObject fadeOutPanel;
    public GameObject fadeInPanel;

    public static Transisi Instance;

    public void Start()
    {
        Instance = this;
        fadeOutPanel.SetActive(false);
        StartCoroutine(FadeInCoroutine());
    }

    public void FadeOut()
    {
        fadeOutPanel.SetActive(true);
    }

    IEnumerator FadeInCoroutine()
    {
        fadeInPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        fadeInPanel.SetActive(false);
    }

}
