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

        // Increment the room count and display it in the GUI.
        roomText = GameObject.Find("RoomText").GetComponent<Text>();
        Stats.RoomCount++;
        roomText.text = "Room " + Stats.RoomCount;

        // Increment active charge count from passing into another rooms.
        Stats.CurrentCharge++;

        if (Stats.RoomCount % 5 != 0)
        {
            this.currentRoom.PopulateEnemies(this.noOfEnemies, this.possibleEnemies);

            obstacleTilemap = tilemaps[2];
            this.currentRoom.PopulateObstacles(this.noOfObstacles, this.possibleObstacleSizes);
            this.currentRoom.AddPopulationToTilemap(obstacleTilemap, this.obstacleTiles);
        }
        else // Item room. No enemies, just three items randomly rolled to choose from in the same positions every time. 
        {
            // Spawn an item room every 5 rooms (may change).
            // eventually randomise 3 items and display in the middle with flavourtext when nearby. 
            //Instantiate(Resources.Load("Items/FairyDust"));

            GameObject[] chosenItems = new GameObject[3];
            chosenItems[0] = ItemManager.instance.allItems[0];
            chosenItems[1] = ItemManager.instance.allItems[1];
            chosenItems[2] = ItemManager.instance.allItems[2];

            this.currentRoom.PopulateItems(chosenItems);
            obstacleTilemap = tilemaps[2];
            this.currentRoom.AddPopulationToTilemap(obstacleTilemap, this.obstacleTiles);

            

        }
        
        decorationTilemap = tilemaps[3];
        this.currentRoom.PopulateDecorations(this.noOfDecorations, this.possibleDecorationSizes);
        this.currentRoom.AddDecorationToTilemap(decorationTilemap, this.decorationTiles);

        

        

    }

    
    
}
