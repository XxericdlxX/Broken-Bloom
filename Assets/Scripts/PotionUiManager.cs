using UnityEngine;

public class PotionUIManager : MonoBehaviour
{
    public CanvasGroup[] uiPotionGroups;
    public RectTransform[] uiPotionRects;
    public int currentIndex = 0;
    public bool hasAllPotions = false;       // Indica si se han recogido todas las pociones.
    public bool potionTriggered = false;     // Bandera para iniciar la animación de la siguiente poción.
    public bool isAnimating = false;         // Controla si se está ejecutando una animación.
    public int animatingIndex = 0;           // Índice de la poción que se está animando.
    public float animationTimer = 0f;        // Temporizador para controlar la animación.

    public float fadeDuration = 1.0f;        // Duración de la animación de aparición.
    public float finalScale = 2.0f;          // Escala final de la poción en el HUD.
    public AudioSource pickupAudio;          // Sonido que se reproduce al recoger una poción.
    public GameObject[] playerModels;        // Modelos del jugador para cambiar de skin.
    public Avatar[] avatars;                 // Avatares para el Animator según el skin.
    public Animator playerAnimator;          // Animator del jugador para aplicar el avatar correspondiente.

    void Start()
    {
        // Activa solo el primer modelo del jugador.
        if (playerModels != null && playerModels.Length > 0)
        {
            for (int i = 0; i < playerModels.Length; i++)
            {
                if (playerModels[i] != null)
                {
                    playerModels[i].SetActive(i == 0);
                }
            }
        }

        // Busca todos los objetos con el tag "Ui_Collectibles" y los ordena por nombre.
        GameObject[] uiPotions = GameObject.FindGameObjectsWithTag("Ui_Collectibles");
        System.Array.Sort(uiPotions, (a, b) => a.name.CompareTo(b.name));

        // Inicializa los arrays de CanvasGroup y RectTransform.
        uiPotionGroups = new CanvasGroup[uiPotions.Length];
        uiPotionRects = new RectTransform[uiPotions.Length];

        // Configura cada poción en el HUD: oculta y escala a cero.
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
        // Si se ha recogido una poción y no hay animación en curso, iniciar animación.
        if (potionTriggered && !isAnimating)
        {
            StartPotionAnimation();
            potionTriggered = false;
        }

        // Si hay animación en curso, actualizarla.
        if (isAnimating)
        {
            AnimatePotion();
        }
    }

    public void ShowNextPotion()
    {
        // Marca que se debe mostrar la siguiente poción.
        potionTriggered = true;
    }

    public void StartPotionAnimation()
    {
        // Inicia la animación de la poción actual si hay más por mostrar.
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

    public void AnimatePotion()
    {
        // Calcula el progreso de la animación.
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
            // Finaliza la animación y actualiza el estado.
            CanvasGroup cg = uiPotionGroups[animatingIndex];
            RectTransform rect = uiPotionRects[animatingIndex];

            if (cg != null && rect != null)
            {
                cg.alpha = 1f;
                rect.localScale = Vector3.one * finalScale;
            }

            isAnimating = false;
            currentIndex++;

            // Si se han mostrado todas las pociones, marcar como completado.
            if (currentIndex == uiPotionGroups.Length)
            {
                hasAllPotions = true;
            }
        }
    }

    public void CheckCalderoTransformation()
    {
        // Si se han recogido todas las pociones, aplicar la skin de transformación.
        if (hasAllPotions)
        {
            ApplySkin(1);
        }
    }

    public bool HasAllPotions()
    {
        // Devuelve si el jugador ha recogido todas las pociones.
        return hasAllPotions;
    }

    public void ApplySkin(int skinIndex)
    {
        // Verifica que el índice sea válido.
        if (skinIndex < 0 || skinIndex >= playerModels.Length)
        {
            return;
        }

        // Guarda la posición y rotación del modelo activo.
        Vector3 lastPosition = Vector3.zero;
        Quaternion lastRotation = Quaternion.identity;

        for (int i = 0; i < playerModels.Length; i++)
        {
            if (playerModels[i] != null && playerModels[i].activeSelf)
            {
                lastPosition = playerModels[i].transform.position;
                lastRotation = playerModels[i].transform.rotation;
                break;
            }
        }

        // Desactiva todos los modelos del jugador.
        for (int i = 0; i < playerModels.Length; i++)
        {
            if (playerModels[i] != null)
            {
                playerModels[i].SetActive(false);
            }
        }

        // Activa el modelo correspondiente al nuevo skin.
        if (playerModels[skinIndex] != null)
        {
            playerModels[skinIndex].SetActive(true);
            playerModels[skinIndex].transform.position = lastPosition;
            playerModels[skinIndex].transform.rotation = lastRotation;
        }

        // Asigna el avatar correspondiente al Animator.
        if (playerAnimator != null && skinIndex < avatars.Length && avatars[skinIndex] != null)
        {
            playerAnimator.avatar = avatars[skinIndex];
        }
    }
}