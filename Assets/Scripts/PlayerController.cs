using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private Vector2Int stepsToEncounterRange;
    
    private PlayerControls _playerControls;
    private Rigidbody _rb;
    private Vector3 _movement;
    private int _stepsInGrass;
    private int _stepsToEncounter;
    private bool _movingInGrass;
    private float _stepTimer;

    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private const string BattleScene = "BattleScene";
    private const float TimePerStep = 0.5f;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody>();
        _movement = new Vector3();
        CalculateStepsToNextEncounter();
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

        animator.SetBool(IsWalking, _movement != Vector3.zero);
        if (_movement.x != 0)
        {
            playerSprite.flipX = _movement.x < 0;
        }
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(transform.position + (_movement * (speed * Time.fixedDeltaTime)));

        _movingInGrass = _movement != Vector3.zero && Physics.CheckSphere(transform.position, 1f, grassLayer);
        if (_movingInGrass)
        {
            _stepTimer += Time.fixedDeltaTime;
            if (_stepTimer > TimePerStep)
            {
                _stepsInGrass++;
                _stepTimer = 0;

                if (_stepsInGrass >= _stepsToEncounter)
                {
                    SceneManager.LoadScene(BattleScene);
                }
            }
        }
    }

    private void CalculateStepsToNextEncounter()
    {
        _stepsToEncounter = Random.Range(stepsToEncounterRange.x, stepsToEncounterRange.y);
    }
}