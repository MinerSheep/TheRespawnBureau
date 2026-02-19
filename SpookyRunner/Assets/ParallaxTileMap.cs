using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxTileMap : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 1f;
    [SerializeField] private Camera cam;
    [SerializeField] private float bufferViewport = 0.1f;

    private Tilemap tilemap;
    private float mapWidth;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        mapWidth = tilemap.localBounds.size.x;
    }

    void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // Left edge of the screen (with buffer)
        Vector3 leftEdgeWorld = cam.ViewportToWorldPoint(
            new Vector3(-bufferViewport, 0.5f, 0f)
        );

        // Right edge of the screen (with buffer)
        Vector3 rightEdgeWorld = cam.ViewportToWorldPoint(
            new Vector3(1f + bufferViewport, 0.5f, 0f)
        );

        float tilemapRightEdge = transform.position.x + mapWidth * 0.5f;

        // When fully past the left side…
        if (tilemapRightEdge < leftEdgeWorld.x)
        {
            // Move so its LEFT edge sits just past the right side
            transform.position = new Vector3(
                rightEdgeWorld.x + mapWidth * 0.5f,
                transform.position.y,
                transform.position.z
            );
        }
    }
}