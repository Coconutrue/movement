using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedur_generate : MonoBehaviour
{
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
        SpawnNewChunk(); 
    }

    private void Update()
    {
        if (spawnedChunks.Count == 0) return;

        if (Player.position.z > spawnedChunks[spawnedChunks.Count - 1].End.position.z - 40f)
        {
            SpawnNewChunk();
        }
    }

    private void DelChunk()
    {
        // for (int i = 0; i < spawnedChunks.Count - 1; i++)
        // {
        //     if (spawnedChunks[i] != null)
        //     {
        //         Destroy(spawnedChunks[i].gameObject);
        //     }
        // }
        // spawnedChunks.Clear();
        count += 1;
    }

    private void SpawnNewChunk()
    {   
        if (spawnedChunks.Count >= 6)
        {
            DelChunk();
            // return; 
        } 

        Chunk prefabToSpawn = null;

        switch (count) 
        {
            case 0:
                prefabToSpawn = ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)];
                break;
            case 1: 
                prefabToSpawn = ChunkPrefabs_step2[Random.Range(0, ChunkPrefabs_step2.Length)];
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
                newChunk.transform.position = spawnPosition - newChunk.Begin.position;
            }
            else
            {
                spawnPosition = spawnedChunks[spawnedChunks.Count - 1].End.position + (newChunk.transform.position - newChunk.Begin.position);
                newChunk.transform.position = spawnPosition;
            }
            
            spawnedChunks.Add(newChunk);
        }
    }
}