using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public PlayerController playerControllerScript;
    public LayerMask collisionCheckLayerMasks; //layer masks that we want to check collisions on.

    private void Awake()
    {
        playerControllerScript = GetComponent<PlayerController>();
        collisionCheckLayerMasks = LayerMask.GetMask("NonWalkable") | LayerMask.GetMask("Door");
    }


    //CheckForCollision is called from PlayerController.UpdateTargetPosition(). It is called everytime player moves character to cast a ray that detects for a collison in a list of layer masks.
    public void CheckForCollision(Vector2 moveDirection)
    {
        //Debug.DrawRay(transform.position, moveDirection, Color.magenta);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, 1f, collisionCheckLayerMasks); //Cast a ray 

        if (hit.collider != null)
        {
            //NonWalkable layer check
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NonWalkable"))
            {
                playerControllerScript.isWalkable = false;
                Debug.Log("hit");
            }
            //Door layer check
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Door"))
            {
                Debug.Log("Door hit, loading new scene " + hit.collider.name);
            }
        }
    }
}
