using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }
    
    private void Update()
    {
        var movement = _playerControls.Player.Move.ReadValue<Vector2>();
        print(movement);
    }
}
