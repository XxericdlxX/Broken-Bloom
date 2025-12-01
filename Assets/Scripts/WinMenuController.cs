using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuController : MonoBehaviour
{
    public GameObject winMenuUI; // Panel del menú de victoria.

    void Start()
    {
        // Oculta el menú de victoria al iniciar la escena.
        if (winMenuUI != null)
            winMenuUI.SetActive(false);
    }

    public void ActivarMenuVictoria()
    {
        // Activa el menú de victoria si está asignado.
        if (winMenuUI != null)
        {
            winMenuUI.SetActive(true);

            // Libera el cursor para que el jugador pueda usar el menú.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Pausa el juego.
            Time.timeScale = 0f;
        }
    }

    public void VolverAJugar()
    {
        // Reanuda el juego y recarga la escena actual.
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverAlMenu()
    {
        // Reanuda el juego y carga la escena del menú principal.
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SalirDelJuego()
    {
        // Cierra la aplicación.
        Time.timeScale = 1f;
        Application.Quit();
    }
}