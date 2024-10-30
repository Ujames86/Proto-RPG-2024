using UnityEngine;

[CreateAssetMenu(fileName = "Doors", menuName = "Scriptable Objects/Doors", order = 1)]
public class Doors : ScriptableObject
{
    
    public string sceneName;
    public int sceneIndex;

    public string doorName;
    [TextArea(3, 10)]
    public string doorDescription;


    public Vector2 loadIntoNextSceneLocation;
    public int idleAnimationDirectionOnSceneChange;

    public bool hasSpecialBehaviour;
}
