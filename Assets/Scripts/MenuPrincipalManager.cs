using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipalManager : MonoBehaviour
{
    public GameObject pauseMenuUI;     // Panel del menú (sempre visible).
    public Slider volumenSlider;       // Slider per ajustar el volum.
    public AudioSource musicaFondo;    // Música de fons del nivell.

    void Start()
    {
        // Manté el menú sempre visible quan la escena està activa
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        // Configura el slider de volum si està assignat
        if (volumenSlider != null)
        {
            volumenSlider.onValueChanged.AddListener(ActualizarVolumen);
            volumenSlider.value = musicaFondo != null ? musicaFondo.volume : 1f;
        }

        // El cursor sempre lliure per interactuar amb el menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // El joc continua a velocitat normal
        Time.timeScale = 1f;
    }

    public void SalirDelJuego()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    // Botó Nivel 1
    public void IrANivel1()
    {
        Time.timeScale = 1f;
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }

    // Botó Nivel 2
    public void IrANivel2()
    {
        Time.timeScale = 1f;
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 2);
    }

    public void ActualizarVolumen(float nuevoVolumen)
    {
        // Ajusta el volum de la música si està assignada
        if (musicaFondo != null)
        {
            musicaFondo.volume = nuevoVolumen;
        }
    }
}