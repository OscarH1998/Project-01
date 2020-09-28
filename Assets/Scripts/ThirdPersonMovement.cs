using System.Collections;


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[RequireComponent (typeof(Collider))]
public class ThirdPersonMovement : MonoBehaviour
{

    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartJumping = delegate { };
    public event Action StartFalling = delegate { };
    public event Action StartTeleport = delegate { };

    public CharacterController controller;
    public Transform cam;

    [SerializeField] Transform rayStart = null;
    [SerializeField] GameObject player = null;
    [SerializeField] float _teleportCheck = 12f;
    [SerializeField] float distance = 8f;
    public float radius;
    public AudioSource teleport = null;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;

    public float sprintspeed;
    public MovementState MovementType;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    bool _isMoving = false;
    bool _isJumping = false;
    bool _isTeleport = false;

    private float jumpCooldown = 0f;
    private float telCooldown = 0f;

    public enum MovementState
    {
        Idle,Running,Sprinting,Jumping
    }
    
    private void Start()
    {
        Idle?.Invoke();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && Time.time > jumpCooldown)
        {
            jumpCooldown = Time.time + 3f;
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            _isJumping = true;
            CheckIfStartedJumping();
            StartCoroutine(JumpToFall());
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        else
        {
            CheckIfStoppedMoving();
        }
        
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > jumpCooldown)
        {
            _isTeleport = true;
            OnTeleport();
            CheckIfStartedTeleport();
            //jumpCooldown = Time.time + 3f;
            StartCoroutine(TeleToRun());
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Sprint();
        }

        else
        {
            speed = 6f;
        }

        if (Input.GetKey(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
   
    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MovementType = MovementState.Sprinting;
            speed = sprintspeed;
        }

        else
        {
            if (_isMoving == false)
            {
                MovementType = MovementState.Idle;
            }

            if (_isMoving == true)
            {
                MovementType = MovementState.Running;
            }
        }
    }

    private void OnTeleport()
    {
        RaycastHit hit;
        Vector3 destination = controller.transform.position + controller.transform.forward * distance;

        if (Physics.Linecast(controller.transform.position, destination, out hit))
        {
            destination = controller.transform.position + controller.transform.forward * (hit.distance - 1f);
        }

        if (Physics.Raycast(destination, -Vector3.up, out hit))
        {
            destination = hit.point;
            destination.y = 0.5f;
            transform.position = destination;
        }

        Vector3 shockwavePos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(shockwavePos, radius);
        teleport.Play();

    }

    IEnumerator TeleToRun()
    {
        yield return new WaitForSeconds(.8f);
        CheckIfStoppedMoving();
        CheckIfStartedMoving();
    }

    IEnumerator JumpToFall()
    {
        yield return new WaitForSeconds(1.5f);
        CheckIfStartedFalling();
        StartCoroutine(FallToRun());
    }

    IEnumerator FallToRun()
    {
        yield return new WaitForSeconds(0.7f);
        CheckIfStoppedMoving();
        CheckIfStartedMoving();
    }


    private void CheckIfStartedMoving()
    {
        if (_isMoving == false)
        {
            StartRunning?.Invoke();
            Debug.Log("Started");
        }

        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }

        _isMoving = false;
    }

    private void CheckIfStartedJumping()
    {
        if (_isJumping == true)
        {
            StartJumping?.Invoke();
            Debug.Log("Jumped");
        }
        _isJumping = false;
    }

    private void CheckIfStartedFalling()
    {
        if (_isJumping == false)
        {
            StartFalling?.Invoke();
            Debug.Log("Falling");
        }
    }
   
    private void CheckIfStartedTeleport()
    {
        if (_isTeleport == true)
        {
            StartTeleport?.Invoke();
            Debug.Log("Teleport");
        }
        _isTeleport=false;
    }
}

