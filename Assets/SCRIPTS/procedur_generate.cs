using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedur_generate : MonoBehaviour
{
    public Transform Player;
    public Chunk FirstChunk;

    [Header("Step 1 (Chunks 1-6)")]
    public Chunk Step1_StartChunk;
    public Chunk[] ChunkPrefabs;
    public Chunk Step1_EndChunk;

    [Header("Step 2 (Chunks 7-12)")]
    public Chunk Step2_StartChunk;
    public Chunk[] ChunkPrefabs_step2;
    public Chunk Step2_EndChunk;

    [Header("Step 3 (Chunks 13-18)")]
    public Chunk Step3_StartChunk;
    public Chunk[] ChunkPrefabs_step3;
    public Chunk Step3_EndChunk; // 18-й чанк (конец 3 степа)

    [Header("Step 4 (Chunks 19+)")]
    public Chunk Step4_StartChunk; // Фиксированный 19-й чанк
    public Chunk[] ChunkPrefabs_step4; // Случайные чанки для 4 степа

    private List<Chunk> spawnedChunks = new List<Chunk>();
    private int totalSpawnedCount = 0;
    private List<Chunk> step1Queue = new List<Chunk>();
    private List<Chunk> step2Queue = new List<Chunk>();
    private List<Chunk> step3Queue = new List<Chunk>();
    private List<Chunk> step4Queue = new List<Chunk>();

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
    }

    public void SpawnNewChunk()
    {
        if (spawnedChunks.Count >= 6)
        {
            DelChunk();
        }

        Chunk prefabToSpawn = null;

        // --- ЛОГИКА ВЫБОРА ЧАНКОВ ПО СТЕПАМ И ПОЗИЦИЯМ ---
        // STEP 1 (0 - 5)
        if (totalSpawnedCount < 6)
        {
            if (totalSpawnedCount == 0) prefabToSpawn = Step1_StartChunk;
            else if (totalSpawnedCount == 5) prefabToSpawn = Step1_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs, step1Queue);
        }
        // STEP 2 (6 - 11)
        else if (totalSpawnedCount < 12)
        {
            if (totalSpawnedCount == 6) prefabToSpawn = Step2_StartChunk;
            else if (totalSpawnedCount == 11) prefabToSpawn = Step2_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs_step2, step2Queue);
        }
        // STEP 3 (12 - 17)
        else if (totalSpawnedCount < 18)
        {
            if (totalSpawnedCount == 12) prefabToSpawn = Step3_StartChunk;
            else if (totalSpawnedCount == 17) prefabToSpawn = Step3_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs_step3, step3Queue);
        }
        // STEP 4 (18 и дальше - бесконечный рандом со стартовым чанком 19-го)
        else
        {
            if (totalSpawnedCount == 18) prefabToSpawn = Step4_StartChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs_step4, step4Queue);
        }

        // Спавн и позиционирование
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
            totalSpawnedCount++;
        }
    }

    private Chunk GetUniqueChunk(Chunk[] originPrefabs, List<Chunk> shuffleQueue)
    {
        if (originPrefabs == null || originPrefabs.Length == 0) return null;

        if (shuffleQueue.Count == 0)
        {
            shuffleQueue.AddRange(originPrefabs);
            ShuffleList(shuffleQueue);
        }

        Chunk selectedChunk = shuffleQueue[0];
        shuffleQueue.RemoveAt(0);
        return selectedChunk;
    }

    private void ShuffleList(List<Chunk> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Chunk temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
