using UnityEngine;


public class CollisionDetection : MonoBehaviour
{
    // Script references
    private PlayerController playerControllerScript;
    // Layer masks that we want to check collisions on.
    public LayerMask collisionCheckLayerMasks; 
    
    private void Awake()
    {
        playerControllerScript = GetComponent<PlayerController>();
        collisionCheckLayerMasks = LayerMask.GetMask("NonWalkable");
    }

    //CheckForCollision is called from PlayerController.UpdateTargetPosition(). It is called everytime player moves character to cast a ray that detects for a collison on a nonwalkable tile.
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
                
            }
        }
    }
}
