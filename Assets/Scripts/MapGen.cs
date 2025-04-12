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

                // Instantiate the tile and parent it to this object for organization.
                GameObject tile = Instantiate(isometricTile, new Vector3(isoX, isoY, 0), Quaternion.identity, transform);

                // Adjust the sorting order based on tile position.
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    // The tile that is further down (a higher x+y value) should render in front.
                    sr.sortingOrder = -(x + y);
                }
            }
        }
    }
}
