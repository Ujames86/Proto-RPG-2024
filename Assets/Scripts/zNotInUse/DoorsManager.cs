/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// **** NO LONGER IN USED - WE USE PREFAB WITH TRIGGERS INSTEAD ***** //

// Door Manager script holds methods and variables that cache all doors in a scene, then when a player walks into a door, loads the scene the door belongs to.
// Door Manager script will live on the zonemanager gameobjects of zone scenes(Starting zone, Route 1, Tenhut City, etc)

public class DoorsManager : MonoBehaviour
{
    [SerializeField] public Doors[] doors;
    public Dictionary<Vector2, Doors> doorCache = new Dictionary<Vector2, Doors>();

    private void Awake()
    {
        
        CacheDoorsInZone();
        Debug.Log("Loaded door manager script");

    }


    //Cache door scriptable objects on zone manager for O(1) retrieval in GetDoorScene()
    private void CacheDoorsInZone()
    {
        foreach(Doors door in doors)
        {
            doorCache.Add(door.doorLocationInWorld, door);
        }
    }
    //Called from CollisionDetection.cs - Loads the scene of the door player just entered.
    public void GetDoorScene(Vector2 doorCordinates)
    {
        try
        {
            SceneManager.LoadSceneAsync(doorCache[doorCordinates].sceneName);
        }
        catch
        {
            Debug.Log(doorCache.Count);
            doorCache.Clear();
        }
        
        
    }
}*/



