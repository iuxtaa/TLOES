using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrevMap : MonoBehaviour
{
    private const float CHECKPOINT_OFFSET = 3f;
    private bool hasCollided = false;
    [SerializeField] private Transform player;
    [SerializeField] private Transform prevSceneTravelCheckpoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && !hasCollided)
        {
            hasCollided = true;
            Invoke("CompleteLevel", 1.5f);
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        player.transform.position = new Vector3(prevSceneTravelCheckpoint.position.x - CHECKPOINT_OFFSET, player.position.y, player.position.z);
    }
}
