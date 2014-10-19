using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(UnitState))]
public class DumbAI : AI {
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackSpeed = 1.5f;

    override protected IEnumerator Attack()
    {
        this.attackState = AttackState.Attack;
        this.playerState.TakeDamage(this.attackDamage);
        this.attackState = AttackState.Recharge;
        yield return new WaitForSeconds(this.attackSpeed);
        this.attackState = AttackState.Available;
    }

    //Message from Agent
    void OnDestinationReached()
    {

        //do something here...
    }

    //Message from Agent
    void OnDestinationInvalid()
    {

    }
}
