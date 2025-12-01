using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeNivelPorPociones : MonoBehaviour
{
    public int pocionesRecogidas = 0; // Contador de pociones recogidas por el jugador.
    public int totalPociones = 4;     // Número total de pociones necesarias para cambiar de nivel.

    public void RegistrarPocion()
    {
        pocionesRecogidas++; // Suma una poción al contador.

        if (pocionesRecogidas >= totalPociones)
        {
            SceneManager.LoadScene(2); // Cambia a la escena en la posición 2
        }
    }
}