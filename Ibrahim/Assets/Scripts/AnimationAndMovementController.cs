using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimationAndMovementController : MonoBehaviour
{

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;

    bool isMovementPressed;
    bool isRunPressed;

    MyControls playerInput;
    CharacterController characterController;
    Animator animator;

    public float rotationFactorPerFrame = 20.0f;
    public float walkMiltiplier = 1.0f;
    public float runMultiplier = 3.0f;

    bool isAttackPressed = false;
    bool canCombo = true;
    bool attackFlag = false;


    private void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        playerInput = new MyControls();
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Attack.started += onAttack;
        playerInput.CharacterControls.Attack.canceled += onAttack;
    }

    void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }

    void enableCombo()
    {
        canCombo = true;
    }

    void resetCombo()
    {
        canCombo = false;
        animator.ResetTrigger("isAttacking");
    }
    void isAttacking()
    {
        attackFlag = true;
    }
    void stoppedAttacking()
    {
        attackFlag = false;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkMiltiplier;
        currentMovement.z = currentMovementInput.y * walkMiltiplier;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
    }

    void handleRotation()
    {
        Quaternion currentRotation = transform.rotation;
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            currentRunMovement.y += gravity;
            currentMovement.y += gravity;
        }
    }



    void handleAnimation()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (isMovementPressed && !isWalking && !isRunPressed)
        {
            animator.SetBool("isWalking", true);
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool("isWalking", false);
        }
        if ((isRunPressed && isMovementPressed) && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else if ((!isRunPressed || !isMovementPressed) && isRunning)
        {
            animator.SetBool("isRunning", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleAnimation();
        if (!attackFlag)
        {
            if (isRunPressed)
            {
                characterController.Move(currentRunMovement * Time.deltaTime);
            }
            else
            {
                characterController.Move(currentMovement * Time.deltaTime);
            }

            handleRotation();

        }
        if (isAttackPressed && canCombo)
        {
            animator.SetTrigger("isAttacking");
        }
        Debug.Log(attackFlag);

    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
