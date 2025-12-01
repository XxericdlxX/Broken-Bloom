using UnityEngine;

public class PotionCollectorNivel2 : MonoBehaviour
{
    public string collectibleTag = "Collectibles";
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
        if (potionCollected && lastPotionCollider != null)
        {
            if (playerHealth != null && playerHealth.CanHeal())
            {
                if (playerHealth.IncreaseLife() && vidaSound != null)
                {
                    audioSource.PlayOneShot(vidaSound);
                }
            }

            Destroy(lastPotionCollider.gameObject);

            if (uiManager != null)
            {
                uiManager.ShowNextPotion();
            }

            pocionesRecogidas++;
            potionCollected = false;
            lastPotionCollider = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collectibleTag))
        {
            lastPotionCollider = other;
            potionCollected = true;
        }
    }
}