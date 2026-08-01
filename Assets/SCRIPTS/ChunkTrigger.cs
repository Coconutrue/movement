using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    private bool hasSpawned = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;

            procedur_generate generator = FindFirstObjectByType<procedur_generate>();
            if (generator != null)
            {
                generator.SpawnNewChunk();
            }
        }
    }
}