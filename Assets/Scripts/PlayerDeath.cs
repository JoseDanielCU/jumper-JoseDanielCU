using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar escena

public class PlayerDeath : MonoBehaviour
{
    public float fallLimitY = -10f; // Y mínima antes de morir (ajusta según tu mapa)

    private void Update()
    {
        // Si el jugador cae por debajo del mapa
        if (transform.position.y < fallLimitY)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si toca una trampa
        if (other.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Jugador muerto 😵");
        // Reiniciar la escena actual (simple)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // O si tienes un GameManager, podrías llamarlo aquí:
        // GameManager.instance.PlayerDied();
    }
}
