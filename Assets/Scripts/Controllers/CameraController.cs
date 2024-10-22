using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    void Start()
    {
        player = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z); // Follow the player, by setting camera position to equal player position.
    }
}
