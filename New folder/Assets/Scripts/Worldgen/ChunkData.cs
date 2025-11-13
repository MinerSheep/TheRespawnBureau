using UnityEngine;

[CreateAssetMenu(fileName = "NewChunkData", menuName = "Level/Chunk Data")]
public class ChunkData : ScriptableObject
{
    [Header("Basic Info")]
    public string tagName;
    public Difficulty difficulty; // <— new enum field for difficulty
    public float weight = 1f;

    [Header("Prefab")]
    public GameObject prefab;

    [Header("Connections & Rules")]
    public string[] allowedNextTags;
}
