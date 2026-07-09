using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoUp : MonoBehaviour
{
    public GameObject PanelUp;

    void Start()
    {
        PanelUp.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement.instance.DisableMovement();
            PlayerShoot.instance.isShooting = true;
            PanelUp.SetActive(true);
        }
    }
    
    public void GoUpButton()
    {
        StartCoroutine(Wait(1));
        PlayerShoot.instance.isShooting = false;
    }

    public void ClosePanel()
    {
        PanelUp.SetActive(false);
        PlayerShoot.instance.isShooting = false;
        PlayerMovement.instance.EnableMovement();
    }

    IEnumerator Wait(int time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Up");
    }
}
