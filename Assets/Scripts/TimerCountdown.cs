using UnityEngine;
using TMPro;

public class TimerCountdown : MonoBehaviour
{
    public float totalTime = 150f;         // Tiempo total en segundos.
    public TMP_Text timerText;             // Referencia al texto que muestra el tiempo restante.

    private float currentTime;             // Tiempo actual restante.
    private bool isRunning = true;         // Controla si el temporizador está activo.

    void Start()
    {
        // Inicializa el tiempo restante con el valor total.
        currentTime = totalTime;

        // Si no se asignó el texto desde el Inspector, intenta encontrar uno en la escena.
        if (timerText == null)
        {
            timerText = GameObject.FindObjectOfType<TMP_Text>();
        }

        // Actualiza el texto del temporizador al iniciar.
        UpdateTimerDisplay();
    }

    void Update()
    {
        // Si el temporizador está detenido, no hace nada.
        if (!isRunning) return;

        // Resta tiempo en función del tiempo real transcurrido.
        currentTime -= Time.deltaTime;

        // Actualiza el texto del temporizador.
        UpdateTimerDisplay();

        // Si el tiempo se ha agotado, lo detiene y lo fija en cero.
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        // Si no hay referencia al texto, no actualiza.
        if (timerText == null) return;

        // Convierte el tiempo restante a minutos y segundos.
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        // Actualiza el texto con formato MM:SS.
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetCurrentTime()
    {
        // Devuelve el tiempo restante actual.
        return currentTime;
    }

    public void ResetTimer()
    {
        // Reinicia el temporizador al valor total y lo activa.
        currentTime = totalTime;
        isRunning = true;
        UpdateTimerDisplay();
    }
}