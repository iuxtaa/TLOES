using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string sceneToLoad;
    public Vector2 newPlayerPosition;
    public VectorValue playerPosition;
    private const float INVOKE_OFFSET = 0.3f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !collision.isTrigger)
        {
            playerPosition.changingValue = newPlayerPosition;
            Invoke("TeleportToMap", INVOKE_OFFSET);
        }
    }

    private void TeleportToMap()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
} 
