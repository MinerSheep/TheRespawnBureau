using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Pools by Difficulty")]
    public List<ChunkData> easyChunks;
    public List<ChunkData> mediumChunks;
    public List<ChunkData> hardChunks;

    [Header("Generation Settings")]
    public int numberOfChunks = 10;
    public string startDifficulty = "Easy";
    public bool increaseDifficultyOverTime = true;

    private Transform currentExit;
    private string currentDifficulty;
    private ChunkData lastChunkData;

    void Start()
    {
        currentDifficulty = startDifficulty;
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int i = 0; i < numberOfChunks; i++)
        {
            // Optionally increase difficulty as we go
            if (increaseDifficultyOverTime)
            {
                if (i > numberOfChunks * 0.33f) currentDifficulty = "Medium";
                if (i > numberOfChunks * 0.66f) currentDifficulty = "Hard";
            }

            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        ChunkData data = ChooseWeightedChunk(GetPoolForDifficulty(currentDifficulty));
        if (data == null) return;

        // Rule-based chaining: ensure new chunk is allowed after last
        if (lastChunkData != null && lastChunkData.allowedNextTags.Length > 0)
        {
            int tries = 0;
            while (!IsAllowedNext(lastChunkData, data) && tries < 10)
            {
                data = ChooseWeightedChunk(GetPoolForDifficulty(currentDifficulty));
                tries++;
            }
        }

        GameObject newChunk = Instantiate(data.prefab);

        // Align with current exit
        if (currentExit != null)
        {
            var chunk = newChunk.GetComponent<Chunk>();
            var entry = chunk.entryPoint;
            Vector3 offset = currentExit.position - (entry.position - newChunk.transform.position);
            newChunk.transform.position = offset;
        }

        // Update tracking
        currentExit = newChunk.GetComponent<Chunk>().exitPoint;
        lastChunkData = data;
    }

    bool IsAllowedNext(ChunkData prev, ChunkData next)
    {
        foreach (string allowed in prev.allowedNextTags)
            if (allowed == next.tagName)
                return true;
        return false;
    }

    List<ChunkData> GetPoolForDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Medium": return mediumChunks;
            case "Hard": return hardChunks;
            default: return easyChunks;
        }
    }

    ChunkData ChooseWeightedChunk(List<ChunkData> pool)
    {
        if (pool == null || pool.Count == 0) return null;

        float totalWeight = 0;
        foreach (var c in pool) totalWeight += c.weight;

        float randomValue = Random.value * totalWeight;
        float cumulative = 0;

        foreach (var c in pool)
        {
            cumulative += c.weight;
            if (randomValue <= cumulative)
                return c;
        }
        return pool[0];
    }
}
