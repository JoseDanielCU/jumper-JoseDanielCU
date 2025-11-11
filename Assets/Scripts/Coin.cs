using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip collectSound;
    public GameObject collectEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Feedback visual en la moneda
            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);

            if (collectSound != null)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Llamar al PlayerColorFeedback si existe
            PlayerColorFeedback pcb = other.GetComponent<PlayerColorFeedback>();
            if (pcb == null)
            {
                // si el script está en un hijo (por ejemplo, Sprite child), busca arriba
                pcb = other.GetComponentInChildren<PlayerColorFeedback>();
            }
            if (pcb != null)
            {
                pcb.PlayFeedback(0.15f); // 0.15s (puedes ajustar)
            }

            Destroy(gameObject);
        }
    }
}
