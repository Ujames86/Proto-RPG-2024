using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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
