using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;

    private PlayerControls _playerControls;
    private Rigidbody _rb;
    private Vector3 _movement;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody>();
        _movement = new Vector3();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void Update()
    {
        var input = _playerControls.Player.Move.ReadValue<Vector2>();
        _movement.x = input.x;
        _movement.z = input.y;
        _movement.Normalize();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + (_movement * (speed * Time.fixedDeltaTime)));
    }
}