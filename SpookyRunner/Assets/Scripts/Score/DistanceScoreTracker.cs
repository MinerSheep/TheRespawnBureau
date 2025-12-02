using UnityEngine;

public class DistanceScoreTracker : MonoBehaviour
{
    public LevelGenerator levelGen;
    private GameObject lastTrackedChunk;
    private float cumulativeDistance = 0f;
    private float currentChunkStartX = 0f;
    private float lastPlayerX = 0f;
    private float totalDistance = 0f;

    public float TotalDistance()
    {
        return totalDistance;
    }
    
    void Update()
    {
        if (levelGen == null || levelGen.player == null) return;
        float currentPlayerX = levelGen.player.position.x;
        float playerMovement = currentPlayerX - lastPlayerX;
        bool isWorldMovingLeft = playerMovement < -0.01f;

        GameObject currentChunk = FindAnyObjectByType<LevelGenerator>()?.FindPlayerChunk();
        if (currentChunk != null)
        {
            Chunk chunk = currentChunk.GetComponent<Chunk>();
            if (currentChunk != lastTrackedChunk)
            {
                if (lastTrackedChunk != null)
                {
                    Chunk lastChunk = lastTrackedChunk.GetComponent<Chunk>();
                    float lastChunkDistance = currentChunkStartX - lastChunk.entryPoint.position.x;
                    cumulativeDistance += lastChunkDistance;
                    Debug.Log($"Chunk Changed! Previous Chunk: {lastChunkDistance}, Cumulative Chunk: {cumulativeDistance}");
                }
                currentChunkStartX = chunk.entryPoint.position.x;
                lastTrackedChunk = currentChunk;
            }

            if (isWorldMovingLeft)
            {
                currentChunkStartX += playerMovement;
            }

            float currentChunkProgress = currentChunkStartX - chunk.entryPoint.position.x;
            totalDistance = cumulativeDistance + currentChunkProgress;

            if (ScoreManager.instance != null)
            {
                ScoreManager.instance.score = Mathf.FloorToInt(Mathf.Max(0, totalDistance));
            }
        }
        lastPlayerX = currentPlayerX;
    }
}