using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberGenerator : MonoBehaviour
{
    public static RandomNumberGenerator instance = null;
    
    private static List<int> randomNumbers;
    
    private static int currentIndex;
    
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
            Debug.Log(string.Format("Seed is {0}", seed));
            if (seed != "")
            {
                Random.InitState(seed.GetHashCode());
            }

            randomNumbers = new List<int>();

            for (int i = 0; i < 5000; i++)
            {
                randomNumbers.Add(Random.Range(01, 99));
                //Debug.Log(randomNumbers[i]);
            }

            currentIndex = 0;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }



    public int Next()
    {
        int nextNo = randomNumbers[currentIndex];
        currentIndex++;
        return nextNo;
    }

    

}
