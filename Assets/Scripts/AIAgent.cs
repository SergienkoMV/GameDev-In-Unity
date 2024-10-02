using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{

    public AIStateMachine StateMachine;
    public AIStateID InitialState;
    [SerializeField] private AIAgentConfig _config;
    [SerializeField] private Transform[] _pointers;
    [SerializeField] private LayerMask _layerMask;
    private GameObject _item;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Animator Animator => _animator;
    public AIAgentConfig Config => _config;
    public Transform[] Pointers => _pointers;
    public LayerMask LayerMask => _layerMask;
    public GameObject Item { get ; set ; }

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        StateMachine = new AIStateMachine(this);
        StateMachine.RegisterState(new IdleState());
        StateMachine.RegisterState(new SearchState());
        StateMachine.RegisterState(new CollectState());
        StateMachine.ChangeState(InitialState);
    }

    void Update()
    {
        StateMachine.Update();
    }
}
