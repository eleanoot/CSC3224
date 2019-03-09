using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // If any items are active on the board, disable them.
            if (Stats.active != null)
                Stats.active.gameObject.SetActive(false);
            SceneManager.LoadScene("Runner");
        }
    }
}
