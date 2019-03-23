// Script for when the player reaches the top of the room and should move on. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Don't activate scene change by an enemy attack!
        if (collision.gameObject.tag == "Player")
        {
            // If any items are active on the board, disable them.
            if (Stats.active != null)
                Stats.active.gameObject.SetActive(false);
            // Reload the room to generate a new one. 
            SceneManager.LoadScene("Runner");
        }
    }
}
