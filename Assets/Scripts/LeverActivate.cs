using UnityEngine;

public class LeverActivate : MonoBehaviour
{
    public Transform leverHandle;               // Parte móvil de la palanca.
    public GameObject wall;                     // Muro que será destruido.
    public AudioSource leverSound;              // Sonido al activar la palanca.
    public GameObject sonidoExplosionPrefab;    // Prefab con AudioSource para explosión.
    public ParticleSystem explosionFX;          // Partículas de explosión.

    public float leverDownAngle = 45f;          // Ángulo al que baja la palanca.
    public float leverSpeed = 90f;              // Velocidad de rotación.
    public float explosionDelay = 2f;           // Tiempo entre activación y explosión.

    private Quaternion initialRotation;         // Rotación inicial de la palanca.
    private Quaternion targetRotation;          // Rotación final deseada.
    private bool isActivated = false;           // Controla si ya fue activada.
    private bool leverSoundPlayed = false;      // Evita repetir el sonido.
    private float activationTimer = 0f;         // Tiempo desde activación.

    void Start()
    {
        // Guarda rotación inicial y calcula la rotación objetivo.
        if (leverHandle != null)
        {
            initialRotation = leverHandle.localRotation;
            targetRotation = Quaternion.Euler(leverDownAngle, 0f, 0f);
        }
    }

    void Update()
    {
        if (isActivated)
        {
            activationTimer += Time.deltaTime;

            // Reproduce el sonido una sola vez.
            if (!leverSoundPlayed)
            {
                leverSound.Play();
                leverSoundPlayed = true;
            }

            // Baja la palanca suavemente.
            if (leverHandle != null)
            {
                leverHandle.localRotation = Quaternion.RotateTowards(
                    leverHandle.localRotation,
                    targetRotation,
                    leverSpeed * Time.deltaTime
                );
            }

            // Ejecuta la explosión cuando se cumple el retardo.
            if (activationTimer >= explosionDelay)
            {
                Vector3 spawnPos = wall.transform.position;
                Quaternion spawnRot = wall.transform.rotation;

                // Instancia partículas si están bien configuradas.
                if (explosionFX != null && explosionFX.main.duration > 0f)
                {
                    ParticleSystem fx = Instantiate(explosionFX, spawnPos, spawnRot);
                    fx.transform.localScale = wall.transform.localScale;
                    fx.Play();

                    float totalDuration = fx.main.duration + fx.main.startLifetime.constant;
                    Destroy(fx.gameObject, totalDuration + 0.5f);
                }

                // Instancia sonido de explosión desde prefab.
                if (sonidoExplosionPrefab != null)
                {
                    GameObject sonidoObj = Instantiate(sonidoExplosionPrefab, spawnPos, spawnRot);
                    AudioSource fxSound = sonidoObj.GetComponent<AudioSource>();

                    if (fxSound != null && fxSound.clip != null)
                    {
                        fxSound.Play();
                        Destroy(sonidoObj, fxSound.clip.length + 0.5f);
                    }
                }

                // Destruye el muro.
                if (wall != null)
                {
                    Destroy(wall);
                }

                // Finaliza la activación.
                isActivated = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Activa la palanca si el jugador entra y aún no se ha activado.
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            activationTimer = 0f;
            leverSoundPlayed = false;
        }
    }
}