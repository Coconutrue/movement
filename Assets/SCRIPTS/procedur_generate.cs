using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class procedur_generate : MonoBehaviour
{
    public Transform Player;
    public Chunk FirstChunk;

    // Создаем удобную структуру для настройки шага в инспекторе
    [System.Serializable]
    public struct StepSettings
    {
        public string stepName; // Просто для красоты в инспекторе (например, "Step 1")
        public Chunk startChunk;
        public Chunk[] chunkPrefabs;
        public Chunk endChunk;
        public int chunksInThisStep; // Сколько ВСЕГО чанков должно быть в этом шаге (включая start и end)
    }

    [Header("Настройки Шагов (Задайте по очереди 1, 2, 3, 4...)")]
    public List<StepSettings> Steps = new List<StepSettings>();

    [Header("Финал")]
    public Chunk FinalChunk;

    private List<Chunk> spawnedChunks = new List<Chunk>();
    private int totalSpawnedCount = 0;
    private bool isFinalSpawned = false;

    // Очередь для уникального перемешивания (общая для текущего шага)
    private List<Chunk> currentStepQueue = new List<Chunk>();
    private int lastCheckedStepIndex = -1;

    private void Start()
    {
        if (FirstChunk != null)
        {
            spawnedChunks.Add(FirstChunk);
        }

        // Спавним стартовые чанки, чтобы игроку было где бежать
        for (int i = 0; i < 4; i++)
        {
            SpawnNewChunk();
        }
    }

    private void DelChunk()
    {
        if (isFinalSpawned) return;

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

        // Находим, к какому шагу относится текущий чанк по общему счетчику
        int currentStepIndex = -1;
        int accumulatedChunks = 0;
        int localIndexInStep = 0;

        for (int i = 0; i < Steps.Count; i++)
        {
            int stepTotal = Steps[i].chunksInThisStep;
            if (totalSpawnedCount < accumulatedChunks + stepTotal)
            {
                currentStepIndex = i;
                localIndexInStep = totalSpawnedCount - accumulatedChunks;
                break;
            }
            accumulatedChunks += stepTotal;
        }

        // 1. Одинаковая логика для всех шагов
        if (currentStepIndex != -1)
        {
            StepSettings currentStep = Steps[currentStepIndex];

            // Если перешли на новый шаг, очищаем старую очередь перемешивания
            if (currentStepIndex != lastCheckedStepIndex)
            {
                currentStepQueue.Clear();
                lastCheckedStepIndex = currentStepIndex;
            }

            // Логика внутри шага: старт, энд или рандомный уникальный чанк
            if (localIndexInStep == 0 && currentStep.startChunk != null)
            {
                prefabToSpawn = currentStep.startChunk;
            }
            else if (localIndexInStep == currentStep.chunksInThisStep - 1 && currentStep.endChunk != null)
            {
                prefabToSpawn = currentStep.endChunk;
            }
            else
            {
                prefabToSpawn = GetUniqueChunk(currentStep.chunkPrefabs, currentStepQueue);
            }
        }
        // 2. Если все шаги пройдены — спавним Финал
        else
        {
            prefabToSpawn = FinalChunk;
            isFinalSpawned = true;
        }

        // Спавн и позиционирование
        if (prefabToSpawn != null)
        {
            Chunk newChunk = Instantiate(prefabToSpawn);
            Vector3 spawnPosition;

            if (spawnedChunks.Count == 0)
            {
                spawnPosition = Player.position;
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
        else
        {
            Debug.LogWarning($"Пропущена ссылка на префаб при спавне чанка №{totalSpawnedCount}!");
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
