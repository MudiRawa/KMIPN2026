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
            PanelUp.SetActive(true);
            PlayerShoot.instance.isShooting = true;
        }
    }
    
    public void GoUpButton(string scene)
    {
        SceneManager.LoadScene(scene);
        PlayerShoot.instance.isShooting = false;
    }

    public void ClosePanel()
    {
        PanelUp.SetActive(false);
        PlayerShoot.instance.isShooting = false;
    }
}
