using UnityEngine;

public class MapGen : MonoBehaviour
{
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float tileWidth = 1f;   // Adjust based on your tile's width
    [SerializeField] private float tileHeight = 0.5f;  // Adjust based on your tile's height
    [SerializeField] private GameObject isometricTile;

    private void Awake()
    {
        GenerateMap();
        CreateDiamondEdgeCollider();
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Convert grid coordinates to isometric world space.
                float isoX = (x - y) * (tileWidth / 2f);
                float isoY = (x + y) * (tileHeight / 2f);

                // Instantiate the tile at the computed position.
                GameObject tile = Instantiate(isometricTile, new Vector3(isoX, isoY, 0), Quaternion.identity, transform);

                // Set sorting order so that tiles further down are rendered on top.
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = -(x + y);
                }
            }
        }
    }

    /// <summary>
    /// Creates a diamond-shaped EdgeCollider2D that outlines 
    /// the perimeter (top face) of the isometric tilemap.
    /// </summary>
    void CreateDiamondEdgeCollider()
    {
        // Add an EdgeCollider2D to this same GameObject.
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Compute the four diamond corners.
        // Here, we want the boundaries to exactly enclose the tilemap area.
        // We adjust the indices so that the boundary lies just along the outer edge.
        // Using the same isometric formula but extending one tile length as needed.

        // Bottom corner: corresponds to grid coordinate (0, 0)
        Vector2 bottomCorner = new Vector2(
            (0 - 0) * (tileWidth / 2f),
            (0 + 0) * (tileHeight / 2f)
        );

        // Right corner: corresponds roughly to grid coordinate (mapWidth, 0)
        Vector2 rightCorner = new Vector2(
            ((mapWidth) - 0) * (tileWidth / 2f),
            ((mapWidth) + 0) * (tileHeight / 2f)
        );

        // Top corner: corresponds roughly to grid coordinate (mapWidth, mapHeight)
        Vector2 topCorner = new Vector2(
            ((mapWidth) - (mapHeight)) * (tileWidth / 2f),
            ((mapWidth) + (mapHeight)) * (tileHeight / 2f)
        );

        // Left corner: corresponds roughly to grid coordinate (0, mapHeight)
        Vector2 leftCorner = new Vector2(
            (0 - (mapHeight)) * (tileWidth / 2f),
            (0 + (mapHeight)) * (tileHeight / 2f)
        );

        // To close the loop for the EdgeCollider2D, we repeat the first point at the end.
        Vector2[] edgePoints = new Vector2[]
        {
            bottomCorner,
            rightCorner,
            topCorner,
            leftCorner,
            bottomCorner  // repeat the first point to close the loop
        };

        edgeCollider.points = edgePoints;
        edgeCollider.edgeRadius = 0.1f;

        // Optionally, set edgeCollider.edgeRadius to give the line some thickness.
        // For example:
        // edgeCollider.edgeRadius = 0.1f;
    }
}
