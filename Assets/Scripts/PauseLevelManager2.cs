using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using StarterAssets;

public class PausaLevelManager2 : MonoBehaviour
{    public GameObject pauseMenuUI;
    public Slider volumenMusicaSlider;
    public Slider volumenEfectosSlider;
    public AudioSource musicaFondo;
    public List<AudioSource> efectosSonido;
    public ThirdPersonController controladorJugador;
    public Toggle irEscenaSiguienteToggle;
    private bool isPaused = false;

    void Start()
    {
        // Amaga el menú de pausa al començar
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        // Configura el slider de música amb el volum actual
        if (volumenMusicaSlider != null)
        {
            volumenMusicaSlider.onValueChanged.AddListener(ActualizarVolumenMusica);
            volumenMusicaSlider.value = musicaFondo != null ? musicaFondo.volume : 1f;
        }

        // Configura el slider d’efectes amb el volum actual
        if (volumenEfectosSlider != null)
        {
            volumenEfectosSlider.onValueChanged.AddListener(ActualizarVolumenEfectos);

            if (efectosSonido != null && efectosSonido.Count > 0)
                volumenEfectosSlider.value = efectosSonido[0].volume; // Agafa el primer efecte com a referència
            else if (controladorJugador != null)
                volumenEfectosSlider.value = controladorJugador.FootstepAudioVolume; // Si no hi ha efectes, usa el volum de les passes
            else
                volumenEfectosSlider.value = 1f; // Valor per defecte
        }

        // Bloqueja i amaga el cursor al començar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Si no s’ha assignat el controlador, el busca automàticament
        if (controladorJugador == null)
            controladorJugador = FindObjectOfType<ThirdPersonController>();

        // Configura el toggle per anar a l’escena següent
        if (irEscenaSiguienteToggle != null)
        {
            irEscenaSiguienteToggle.isOn = false; // Inicialment desactivat
            irEscenaSiguienteToggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    void Update()
    {
        //Control amb la tecla P
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Posa el joc en pausa
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Atura el temps del joc

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true); // Mostra el menú de pausa

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Reprèn el joc
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reactiva el temps del joc

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); // Amaga el menú de pausa

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Torna al menú principal
    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Tanca el joc completament
    public void SalirDelJuego()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    // Va directament al nivell 1
    public void IrANivel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    // Actualitza el volum de la música de fons
    public void ActualizarVolumenMusica(float nuevoVolumen)
    {
        if (musicaFondo != null)
            musicaFondo.volume = nuevoVolumen;
    }

    // Actualitza el volum dels efectes de so i del jugador
    public void ActualizarVolumenEfectos(float nuevoVolumen)
    {
        if (efectosSonido != null)
        {
            foreach (AudioSource efecto in efectosSonido)
            {
                if (efecto != null)
                    efecto.volume = nuevoVolumen;
            }
        }

        if (controladorJugador != null)
            controladorJugador.FootstepAudioVolume = nuevoVolumen;
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            int escenaActual = SceneManager.GetActiveScene().buildIndex;
            int escenaSiguiente = escenaActual + 1;

            if (escenaSiguiente < SceneManager.sceneCountInBuildSettings)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(escenaSiguiente);
            }
        }
    }
}