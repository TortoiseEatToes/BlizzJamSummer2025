using UnityEngine;

public class TileGenerator : MonoBehaviour {
  [Header("Tile Generation Settings")]
  [SerializeField] private GameObject tilePrefab;
  [SerializeField] private int gridWidth = 64;
  [SerializeField] private int gridHeight = 64;
  [SerializeField] private float tileSize = 1f;
  [SerializeField] private Vector3 startPosition = Vector3.zero;
  
  [Header("Tile Variations")]
  [SerializeField] private GameObject[] tileVariations;
  
  [Header("Generation Options")]
  [SerializeField] private bool generateOnStart = true;
  [SerializeField] private bool useRandomTiles = false;
  [SerializeField] private bool createParentObject = true;
  
  private GameObject gridParent;
  
  void Start() {
    if (generateOnStart) {
      GenerateTileGrid();
    }
  }
  
  public void GenerateTileGrid() {
    ClearExistingTiles();
    
    if (createParentObject) {
      gridParent = new GameObject("TileGrid");
      gridParent.transform.position = startPosition;
    }
    
    // Generate tiles
    for (int x = 0; x < gridWidth; x++) {
      for (int z = 0; z < gridHeight; z++) {
        CreateTile(x, z);
      }
    }
    
    Debug.Log($"Generated {gridWidth * gridHeight} tiles in a {gridWidth}x{gridHeight} grid");
  }
  
  private void CreateTile(int x, int z) {
    Vector3 tilePosition = startPosition + new Vector3(x * tileSize, 0, z * tileSize);
    
    GameObject selectedPrefab = tilePrefab;
    if (useRandomTiles && tileVariations != null && tileVariations.Length > 0) {
      selectedPrefab = tileVariations[Random.Range(0, tileVariations.Length)];
    }

    var rotation = Quaternion.Euler(90, 0, 0);
    
    GameObject tile = Instantiate(selectedPrefab, tilePosition, rotation);
    
    if (createParentObject && gridParent != null) {
      tile.transform.SetParent(gridParent.transform);
    }
    
    tile.name = $"Tile_{x}_{z}";
  }
  
  public void ClearExistingTiles() {
    if (gridParent != null) {
      DestroyImmediate(gridParent);
    } else {
      GameObject[] existingTiles = GameObject.FindGameObjectsWithTag("Tile");
      foreach (GameObject tile in existingTiles) {
        DestroyImmediate(tile);
      }
    }
  }
  
  public void RegenerateGrid() {
    GenerateTileGrid();
  }
  
  void Update() {
  }
  
  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.yellow;
    Vector3 gridSize = new Vector3(gridWidth * tileSize, 0, gridHeight * tileSize);
    Gizmos.DrawWireCube(startPosition + gridSize * 0.5f, gridSize);
  }
}
