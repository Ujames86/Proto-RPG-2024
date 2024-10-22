using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    // ################################## //
    //   Global Variables    //
    // ################################## //

    //Movement variables
    public PlayerControlsInputs playerControlsInputs; // Input actions asset that maps keys to bindings
    [SerializeField] private float moveSpeed = 3f;
    private Vector2 moveDirectionInput; // Always equal to a Vector2(ie . (-1,0)) based on key (WASD or Arrows) player presses
    private Vector2 targetPosition; // Always equal to players current position + moveDirectionInput, used to move player
    private bool isMoving;

    //Collision Variables - set from CollisionDetection.cs
    public bool isWalkable;
    [SerializeField] private CollisionDetection collisionDetectionScript;

    //Animation varaiables (Movement)
    private Animator animator;

    // ################################## //
    //   END - Global Variables    //
    // ################################## //



    private void Awake()
    {
        //Movement 
        playerControlsInputs = new PlayerControlsInputs();
        targetPosition = transform.position; // Anytime the scene is loaded, targetPosition needs to be set to where current player is in order to have moveDirectionInput + targetPosition lead to the correct next tile.
        isMoving = false;

        //Collision
        isWalkable = true;
        
        //Movement Animations
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        collisionDetectionScript = GetComponent<CollisionDetection>();
    }

    private void OnEnable()
    {
        playerControlsInputs.BaseGameplay.Enable(); //Enable entire BaseGameplay action map for use - https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/Actions.html#using-actions
        playerControlsInputs.BaseGameplay.Movement.performed += MoveKeyPressed;
        playerControlsInputs.BaseGameplay.Movement.canceled += MoveKeyReleased;
    }
    private void OnDisable()
    {
        playerControlsInputs.BaseGameplay.Disable();
        playerControlsInputs.BaseGameplay.Movement.performed -= MoveKeyPressed;
        playerControlsInputs.BaseGameplay.Movement.canceled -= MoveKeyReleased;
    }
    private void Update()
    {
        MovePlayer();
    }



    // ################################## //
    //   Player Movement Functions     //
    // ################################## //


    
    private void MoveKeyPressed(InputAction.CallbackContext context)
    {
        moveDirectionInput = context.ReadValue<Vector2>(); //Input from WASD or arrows are clicked/held down
    }
    
    private void MoveKeyReleased(InputAction.CallbackContext context)
    {
        moveDirectionInput = Vector2.zero; // Direction set to zero when WASD or arrow key is not being clicked/Held down
    }


    
    private void MovePlayer()
    {
        UpdateTargetPosition(); // Check to see if their is an updated targetPosition to moveplayer to.


        if (isMoving) // If there is an updated targetPosition, move player until they reach next tile.
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
            {
                isMoving = false;
                animator.SetBool("isMoving", false);
                transform.position = targetPosition; //snap to grid type of help, removed for now need to test it again later if its too clunky to walk
            }
        }
    }

    private void UpdateTargetPosition()
    {
        //Remove Diagonal Movement
        if (moveDirectionInput.x != 0 && moveDirectionInput.y != 0)
        {
            moveDirectionInput.x = 0;
            if (moveDirectionInput.y > 0)
            {
                moveDirectionInput.y = 1;
            }
            else if (moveDirectionInput.y < 0)
            {
                moveDirectionInput.y = -1;
            }
        }

        // Update target position when player is not currently moving and there is input.
        if (moveDirectionInput != Vector2.zero && isMoving == false)
        {
            collisionDetectionScript.CheckForCollision(moveDirectionInput);
            if (isWalkable)
            {
                targetPosition += moveDirectionInput;
                isMoving = true;
                animator.SetBool("isMoving", true);
                animator.SetFloat("InputX", moveDirectionInput.x);
                animator.SetFloat("InputY", moveDirectionInput.y);
            }
            else
            {
                isWalkable = true;
                animator.SetBool("isMoving", false);
                animator.SetFloat("InputX", moveDirectionInput.x);
                animator.SetFloat("InputY", moveDirectionInput.y);
            }

        }
    }




}
