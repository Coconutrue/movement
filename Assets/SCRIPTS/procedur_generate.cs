using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedur_generate : MonoBehaviour
{
    // Поле Player больше не нужно в Update, но оставим для инициализации первого чанка
    public Transform Player; 
    public Chunk[] ChunkPrefabs;
    public Chunk[] ChunkPrefabs_step2;
    public Chunk FirstChunk;

    private List<Chunk> spawnedChunks = new List<Chunk>();
    private int count = 0;

    private void Start()
    {
        if (FirstChunk != null)
        {
            spawnedChunks.Add(FirstChunk);
        }
        
        for (int i = 0; i < 3; i++)
        {
            SpawnNewChunk();
        }
    }


    private void DelChunk()
    {
        if (spawnedChunks.Count > 0)
        {
            Chunk oldestChunk = spawnedChunks[0];
            if (oldestChunk != null)
            {
                Destroy(oldestChunk.gameObject);
            }
            spawnedChunks.RemoveAt(0);
        }
        count += 1;
    }

    public void SpawnNewChunk()
    {
        if (spawnedChunks.Count >= 6)
        {
            DelChunk();
        }

        Chunk prefabToSpawn = null;
        switch (count)
        {
            case 0:
                prefabToSpawn = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)];
                break;
            default:
                prefabToSpawn = ChunkPrefabs_step2[Random.Range(0, ChunkPrefabs_step2.Length)];
                break;
        }

        if (prefabToSpawn != null)
        {
            Chunk newChunk = Instantiate(prefabToSpawn);
            Vector3 spawnPosition;

            if (spawnedChunks.Count == 0)
            {
                spawnPosition = new Vector3(Player.position.x, Player.position.y, Player.position.z);
                newChunk.transform.position = spawnPosition - (newChunk.Begin.position - newChunk.transform.position);
            }
            else
            {
                Vector3 lastChunkEnd = spawnedChunks[spawnedChunks.Count - 1].End.position;
                Vector3 newChunkBeginOffset = newChunk.Begin.position - newChunk.transform.position;
                
                spawnPosition = lastChunkEnd - newChunkBeginOffset;
                newChunk.transform.position = spawnPosition;
            }

            spawnedChunks.Add(newChunk);
        }
    }
}
