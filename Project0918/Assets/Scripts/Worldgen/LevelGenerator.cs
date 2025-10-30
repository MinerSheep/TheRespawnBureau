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
    public int totalRoomsOverride = -1;

    private Transform currentExit;
    private ChunkData lastChunkData;
    private int currentChunkIndex = 0;
    private List<GameObject> activeChunks = new List<GameObject>();

    private int totalToGenerate;

    GameObject currentPlayerChunk;
    GameObject lastPlayerChunk = null;

    void Start()
    {
        totalToGenerate = (totalRoomsOverride > 0) ? totalRoomsOverride : GetSequenceTotalRooms();
        Debug.Log($"[LevelGen] Starting Level Generation — total rooms: {totalToGenerate}");

        for (int i = 0; i < chunksAhead; i++)
        {
            SpawnNextChunk();
        }
    }

    void Update()
    {
        if (player == null || activeChunks.Count == 0)
            return;

        float playerX = player.position.x;
        var firstChunk = activeChunks[0];
        var lastChunk = activeChunks[activeChunks.Count - 1];

        // Debug player progress
        Debug.DrawLine(Vector3.right * playerX, Vector3.right * (playerX + 2f), Color.yellow);

        // Spawn new chunks if player approaches the end
        if (playerX > lastChunk.GetComponent<Chunk>().exitPoint.position.x - chunkDistAhead)
        {
            // Debug.Log($"[LevelGen] Player reached near end of chunk {currentChunkIndex - 1}. Spawning next...");
            SpawnNextChunk();
        }

        // Try to find which chunk the player is in
        currentPlayerChunk = FindPlayerChunk();

        if (currentPlayerChunk != null && currentPlayerChunk != lastPlayerChunk)
        {
            Debug.Log($"[LevelGen] Player is in Chunk: {currentPlayerChunk.name}");
            lastPlayerChunk = currentPlayerChunk;
        }

        // Despawn old chunks behind
        while (activeChunks.Count > 0 &&
               playerX - activeChunks[0].GetComponent<Chunk>().exitPoint.position.x > chunkDistBehind)
        {
            // Debug.Log($"[LevelGen] Despawning chunk {activeChunks[0].name} (behind player).");
            Destroy(activeChunks[0]);
            activeChunks.RemoveAt(0);
        }
    }

    void SpawnNextChunk()
    {
        if (currentChunkIndex >= totalToGenerate)
        {
            // Debug.Log("[LevelGen] All chunks generated, stopping.");
            return;
        }

        Difficulty currentDifficulty = GetDifficultyForIndex(currentChunkIndex);
        // Debug.Log($"[LevelGen] Spawning chunk #{currentChunkIndex} (Difficulty: {currentDifficulty})");

        SpawnChunk(currentDifficulty, currentChunkIndex);
        currentChunkIndex++;
    }

    void SpawnChunk(Difficulty difficulty, int index)
    {
        ChunkData data = GetOverrideChunk(index);

        if (data != null)
        {
            // Debug.Log($"[LevelGen] Using OVERRIDE chunk '{data.name}' for index {index}");
        }
        else
        {
            data = ChooseWeightedChunk(GetPoolForDifficulty(difficulty));
            if (data == null)
            {
                // Debug.LogError($"[LevelGen] ERROR: No chunks found for difficulty {difficulty}");
                return;
            }

            // Debug.Log($"[LevelGen] Randomly selected chunk '{data.name}' (tag: {data.tagName}) for difficulty {difficulty}");

            // Rule-based chaining
            if (lastChunkData != null && lastChunkData.allowedNextTags.Length > 0)
            {
                int tries = 0;
                while (!IsAllowedNext(lastChunkData, data) && tries < 10)
                {
                    // Debug.LogWarning($"[LevelGen] Chunk '{data.name}' not allowed after '{lastChunkData.name}'. Retrying...");
                    data = ChooseWeightedChunk(GetPoolForDifficulty(difficulty));
                    tries++;
                }

                if (tries >= 10) { }
                    // Debug.LogError($"[LevelGen] Failed to find valid chunk after '{lastChunkData.name}' — using last attempt '{data.name}'");
            }
        }

        GameObject newChunk = Instantiate(data.prefab);
        newChunk.name = $"Chunk_{index}_{data.name}_{difficulty}";

        var chunk = newChunk.GetComponent<Chunk>();
        if (currentExit != null)
        {
            Vector3 offset = currentExit.position - (chunk.entryPoint.position - newChunk.transform.position);
            newChunk.transform.position = offset;
        }

        currentExit = chunk.exitPoint;
        lastChunkData = data;

        activeChunks.Add(newChunk);
        // Debug.Log($"[LevelGen] Spawned {newChunk.name} at position {newChunk.transform.position}");
    }

    // ---------------- Helper Methods ----------------

    GameObject FindPlayerChunk()
    {
        // Get all chunks currently active

        foreach (var chunk in activeChunks)
        {
            if (((chunk.GetComponent<Chunk>().exitPoint.position.x - player.position.x) > 0) && ((chunk.GetComponent<Chunk>().entryPoint.position.x - player.position.x) < 0)) { 
                return chunk;
            }
        }

        return null;
    }


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
        if (difficultySequence == null || difficultySequence.Count == 0)
        {
            // Debug.LogWarning("[LevelGen] No difficulty sequence defined, defaulting to Easy.");
            return Difficulty.Easy;
        }

        int totalRooms = GetSequenceTotalRooms();

        // --- HANDLE LOOPING ---
        if (totalRooms > 0)
        {
            index = index % totalRooms; // wrap around if looping
        }
        else if (index >= totalRooms)
        {
            // --- NON-LOOPING: stop generation when sequence ends ---
            // Debug.Log($"[LevelGen] Index {index} beyond sequence end; stopping generation.");
            return difficultySequence[difficultySequence.Count - 1].difficulty; // return last difficulty as fallback
        }

        int cumulative = 0;
        foreach (var segment in difficultySequence)
        {
            for (int i = 0; i < segment.roomCount; i++)
            {
                if (index == cumulative)
                {
                    // Debug.Log($"[LevelGen] Index {index} → Difficulty {segment.difficulty}");
                    return segment.difficulty;
                }
                cumulative++;
            }
        }

        // Debug.LogWarning($"[LevelGen] Index {index} out of bounds, defaulting to Easy.");
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
            case Difficulty.Medium:
                // Debug.Log("[LevelGen] Using MEDIUM pool");
                return mediumChunks;
            case Difficulty.Hard:
                // Debug.Log("[LevelGen] Using HARD pool");
                return hardChunks;
            default:
                // Debug.Log("[LevelGen] Using EASY pool");
                return easyChunks;
        }
    }

    ChunkData ChooseWeightedChunk(List<ChunkData> pool)
    {
        if (pool == null || pool.Count == 0)
        {
            // Debug.LogError("[LevelGen] Empty chunk pool!");
            return null;
        }

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
