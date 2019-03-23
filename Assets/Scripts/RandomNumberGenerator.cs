// Generates all random numbers to be used throughout the game, whether from a seed or otherwise.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour
{
    public static RandomNumberGenerator instance = null;
    
    // Two seperate lists: one for dealing with item rarity, the other for everything else. 
    // A seperate list for item rarity was simpler for dealing with the relative probablity. 
    private static List<int> randomNumbers;
    private static List<int> itemRarity;
    
    // Mark where the game currently is in using both lists.
    private static int currentRandomIndex;
    private static int currentItemIndex;
    
    [SerializeField]
    private string seed = "";

    // Start is called before the first frame update
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
            if (seed != "")
            {
                // Seeds based on entering words and converting them to a hash. 
                Random.InitState(seed.GetHashCode());
            }

            randomNumbers = new List<int>();

            for (int i = 0; i < 5000; i++)
            {
                randomNumbers.Add(Random.Range(11, 78));
            }

            itemRarity = new List<int>();

            for (int i = 0; i < 5000; i++)
            {
                itemRarity.Add(Random.Range(0, 12)); // 8 = common, 3 = rare, 1 = legendary. 
            }

            currentRandomIndex = 0;
            currentItemIndex = 0;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }


    // Retrieve the next random number.
    public int Next()
    {
        int nextNo = randomNumbers[currentRandomIndex];
        currentRandomIndex++;

        // If somehow reached the end of all the random numbers generated, roll some more. 
        if (currentRandomIndex == randomNumbers.Count - 1)
        {
            currentRandomIndex = 0;
            randomNumbers.Clear();
            for (int i = 0; i < 5000; i++)
            {
               
                randomNumbers.Add(Random.Range(11, 78));
                
            }
        }

        return nextNo;
    }

    // Retrieve the next item probabilty. 
    public int NextItem()
    {
        int nextNo = itemRarity[currentItemIndex];
        currentItemIndex++;

        // If somehow reached the end of all the random numbers generated, roll some more. 
        if (currentItemIndex == itemRarity.Count - 1)
        {
            currentItemIndex = 0;
            itemRarity.Clear();
            for (int i = 0; i < 5000; i++)
            {
                itemRarity.Add(Random.Range(0, 12)); // 8 = common, 3 = rare, 1 = legendary. 
            }
        }

        return nextNo;
    }
    

}
