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

    [Header("Chunk Overrides")]
    public List<ChunkOverride> chunkOverrides = new List<ChunkOverride>();

    [Header("Generation Settings")]
    public bool loopSequence = false;   // repeat the difficulty pattern
    public int totalRoomsOverride = -1; // optional override of total room count (-1 = sum of sequence)

    private Transform currentExit;
    private ChunkData lastChunkData;
    private int currentChunkIndex = 0;

    void Start()
    {
        currentExit = transform.Find("Entrance")?.transform;
        GenerateLevel();
    }

    void GenerateLevel()
    {
        int totalToGenerate = (totalRoomsOverride > 0) ? totalRoomsOverride : GetSequenceTotalRooms();

        if (totalToGenerate == 0)
            Debug.LogWarning("No rooms to generate");

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
            Difficulty currentDifficulty = currentSegment.difficulty;

            for (int i = 0; i < currentSegment.roomCount && roomsGenerated < totalToGenerate; i++)
            {
                SpawnChunk(currentDifficulty, currentChunkIndex);
                currentChunkIndex++;
                roomsGenerated++;
            }

            sequenceIndex++;
        }
    }

    void SpawnChunk(Difficulty difficulty, int index)
    {
        // Check if this index has a manual override
        ChunkData data = GetOverrideChunk(index);
        if (data == null)
        {
            // Pick random from the pool if not overridden
            data = ChooseWeightedChunk(GetPoolForDifficulty(difficulty));
            if (data == null) return;

            // Rule-based chaining (optional)
            if (lastChunkData != null && lastChunkData.allowedNextTags.Length > 0)
            {
                int tries = 0;
                while (!IsAllowedNext(lastChunkData, data) && tries < 10)
                {
                    data = ChooseWeightedChunk(GetPoolForDifficulty(difficulty));
                    tries++;
                }
            }
        }

        // Spawn the chunk prefab
        GameObject newChunk = Instantiate(data.prefab, transform);

        // Align with previous exit
        if (currentExit != null)
        {
            var chunk = newChunk.GetComponent<Chunk>();
            var entry = chunk.entryPoint;
            Vector3 offset = currentExit.position - (entry.position - newChunk.transform.position);
            newChunk.transform.position = offset;
        }

        // Update references
        currentExit = newChunk.GetComponent<Chunk>().exitPoint;
        lastChunkData = data;
    }

    // ---------------- Helper Methods ----------------

    ChunkData GetOverrideChunk(int index)
    {
        foreach (var ovr in chunkOverrides)
        {
            if (ovr.chunkIndex == index && ovr.specificChunk != null)
                return ovr.specificChunk;
        }
        return null;
    }

    bool IsAllowedNext(ChunkData prev, ChunkData next)
    {
        foreach (string allowed in prev.allowedNextTags)
            if (allowed == next.tagName)
                return true;
        return false;
    }

    List<ChunkData> GetPoolForDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Medium: return mediumChunks;
            case Difficulty.Hard: return hardChunks;
            case Difficulty.Easy:
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
    public Difficulty difficulty; // Now uses the enum!
    public int roomCount = 1;
}

[System.Serializable]
public class ChunkOverride
{
    public int chunkIndex;
    public ChunkData specificChunk;
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard
}
