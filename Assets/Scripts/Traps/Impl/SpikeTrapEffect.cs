using UnityEngine;
using System.Collections;

public class SpikeTrapEffect : TrapEffectOnHit {
    [SerializeField]
    private float stunDuration = 3.5f;

    override protected void EffectState(UnitState state)
    {
        base.EffectState(state);
        state.StunMe(this.stunDuration);
    }
}