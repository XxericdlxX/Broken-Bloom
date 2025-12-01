using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] vidaImages;
    public AudioClip damageSound;
    private AudioSource audioSource;
    private int currentLifeIndex = 0;    // Índice actual de vida (0 = sin daño, 4 = sin vidas).
    private bool canTakeDamage = true;   // Controla si el jugador puede recibir daño.
    public float invulnerabilityTime = 1.0f; // Tiempo de invulnerabilidad tras recibir daño.
    private float invulnerabilityTimer = 0f; // Temporizador para contar la invulnerabilidad.

    void Start()
    {
        // Asigna el componente AudioSource si no está presente.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Inicializa la vida visual en el índice 0.
        SetActiveLife(0);
    }

    void Update()
    {
        // Si el jugador está invulnerable, cuenta hacia atrás.
        if (!canTakeDamage)
        {
            invulnerabilityTimer -= Time.deltaTime;

            // Cuando el tiempo se agota, vuelve a permitir daño.
            if (invulnerabilityTimer <= 0f)
            {
                canTakeDamage = true;
                invulnerabilityTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si colisiona con un objeto con tag "Debris" y puede recibir daño.
        if (other.CompareTag("Debris") && canTakeDamage)
        {
            ReduceLife();
        }
    }

    void ReduceLife()
    {
        // Solo reduce vida si aún no ha llegado al máximo (4).
        if (currentLifeIndex < 4)
        {
            currentLifeIndex++;

            // Actualiza la imagen de vida activa.
            SetActiveLife(currentLifeIndex);

            // Reproduce el sonido de daño si está asignado.
            if (damageSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(damageSound);
            }

            // Activa el estado de invulnerabilidad temporal.
            ActivateInvulnerability();
        }
    }

    public bool IncreaseLife()
    {
        // Solo cura si hay daño (índice mayor a 0).
        if (currentLifeIndex > 0)
        {
            currentLifeIndex--;

            // Actualiza la imagen de vida activa.
            SetActiveLife(currentLifeIndex);
            return true;
        }

        return false;
    }

    public bool CanHeal()
    {
        // Devuelve true si el jugador tiene al menos un punto de daño.
        return currentLifeIndex > 0;
    }

    public int GetCurrentLifeIndex()
    {
        // Devuelve el índice actual de vida.
        return currentLifeIndex;
    }

    void SetActiveLife(int lifeIndex)
    {
        // Desactiva todas las imágenes de vida.
        for (int i = 0; i < vidaImages.Length; i++)
        {
            if (vidaImages[i] != null)
                vidaImages[i].gameObject.SetActive(false);
        }

        // Activa solo la imagen correspondiente al índice actual.
        if (lifeIndex >= 0 && lifeIndex < vidaImages.Length && vidaImages[lifeIndex] != null)
        {
            vidaImages[lifeIndex].gameObject.SetActive(true);
        }
    }

    void ActivateInvulnerability()
    {
        // Desactiva el daño y reinicia el temporizador.
        canTakeDamage = false;
        invulnerabilityTimer = invulnerabilityTime;
    }

    public void ResetHealth()
    {
        // Reinicia la vida y el estado de daño.
        currentLifeIndex = 0;
        SetActiveLife(0);
        canTakeDamage = true;
        invulnerabilityTimer = 0f;
    }
}