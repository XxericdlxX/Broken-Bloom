using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    public GameObject gameOverUI;                // Panel del menú Game Over.
    public PlayerHealth playerHealth;            // Script que gestiona la salud del jugador.
    public TimerCountdown timerCountdown;        // Script que gestiona el tiempo de juego.
    public PotionUIManager potionUIManager;      // Script para cambiar la skin del jugador al perder.

    private bool isGameOver = false;             // Controla si ya se activó el Game Over.

    void Awake()
    {
        // si ya existe una instancia, destruye esta.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Oculta el panel de Game Over al iniciar la escena.
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        // Activa Game Over si el jugador pierde todas las vidas.
        if (playerHealth.GetCurrentLifeIndex() >= 4)
        {
            TriggerGameOver();
        }

        // Activa Game Over si se acaba el tiempo.
        if (timerCountdown.GetCurrentTime() <= 0f)
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Cambia la skin del jugador al modelo de "muerto".
        if (potionUIManager != null)
        {
            potionUIManager.ApplySkin(2);
        }

        // Muestra el panel de Game Over.
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Libera el cursor para que el jugador pueda usar el menú.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Pausa el juego.
        Time.timeScale = 0f;
    }

    // Reinicia la escena actual.
    public void OnRetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Carga el menú principal (escena índice 0).
    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Cierra la aplicación.
    public void OnQuitButton()
    {
        Application.Quit();
    }
}