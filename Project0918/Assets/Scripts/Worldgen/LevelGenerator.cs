using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Pools by Difficulty")]
    public List<ChunkData> easyChunks;
    public List<ChunkData> mediumChunks;
    public List<ChunkData> hardChunks;

    [Header("Difficulty Sequence")]
    public List<DifficultySegment> difficultySequence = new List<DifficultySegment>();

    [Header("Generation Settings")]
    public bool loopSequence = false;   // if true, repeat the sequence if we run out
    public int totalRoomsOverride = -1; // optional total count override (-1 = use sequence total)

    private Transform currentExit;
    private string currentDifficulty;
    private ChunkData lastChunkData;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        int totalToGenerate = (totalRoomsOverride > 0) ? totalRoomsOverride : GetSequenceTotalRooms();

        int roomsGenerated = 0;
        int sequenceIndex = 0;

        while (roomsGenerated < totalToGenerate)
        {
            if (sequenceIndex >= difficultySequence.Count)
            {
                if (loopSequence)
                    sequenceIndex = 0;
                else
                    break;
            }

            DifficultySegment currentSegment = difficultySequence[sequenceIndex];
            currentDifficulty = currentSegment.difficultyName;

            for (int i = 0; i < currentSegment.roomCount && roomsGenerated < totalToGenerate; i++)
            {
                SpawnChunk();
                roomsGenerated++;
            }

            sequenceIndex++;
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

    int GetSequenceTotalRooms()
    {
        int total = 0;
        foreach (var segment in difficultySequence)
            total += segment.roomCount;
        return total;
    }
}

[System.Serializable]
public class DifficultySegment
{
    public string difficultyName;
    public int roomCount = 1;
}
