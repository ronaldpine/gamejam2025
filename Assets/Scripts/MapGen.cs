using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] private int mapWidth = 10;         // Island width in tiles
    [SerializeField] private int mapHeight = 10;        // Island height in tiles
    [SerializeField] private float tileWidth = 1f;        // Width of a tile (world units)
    [SerializeField] private float tileHeight = 0.5f;     // Height of a tile (world units)
    [SerializeField] private int oceanRing = 10;           // Number of ocean tile rings around the island
    [SerializeField] private GameObject isometricTile;    // Prefab used for island tiles
    [SerializeField] private GameObject oceanTile;        // Prefab used for ocean (sea) tiles

    private void Awake()
    {
        Debug.Log("This is the ocean ring " + oceanRing);
        GenerateMap();
        CreateDiamondEdgeCollider();
    }

    /// <summary>
    /// Generates the island surrounded by an ocean boundary.
    /// The loop runs from -oceanRing to mapWidth+oceanRing (and similarly for y)
    /// so that the outer ring (or rings) becomes the ocean border.
    /// </summary>
    void GenerateMap()
    {
        // Loop ranges for x and y include the extra rings.
        for (int x = -oceanRing; x < mapWidth + oceanRing; x++)
        {
            for (int y = -oceanRing; y < mapHeight + oceanRing; y++)
            {
                // Determine if the current grid coordinate falls within the island.
                bool isIslandTile = (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight);
                GameObject tilePrefab = isIslandTile ? isometricTile : oceanTile;

                // Convert grid coordinates to isometric world space.
                float isoX = (x - y) * (tileWidth / 2f);
                float isoY = (x + y) * (tileHeight / 2f);

                // Instantiate the tile at the computed position and parent it to this GameObject.
                GameObject tile = Instantiate(tilePrefab, new Vector3(isoX, isoY, 0), Quaternion.identity, transform);

                // Set sorting order so that island tiles render properly relative to each other.
                // Ocean tiles get an extra offset so they render behind the island.
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (isIslandTile)
                        sr.sortingOrder = -(x + y);
                    else
                        sr.sortingOrder = -(x + y) - 100;
                }
            }
        }
    }

    /// <summary>
    /// Creates a diamond-shaped EdgeCollider2D around the island.
    /// An EdgeCollider2D only creates a collision line (not a filled shape)
    /// so that characters can be inside the boundary while being prevented
    /// from leaving the island's top face.
    /// </summary>
    void CreateDiamondEdgeCollider()
    {
        // Add an EdgeCollider2D to this GameObject.
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Calculate the four corners of the island's outer face, based on its grid.
        // Note: We use the island boundaries (0 to mapWidth/mapHeight) even though the ocean extends further.
        // Bottom corner corresponds to grid coordinate (0, 0)
        Vector2 bottomCorner = new Vector2(
            (0 - 0) * (tileWidth / 2f),
            (0 + 0) * (tileHeight / 2f)
        );

        // Right corner corresponds roughly to grid coordinate (mapWidth, 0)
        Vector2 rightCorner = new Vector2(
            (mapWidth - 0) * (tileWidth / 2f),
            (mapWidth + 0) * (tileHeight / 2f)
        );

        // Top corner corresponds roughly to grid coordinate (mapWidth, mapHeight)
        Vector2 topCorner = new Vector2(
            (mapWidth - mapHeight) * (tileWidth / 2f),
            (mapWidth + mapHeight) * (tileHeight / 2f)
        );

        // Left corner corresponds roughly to grid coordinate (0, mapHeight)
        Vector2 leftCorner = new Vector2(
            (0 - mapHeight) * (tileWidth / 2f),
            (0 + mapHeight) * (tileHeight / 2f)
        );

        // Define the diamond shape; repeat the first point to close the loop.
        Vector2[] edgePoints = new Vector2[]
        {
            bottomCorner,
            rightCorner,
            topCorner,
            leftCorner,
            bottomCorner  // Repeating to close the loop.
        };

        edgeCollider.points = edgePoints;
        edgeCollider.edgeRadius = 0.1f;
    }
}
