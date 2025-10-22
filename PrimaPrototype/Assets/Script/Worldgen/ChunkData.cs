using UnityEngine;

[CreateAssetMenu(fileName = "NewChunkData", menuName = "Level/Chunk Data")]
public class ChunkData : ScriptableObject
{
    public GameObject prefab;    // The actual prefab to spawn
    public float weight = 1f;    // How often it appears
    public string difficulty = "Easy";  // Category
    public string[] allowedNextTags;    // What can follow this chunk
    public string tagName;              // Type tag (for rule chaining)
}
