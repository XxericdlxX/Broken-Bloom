using UnityEngine;

public class PotionRandomizer : MonoBehaviour
{
    public GameObject[] potions;       // Array de prefabs de pociones que se colocarán en la escena.
    public Transform[] spawnPoints;    // Array de puntos donde pueden aparecer las pociones.

    private bool potionsPlaced = false; // Controla si ya se han colocado las pociones.

    void Update()
    {
        // Coloca las pociones solo una vez, al inicio del juego.
        if (!potionsPlaced && potions.Length > 0 && spawnPoints.Length > 0)
        {
            PlacePotionsRandomly();
            potionsPlaced = true;
        }
    }

    void PlacePotionsRandomly()
    {
        // Asigna a cada poción una posición aleatoria de entre los puntos disponibles.
        for (int i = 0; i < potions.Length; i++)
        {
            int index = Random.Range(0, spawnPoints.Length);
            potions[i].transform.position = spawnPoints[index].position;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja esferas verdes en el editor para visualizar los puntos de aparición.
        Gizmos.color = Color.green;
        if (spawnPoints != null)
        {
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                    Gizmos.DrawSphere(point.position, 0.5f);
            }
        }
    }
}