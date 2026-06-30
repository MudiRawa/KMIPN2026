using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Mati : MonoBehaviour
{
    public GameObject matiPanel;

    public static Mati instance;
    void Start()
    {
        matiPanel.SetActive(false);
        instance = this;
    }

    public void mati()
    {
        matiPanel.SetActive(true);
        StartCoroutine(Wait(1.2f));
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Up");
    }
}
