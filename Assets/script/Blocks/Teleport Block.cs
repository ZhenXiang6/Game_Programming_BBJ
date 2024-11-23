using UnityEngine;
using UnityEngine.Tilemaps;

public class TeleportBlock : MonoBehaviour
{
    public Tilemap tilemap;
    public float teleportDistance = 5f;  // Distance to teleport the block above the player
    public float detectionRadius = 50f;  // Detection radius between player and block
    public float teleportInterval = 5f;  // Minimum time interval between teleports (in seconds)

    public GameObject transportOK;  // UI element to show when teleport is possible
    private float lastTeleportTime = -1f;  // Time of the last teleport
    [SerializeField] public Vector2 closestTile;  // Serialized field for closest tile position

    [SerializeField] GameObject player;
    [SerializeField] float distance = 10000;
    [SerializeField] Vector2 playerPosition;

    void Start()
    {
        lastTeleportTime = -1 * teleportInterval;
    }
    void Update()
    {
        // Find the player with the "Player" tag
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;

        if (player != null)
        {
            // Ensure teleport interval is met before teleporting
            if (Time.time - lastTeleportTime >= teleportInterval)
            {
                closestTile = FindClosestTileToPlayer();
                HandleTeleport(player);
            }
        }
    }

    private void HandleTeleport(GameObject player)
    {
        distance = Vector2.Distance(closestTile, (Vector2)player.transform.position);

        // If the player is close enough, show the teleport indicator
        if (distance <= detectionRadius)
        {
            transportOK.SetActive(true);  // Show the transport UI element

            // Check if the mouse click happens inside the area (we can use Raycasting or simply click within the screen bounds)
            if (Input.GetMouseButtonDown(0))  // Left mouse button
            {
                Vector3 newPosition = new Vector3(
                    closestTile.x,
                    closestTile.y + teleportDistance,
                    player.transform.position.z
                );
                player.transform.position = newPosition;
                lastTeleportTime = Time.time;  // Update the last teleport time

                // Hide transport OK UI after teleport
                transportOK.SetActive(false);
                Debug.Log("Teleport OK!");
            }
        }
        else
        {
            transportOK.SetActive(false);  // Hide the transport UI element when not in range
        }
    }

    Vector2 FindClosestTileToPlayer()
    {
        // Get the player's world position
        Vector3 playerWorldPosition = player.transform.position;

        // Convert the player's world position to a cell position in the Tilemap
        Vector3Int playerCellPosition = tilemap.WorldToCell(playerWorldPosition);
        float closestDistance = float.MaxValue;
        Vector2 closestTilePosition = new Vector2(10000, 10000);

        // Iterate over a grid of cells around the player within the detection radius
        for (int x = -Mathf.FloorToInt(detectionRadius); x < Mathf.FloorToInt(detectionRadius); x++)
        {
            for (int y = -Mathf.FloorToInt(detectionRadius); y < Mathf.FloorToInt(detectionRadius); y++)
            {
                Vector3Int checkCell = playerCellPosition + new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(checkCell);

                if (tile != null)
                {
                    Vector3 worldPosition = tilemap.CellToWorld(checkCell);
                    float dist = Vector2.Distance(worldPosition, playerWorldPosition);

                    // If this tile is closer, update closest tile
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestTilePosition = worldPosition;
                    }
                }
            }
        }

        // Print the closest tile position to the console
        return closestTilePosition;
    }
}
