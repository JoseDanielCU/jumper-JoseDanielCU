using UnityEngine;

public class Coin : MonoBehaviour
{
    // Si quieres agregar sonido o efectos, puedes hacerlo aquí más adelante

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que entró en contacto tiene el tag "Player"
        if (other.CompareTag("Player"))
        {
            // Aquí puedes sumar puntos o reproducir un sonido si quieres
            Debug.Log("Moneda recogida!");

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}
