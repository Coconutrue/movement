using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedur_generate : MonoBehaviour
{
    public Transform Player;
    public Chunk[] ChunkPrefabs;
    public Chunk_step2[] ChunkPrefabs_step2;
    public Chunk FirstChunk;

    private List<Chunk> spawnedChunks = new List<Chunk>();
    private float count = 0;

    private void Start()
    {
        spawnedChunks.Add(FirstChunk);
        SpawnNewChunk(); 

    }

   private void Update()
    {
        if (Player.position.z > spawnedChunks[spawnedChunks.Count - 1].End.position.z - 40f)
        {
            SpawnNewChunk();
        }
        if (count >= 6) 
        {
            
        }
        
        
    }
    private void SpawnNewChunk()
    {   
        if (spawnedChunks.Length() >= 6)
        {
            Chunk newChunk = Instantiate(ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)]);
            newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position + (newChunk.transform.position - newChunk.Begin.position);
            count += 1;
            spawnedChunks.Add(newChunk);
        }
        count += 1;
        Chunk newChunk = Instantiate(ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)]);
        newChunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position + (newChunk.transform.position - newChunk.Begin.position);
        
        spawnedChunks.Add(newChunk);
    }
}
