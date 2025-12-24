using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;


namespace EnvInteract
{
    
    public class Context : IStateMachineContext
    {
        [SerializeField] TwoBoneIKConstraint _rightHandIK;
        [SerializeField] TwoBoneIKConstraint _leftHandIK;
        [SerializeField] MultiRotationConstraint _leftMultiRotation;
        [SerializeField] MultiRotationConstraint _rightMultiRotation;
        [SerializeField] Rigidbody _rb;
        [SerializeField] Collider _collider;

        public TwoBoneIKConstraint RightHandIK => _rightHandIK;
        public TwoBoneIKConstraint LeftHandIK => _leftHandIK;
        public MultiRotationConstraint LeftMultiRotation => _leftMultiRotation;
        public MultiRotationConstraint RightMultiRotation => _rightMultiRotation;
        public Rigidbody RB => _rb;
        public Collider RootCollider => _collider;

        public void Validate()
        {
            
        }
    }

    public enum EStateType {
        Reset, Search, Approach, Rise, Touch
    }

    public class StateMachine : IStateManager<EStateType, Context>
    {
        void Awake()
        {
            _context.Validate();

            InitializeStates();
        }

        void InitializeStates()
        {
            RegisterState(new ResetState(_context));
            RegisterState(new SearchState(_context));
            RegisterState(new ApproachState(_context));
            RegisterState(new RiseState(_context));
            RegisterState(new TouchState(_context));

            _currentState = _stateList[EStateType.Reset];
        }
    }

}