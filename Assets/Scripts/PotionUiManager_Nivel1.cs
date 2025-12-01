using UnityEngine;
using UnityEngine.SceneManagement;

public class PotionUIManager_Nivel1 : MonoBehaviour
{
    public CanvasGroup[] uiPotionGroups;
    public RectTransform[] uiPotionRects;
    public AudioSource pickupAudio;

    public float fadeDuration = 1.0f;        // Duración de la animación de aparición.
    public float finalScale = 2.0f;          // Escala final de la poción en el HUD.

    private int currentIndex = 0;            // Índice de la poción actual que se está mostrando.
    private bool isAnimating = false;        // Controla si se está ejecutando una animación.
    private float animationTimer = 0f;       // Temporizador para controlar la animación.
    private int animatingIndex = 0;          // Índice de la poción que se está animando.
    private bool potionTriggered = false;    // Bandera para iniciar la animación de la siguiente poción.

    void Start()
    {
        GameObject[] uiPotions = GameObject.FindGameObjectsWithTag("Ui_Collectibles");
        System.Array.Sort(uiPotions, (a, b) => a.name.CompareTo(b.name));

        uiPotionGroups = new CanvasGroup[uiPotions.Length];
        uiPotionRects = new RectTransform[uiPotions.Length];

        for (int i = 0; i < uiPotions.Length; i++)
        {
            CanvasGroup cg = uiPotions[i].GetComponent<CanvasGroup>();
            RectTransform rect = uiPotions[i].GetComponent<RectTransform>();

            if (cg != null && rect != null)
            {
                rect.localScale = Vector3.zero;
                cg.alpha = 0f;
                cg.gameObject.SetActive(true);

                uiPotionGroups[i] = cg;
                uiPotionRects[i] = rect;
            }
        }
    }

    void Update()
    {
        if (potionTriggered && !isAnimating)
        {
            StartPotionAnimation();
            potionTriggered = false;
        }

        if (isAnimating)
        {
            AnimatePotion();
        }
    }

    public void TriggerPotionPickup()
    {
        potionTriggered = true;
    }

    private void StartPotionAnimation()
    {
        if (currentIndex < uiPotionGroups.Length)
        {
            if (pickupAudio != null)
            {
                pickupAudio.pitch = Random.Range(0.95f, 1.05f);
                pickupAudio.Play();
            }

            isAnimating = true;
            animationTimer = 0f;
            animatingIndex = currentIndex;
        }
    }

    private void AnimatePotion()
    {
        animationTimer += Time.deltaTime;
        float t = animationTimer / fadeDuration;

        if (t <= 1f)
        {
            CanvasGroup cg = uiPotionGroups[animatingIndex];
            RectTransform rect = uiPotionRects[animatingIndex];

            if (cg != null && rect != null)
            {
                cg.alpha = Mathf.Lerp(0f, 1f, t);
                rect.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * finalScale, t);
            }
        }
        else
        {
            CanvasGroup cg = uiPotionGroups[animatingIndex];
            RectTransform rect = uiPotionRects[animatingIndex];

            if (cg != null && rect != null)
            {
                cg.alpha = 1f;
                rect.localScale = Vector3.one * finalScale;
            }

            isAnimating = false;
            currentIndex++;

            if (currentIndex == uiPotionGroups.Length)
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}