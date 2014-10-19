using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(UnitState))]
public class DumbAI : AI {
    private bool attacking = false;

    [SerializeField]
    private float attackRange = 0.3f;

    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackSpeed = 0.5f;

    override protected void AIRoutine()
    {
        if (this.playerState.IsDead())
        {
            this.agent.Stop();
            return;
        }

        if (this.GetComponent<UnitState>().Stunned)
        {
            this.agent.Stop();
            return;
        }

        float dist = Vector2.Distance(this.transform.position, this.player.transform.position);
        if (dist > this.attackRange)
        {
            this.agent.SetDestination(player.transform.position);
        }
        else
        {
            if (attacking) return;

            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        this.attacking = true;
        this.playerState.TakeDamage(this.attackDamage);
        yield return new WaitForSeconds(this.attackSpeed);
        this.attacking = false;
        yield break;
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
