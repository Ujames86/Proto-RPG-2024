using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public static CameraController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z); // Follow the player, by setting camera position to equal player position.
    }
}
