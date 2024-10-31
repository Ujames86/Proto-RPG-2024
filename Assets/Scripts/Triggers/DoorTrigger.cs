using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// DoorTrigger is set on box colliders with is trigger enabled, on door tilemaps in the world and in houses. When triggered, it loads the scene associated with the door scriptanble object attached to the collider.
// Then it sends an event where playercontroller is subscribed to for position and movement once in the new scene
public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Doors door;
    public static event Action<Vector2, int> SceneTransitionSetPlayerPosition;
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadSceneAsync(door.sceneName);
        SceneTransitionSetPlayerPosition?.Invoke(door.loadIntoNextSceneLocation, door.idleAnimationDirectionOnSceneChange);
    }

}
