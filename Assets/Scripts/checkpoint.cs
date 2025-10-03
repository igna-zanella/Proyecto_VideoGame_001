using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUIController ui = FindFirstObjectByType<GameUIController>();
            if (ui != null)
            {
                ui.EstablecerCheckpoint(transform.position);
                Debug.Log("Checkpoint actualizado");
            }
        }
    }
}

