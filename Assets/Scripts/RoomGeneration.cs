using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RoomGeneration : MonoBehaviour
{
    // Parameters for the obstacles and how they could be laid out.
    [SerializeField]
    private TileBase[] obstacleTiles;
    [SerializeField]
    private Vector2Int[] possibleObstacleSizes;
    [SerializeField]
    private GameObject[] possibleEnemies;

    [SerializeField]
    private int noOfObstacles;
    [SerializeField]
    private int noOfEnemies;

    // Parameters for the floor decorations and how they could be laid out.
    [SerializeField]
    private TileBase[] decorationTiles;
    [SerializeField]
    private Vector2Int[] possibleDecorationSizes;
    [SerializeField]
    private int noOfDecorations;

    // The tilemap references that will be randomly added to.
    private Tilemap obstacleTilemap;
    private Tilemap decorationTilemap;

    private Room currentRoom;

    private GameObject randomGen;

    private Text roomText;

    // Start is called before the first frame update
    void Start()
    {
        this.currentRoom = new Room();
        Grid roomObject = GetComponent<Grid>();
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        this.currentRoom.PopulatePrefabs(this.noOfEnemies, this.possibleEnemies);

        obstacleTilemap = tilemaps[2];
        this.currentRoom.PopulateObstacles(this.noOfObstacles, this.possibleObstacleSizes);
        this.currentRoom.AddPopulationToTilemap(obstacleTilemap, this.obstacleTiles);

        decorationTilemap = tilemaps[3];
        this.currentRoom.PopulateDecorations(this.noOfDecorations, this.possibleDecorationSizes);
        this.currentRoom.AddDecorationToTilemap(decorationTilemap, this.decorationTiles);

        // Increment the room count and display it in the GUI.
        roomText = GameObject.Find("RoomText").GetComponent<Text>();
        Stats.RoomCount++;
        roomText.text = "Room " + Stats.RoomCount;
    }

    
    
}
