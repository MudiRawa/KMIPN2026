using UnityEngine;
using UnityEngine.SceneManagement;

public class Dive : MonoBehaviour
{
    public GameObject buttonDive;

    void Start()
    {
        buttonDive.SetActive(false);
    }
    
    private void OnTriggerStay2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonDive.SetActive(true);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            buttonDive.SetActive(false);
        }
    }

    public void DiveButton(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
