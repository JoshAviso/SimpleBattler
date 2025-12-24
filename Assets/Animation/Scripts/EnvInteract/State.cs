using UnityEngine;

namespace EnvInteract
{
    public abstract class State : IGenericState<EStateType, Context>
    {
        public State(EStateType state_type, EnvInteract.Context context) : base(state_type, context) { }
    }
}
