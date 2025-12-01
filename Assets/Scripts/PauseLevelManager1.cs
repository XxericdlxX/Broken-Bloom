using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterAssets;

public class PausaLevelManager1 : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Slider volumenMusicaSlider;
    public Slider volumenEfectosSlider;
    public AudioSource musicaFondo;
    public List<AudioSource> efectosSonido;
    public ThirdPersonController controladorJugador;

    private bool isPaused = false;             // Controla si el joc està pausat.

    void Start()
    {
        // Oculta el menú de pausa al iniciar.
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Configura el slider de música si està assignat.
        if (volumenMusicaSlider != null)
        {
            volumenMusicaSlider.onValueChanged.AddListener(ActualizarVolumenMusica);
            volumenMusicaSlider.value = musicaFondo != null ? musicaFondo.volume : 1f;
        }

        // Configura el slider d'efectes si està assignat.
        if (volumenEfectosSlider != null)
        {
            volumenEfectosSlider.onValueChanged.AddListener(ActualizarVolumenEfectos);

            if (efectosSonido != null && efectosSonido.Count > 0)
                volumenEfectosSlider.value = efectosSonido[0].volume;
            else if (controladorJugador != null)
                volumenEfectosSlider.value = controladorJugador.FootstepAudioVolume;
            else
                volumenEfectosSlider.value = 1f;
        }

        // Bloqueja el cursor al iniciar el nivell.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Si no s'ha assignat manualment el controlador, intenta trobar-lo automàticament
        if (controladorJugador == null)
        {
            controladorJugador = FindObjectOfType<ThirdPersonController>();
        }
    }

    void Update()
    {
        // Alterna entre pausar i continuar amb la tecla P.
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void IrANivel2()
    {
        Time.timeScale = 1f;
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }

    public void ActualizarVolumenMusica(float nuevoVolumen)
    {
        if (musicaFondo != null)
            musicaFondo.volume = nuevoVolumen;
    }

    public void ActualizarVolumenEfectos(float nuevoVolumen)
    {
        // Ajusta el volum dels efectes normals
        if (efectosSonido != null)
        {
            foreach (AudioSource efecto in efectosSonido)
            {
                if (efecto != null)
                    efecto.volume = nuevoVolumen;
            }
        }

        // Ajusta el volum dels passos del jugador
        if (controladorJugador != null)
        {
            controladorJugador.FootstepAudioVolume = nuevoVolumen;
        }
    }
}