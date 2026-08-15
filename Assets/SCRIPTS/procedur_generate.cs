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
    public Chunk Step3_EndChunk;

    [Header("Step 4 (Identical Logic)")]
    public Chunk Step4_StartChunk;
    public Chunk[] ChunkPrefabs_step4;
    public Chunk Step4_EndChunk; 

    [Header("Final Settings (Spawn 1 time)")]
    public Chunk FinalChunk; 

    private List<Chunk> spawnedChunks = new List<Chunk>();
    private int totalSpawnedCount = 0;
    private bool isFinalSpawned = false; 

    private List<Chunk> step1Queue = new List<Chunk>();
    private List<Chunk> step2Queue = new List<Chunk>();
    private List<Chunk> step3Queue = new List<Chunk>();
    private List<Chunk> step4Queue = new List<Chunk>();

    private int step1_Start, step1_End;
    private int step2_Start, step2_End;
    private int step3_Start, step3_End;
    private int step4_Start, step4_End;
    private int finalIdx;

    private void Start()
    {
        step1_Start = 0;
        step1_End = step1_Start + (1 + ChunkPrefabs.Length + 1) - 1; 

        step2_Start = step1_End + 1;
        step2_End = step2_Start + (1 + ChunkPrefabs_step2.Length + 1) - 1;

        step3_Start = step2_End + 1;
        step3_End = step3_Start + (1 + ChunkPrefabs_step3.Length + 1) - 1;

        step4_Start = step3_End + 1;
        step4_End = step4_Start + (1 + ChunkPrefabs_step4.Length + 1) - 1;

        finalIdx = step4_End + 1;

        if (FirstChunk != null)
        {
            spawnedChunks.Add(FirstChunk);
            // ВАЖНО: Если FirstChunk НЕ является частью Step 1, 
            // он не должен смещать счетчик. Но если он считается за первый чанк,
            // раскомментируйте строку ниже, чтобы сдвинуть индексы:
            // totalSpawnedCount = 1; 
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
        if (isFinalSpawned) return;

        if (spawnedChunks.Count >= 6)
        {
            DelChunk();
        }

        Chunk prefabToSpawn = null;

        if (totalSpawnedCount == finalIdx)
        {
            prefabToSpawn = FinalChunk;
            isFinalSpawned = true; 
        }
        // STEP 1
        else if (totalSpawnedCount >= step1_Start && totalSpawnedCount <= step1_End)
        {
            if (totalSpawnedCount == step1_Start) prefabToSpawn = Step1_StartChunk;
            else if (totalSpawnedCount == step1_End) prefabToSpawn = Step1_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs, step1Queue);
        }
        // STEP 2
        else if (totalSpawnedCount >= step2_Start && totalSpawnedCount <= step2_End)
        {
            if (totalSpawnedCount == step2_Start) prefabToSpawn = Step2_StartChunk;
            else if (totalSpawnedCount == step2_End) prefabToSpawn = Step2_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs_step2, step2Queue);
        }
        // STEP 3
        else if (totalSpawnedCount >= step3_Start && totalSpawnedCount <= step3_End)
        {
            if (totalSpawnedCount == step3_Start) prefabToSpawn = Step3_StartChunk;
            else if (totalSpawnedCount == step3_End) prefabToSpawn = Step3_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs_step3, step3Queue);
        }
        // STEP 4
        else if (totalSpawnedCount >= step4_Start && totalSpawnedCount <= step4_End)
        {
            if (totalSpawnedCount == step4_Start) prefabToSpawn = Step4_StartChunk;
            else if (totalSpawnedCount == step4_End) prefabToSpawn = Step4_EndChunk;
            else prefabToSpawn = GetUniqueChunk(ChunkPrefabs_step4, step4Queue);
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
