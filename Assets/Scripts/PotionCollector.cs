using UnityEngine;
using UnityEngine.SceneManagement;

public class PotionCollector : MonoBehaviour
{
    
    public string collectibleTag = "collectibles";
    public int escenaDestino = 2;
    public int totalPociones = 4;
    public AudioClip vidaSound;

    private PotionUIManager uiManager;
    private PlayerHealth playerHealth;
    private AudioSource audioSource;

    private Collider lastPotionCollider;
    private bool potionCollected = false;
    private int pocionesRecogidas = 0;

    void Start()
    {
        uiManager = FindObjectOfType<PotionUIManager>();
        playerHealth = FindObjectOfType<PlayerHealth>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Comprova si s’ha recollit una poció i si encara existeix el seu collider
        if (potionCollected && lastPotionCollider != null)
        {
            // Si el jugador pot curar-se
            if (playerHealth != null && playerHealth.CanHeal())
            {
                // Augmenta la vida del jugador i reprodueix el so de curació si està definit
                if (playerHealth.IncreaseLife() && vidaSound != null)
                {
                    audioSource.PlayOneShot(vidaSound);
                }
            }

            // Destrueix la poció recollida de l’escena
            Destroy(lastPotionCollider.gameObject);

            // Actualitza la UI per mostrar la següent poció disponible
            if (uiManager != null)
            {
                uiManager.ShowNextPotion();
            }

            // Incrementa el comptador de pocions recollides
            pocionesRecogidas++;

            // Si s’han recollit totes les pocions necessàries, carrega l’escena de destí
            if (pocionesRecogidas >= totalPociones)
            {
                SceneManager.LoadScene(escenaDestino);
            }

            // Reinicia els flags per preparar la següent recollida
            potionCollected = false;
            lastPotionCollider = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Quan el jugador entra en contacte amb un objecte amb el tag "Collectibles"
        if (other.CompareTag(collectibleTag))
        {
            // Guarda el collider de la poció trobada
            lastPotionCollider = other;

            // Marca que s’ha recollit una poció
            potionCollected = true;
        }
    }
}