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

    [Header("Streaming Settings")]
    public Transform player;
    public int chunksAhead = 10;
    public float chunkDistAhead = 50f;
    public float chunkDistBehind = 100f;

    [Header("Generation Settings")]
    public bool loopSequence = false;
    public int totalRoomsOverride = -1;

    private Transform currentExit;
    private ChunkData lastChunkData;
    private int currentChunkIndex = 0;
    private List<GameObject> activeChunks = new List<GameObject>();

    private int sequenceIndex = 0;
    private int totalToGenerate;

    void Start()
    {
        totalToGenerate = (totalRoomsOverride > 0) ? totalRoomsOverride : GetSequenceTotalRooms();
        // Start with one or two chunks initially
        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (player == null || activeChunks.Count == 0)
            return;

        // Check if the player passed the midpoint of the current chunk
        var firstChunk = activeChunks[0];
        var lastChunk = activeChunks[activeChunks.Count - 1];
        float playerX = player.position.x;

        // Spawn new chunks if player approaches the end of current ones
        if (playerX > lastChunk.GetComponent<Chunk>().exitPoint.position.x - chunkDistAhead)
        {
            SpawnNextChunk();
        }

        // Despawn old chunks if too far behind
        while (activeChunks.Count > 0 &&
               playerX - activeChunks[0].GetComponent<Chunk>().exitPoint.position.x > chunkDistBehind)
        {
            Destroy(activeChunks[0]);
            activeChunks.RemoveAt(0);
        }
    }

    void SpawnNextChunk()
    {
        if (currentChunkIndex >= totalToGenerate)
            return;

        Difficulty currentDifficulty = GetDifficultyForIndex(currentChunkIndex);
        SpawnChunk(currentDifficulty, currentChunkIndex);
        currentChunkIndex++;
    }

    void SpawnChunk(Difficulty difficulty, int index)
    {
        ChunkData data = GetOverrideChunk(index);

        if (data == null)
        {
            data = ChooseWeightedChunk(GetPoolForDifficulty(difficulty));
            if (data == null) return;

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

        GameObject newChunk = Instantiate(data.prefab);
        var chunk = newChunk.GetComponent<Chunk>();

        if (currentExit != null)
        {
            Vector3 offset = currentExit.position - (chunk.entryPoint.position - newChunk.transform.position);
            newChunk.transform.position = offset;
        }

        currentExit = chunk.exitPoint;
        lastChunkData = data;

        activeChunks.Add(newChunk);
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

    Difficulty GetDifficultyForIndex(int index)
    {
        int sum = 0;
        foreach (var segment in difficultySequence)
        {
            for (int i = 0; i < segment.roomCount; i++)
            {
                if (index == sum) return segment.difficulty;
                sum++;
            }
        }
        return Difficulty.Easy;
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
