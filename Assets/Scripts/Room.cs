using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room
{
    // Represent the contents of the fillable grid with a 2D array of ID values.
    private string[,] population;

    public Room()
    {
        // Initialise the grid.
        this.population = new string[10, 10];
        for (int xIndex = 0; xIndex < 10; xIndex++)
        {
            for (int yIndex = 0; yIndex < 10; yIndex++)
            {
                this.population[xIndex, yIndex] = "";
            }
        }
        
    }

    // Add obstacle tiles to the tile positions marked by the grid.
    public void AddPopulationToTilemap(Tilemap tilemap, TileBase[] obstacleTiles)
    {
        for (int xIndex = 0; xIndex < 10; xIndex++)
        {
            for (int yIndex = 0; yIndex < 10; yIndex++)
            {
                if (this.population[xIndex, yIndex] == "Obstacle")
                {
                    // Choose a random tile from the obstacles to set here.
                    int r = Random.Range(0, obstacleTiles.Length);
                    TileBase obstacleTile = obstacleTiles[r];
                    // Need to subtract 4 as grid matrix (0,0) is lower left corner, but tilemap (0,0) is the centre.
                    tilemap.SetTile(new Vector3Int(xIndex - 4, yIndex - 4, 0), obstacleTile);
                }
            }
        }
    }

    // Add decoration tiles to the tile positions marked by the grid.
    public void AddDecorationToTilemap(Tilemap tilemap, TileBase[] decorationTiles)
    {
        for (int xIndex = 0; xIndex < 10; xIndex++)
        {
            for (int yIndex = 0; yIndex < 10; yIndex++)
            {
                if (this.population[xIndex, yIndex] == "Decoration")
                {
                    // Choose a random tile from the obstacles to set here.
                    int r = Random.Range(0, decorationTiles.Length);
                    TileBase decorationTile = decorationTiles[r];
                    tilemap.SetTile(new Vector3Int(xIndex - 4, yIndex - 4, 0), decorationTile);
                }
            }
        }
    }

    // Iterate through each chosen tile to check if they're still available for placement. 
    private bool IsFree(List<Vector2Int> region)
    {
        foreach (Vector2Int tile in region)
        {
            if (this.population[tile.x, tile.y] != "")
            {
                return false;
            }
        }
        return true;
    }

    // Look for random areas on the grid until one is available to place tiles in. 
    private List<Vector2Int> FindFreeRegion(Vector2Int sizeInTiles)
    {
        List<Vector2Int> region = new List<Vector2Int>();
        // Continue until a valid region for this tile set is found. 
        do
        {
            region.Clear();

            // Generate random centre for the tiles between the valid tiles in the full map (i.e. not the outside walls or entrance/exit rows)
            Vector2Int centerTile = new Vector2Int(UnityEngine.Random.Range(1, 8), UnityEngine.Random.Range(1, 8));

            region.Add(centerTile);

            // Calculate the coordinates of the rest of the tile set from the given size.
            int initialXCoordinate = (centerTile.x - (int)Mathf.Floor(sizeInTiles.x / 2));
            int initialYCoordinate = (centerTile.y - (int)Mathf.Floor(sizeInTiles.y / 2));
            for (int xCoordinate = initialXCoordinate; xCoordinate < initialXCoordinate + sizeInTiles.x; xCoordinate++)
            {
                for (int yCoordinate = initialYCoordinate; yCoordinate < initialYCoordinate + sizeInTiles.y; yCoordinate++)
                {
                    region.Add(new Vector2Int(xCoordinate, yCoordinate));
                }
            }
        } while (!IsFree(region));
        return region;
    }

    // Choose a random size for this obstacle set and place it in the grid matrix.
    public void PopulateObstacles(int numberOfObstacles, Vector2Int[] possibleSizes)
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            int sizeIndex = Random.Range(0, possibleSizes.Length);
            Vector2Int regionSize = possibleSizes[sizeIndex];
            List<Vector2Int> region = FindFreeRegion(regionSize);
            foreach (Vector2Int coord in region)
            {
                this.population[coord.x, coord.y] = "Obstacle";
            }
        }
    }

    // Choose a random size for this decoration set and place it in the grid matrix.
    public void PopulateDecorations(int numberOfDecorations, Vector2Int[] possibleSizes)
    {
        for (int i = 0; i < numberOfDecorations; i++)
        {
            int sizeIndex = Random.Range(0, possibleSizes.Length);
            Vector2Int regionSize = possibleSizes[sizeIndex];
            List<Vector2Int> region = FindFreeRegion(regionSize);
            foreach (Vector2Int coord in region)
            {
                this.population[coord.x, coord.y] = "Decoration";
            }
        }
    }
}
