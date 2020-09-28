using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;

    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string TeleportState = "Teleport";

    public AudioSource run = null;
    public AudioSource jump = null;
    public AudioSource land = null;


    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }
    
    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.StartJumping += OnStartJumping;
        _thirdPersonMovement.StartFalling += OnStartFalling;
        _thirdPersonMovement.StartTeleport += OnStartTeleport;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle -= OnIdle;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.StartJumping -= OnStartJumping;
        _thirdPersonMovement.StartFalling -= OnStartFalling;
        _thirdPersonMovement.StartTeleport -= OnStartTeleport;
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
        //run.Play();
    }

    private void OnStartJumping()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
        jump.Play();
    }
    private void OnStartFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
        land.Play();
    }

    private void OnStartTeleport()
    {
        _animator.CrossFadeInFixedTime(TeleportState, .2f);

    }
}
