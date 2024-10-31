using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // ################################## //
    //   Global Variables    //
    // ################################## //

    //Movement variables
    public PlayerControlsInputs playerControlsInputs; // Input actions asset that maps keys to bindings
    [SerializeField] private float moveSpeed = 3f;
    public Vector2 moveDirectionInput; // Always equal to a Vector2(ie . (-1,0)) based on key (WASD or Arrows) player presses
    public Vector2 targetPosition; // Always equal to players current position + moveDirectionInput, used to move player
    public bool isMoving;

    //Collision Variables - set from CollisionDetection.cs
    [SerializeField] public bool isWalkable;
    [SerializeField] private CollisionDetection collisionDetectionScript;

    //Animation varaiables (Movement)
    private Animator animator;

    //Scene Variables
    private Vector2 startPositionOfNewScene;
    private int idleAnimationDirection;
   


    // ################################## //
    //   END - Global Variables    //
    // ################################## //



    private void Awake()
    {
        //Movement 
        playerControlsInputs = new PlayerControlsInputs();
        startPositionOfNewScene = transform.position;
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
        // Input Controls events
        playerControlsInputs.BaseGameplay.Enable(); //Enable entire BaseGameplay action map for use - https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/Actions.html#using-actions
        playerControlsInputs.BaseGameplay.Movement.performed += MoveKeyPressed;
        playerControlsInputs.BaseGameplay.Movement.canceled += MoveKeyReleased;
        // Trigger Events
        SceneManager.sceneLoaded += OnSceneLoaded; // Unity static Event that is triggered when a scene is loaded, calls SetPositionOnDoorEnterOrExit() to set player position in new scene.
        DoorTrigger.SceneTransitionSetPlayerPosition += SetPositionVariablesOnDoorEnterOrExit; // Sets the variables startPositionOfNewScene and idleAnimationDirection for when player goes into new scene.
    }
    private void OnDisable()
    {
        // Input Controls events
        playerControlsInputs.BaseGameplay.Disable();
        playerControlsInputs.BaseGameplay.Movement.performed -= MoveKeyPressed;
        playerControlsInputs.BaseGameplay.Movement.canceled -= MoveKeyReleased;
        // Trigger Events
        SceneManager.sceneLoaded -= OnSceneLoaded;
        DoorTrigger.SceneTransitionSetPlayerPosition -= SetPositionVariablesOnDoorEnterOrExit;
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

            if (Vector3.Distance(transform.position, targetPosition) <= 0.01f) //Once player reaches next tile, stop player from moving and snap sprite to grid to ensure grid based movement.
            {
                isMoving = false;
                animator.SetBool("isMoving", false);
                transform.position = targetPosition; //snap to grid
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
            collisionDetectionScript.CheckForCollision(moveDirectionInput); // Detect if player ran into a building, door, etc and respond accordingly.
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


    // ################################## //
    //   Event Subscription Functions    //
    // ################################## //

    // Whenever a scene loads via a door, updates players start position in new scene and which idle animation direction they face, 
    private void SetPositionVariablesOnDoorEnterOrExit(Vector2 _startPositionOfNewScene, int _idleAnimationDirection)
    {
        startPositionOfNewScene = _startPositionOfNewScene;
        idleAnimationDirection = _idleAnimationDirection; 
    }
    //Whenever a scene(or the game) is loaded, call a function to set player position and targetposition. - This also stops the bug where player loads before the new scene does.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            if (scene.isLoaded)
            {
                SetPositionOfPlayer();
            }
        }
        catch
        {
            Debug.Log("scene did not load correctly, function to set player position in new scene not called");
        }
    }

    //Set position of player and targetposition of player.
    private void SetPositionOfPlayer()
    {
        isMoving = false;
        transform.position = startPositionOfNewScene;
        targetPosition = startPositionOfNewScene;

        animator.SetBool("isMoving", false);
        animator.SetFloat("InputX", 0);
        animator.SetFloat("InputY", idleAnimationDirection);
    }



}
