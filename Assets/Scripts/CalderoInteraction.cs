using UnityEngine;

public class CalderoInteraction : MonoBehaviour
{
    public PotionUIManager potionUIManager;
    public WinMenuController winMenuController;

    void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra tiene el tag "Player".
        if (other.CompareTag("Player"))
        {
            // Comprueba que potionUIManager está asignado y que el jugador tiene todas las pociones.
            if (potionUIManager != null && potionUIManager.HasAllPotions())
            {
                // Ejecuta la transformación del caldero.
                potionUIManager.CheckCalderoTransformation();

                // Activa el menú de victoria si está asignado.
                if (winMenuController != null)
                {
                    winMenuController.ActivarMenuVictoria();
                }

                //  Hace rotar al jugador 180 grados sobre el eje Y
                Transform playerTransform = other.transform;
                playerTransform.Rotate(0f, 180f, 0f);
            }
        }
    }
}