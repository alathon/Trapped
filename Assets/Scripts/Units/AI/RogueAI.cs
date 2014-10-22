using UnityEngine;
using System.Collections;

public class RogueAI : AI {
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackSpeed = 1.5f;

    [SerializeField]
    private float specialChance = 0.45f;

    [SerializeField]
    private float specialDuration = 2f;

    private bool isInvisible = false;

    override protected IEnumerator MoveTowardsPlayer(bool canSee)
    {
        this.agent.SetDestination(this.player.transform.position);
        if (UnityEngine.Random.Range(0f, 1f) <= this.specialChance && canSee && !this.isInvisible)
        {
            StartCoroutine(Special());
        }
        yield break;
    }

    IEnumerator Special()
    {
        // 1) Spawn smoke
        GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke"), this.transform.position, Quaternion.identity);

        // 2) Make ourselves invisible.
        this.SetVisibility(0f);
        this.isInvisible = true;
        yield return new WaitForSeconds(this.specialDuration);

        // 3) Make ourselves visible later.
        GameObject.Destroy(smoke);
        this.isInvisible = false;
        this.SetVisibility(1f);
    }

    void SetVisibility(float alpha)
    {
        Color prev = this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = new Color(prev.r, prev.g, prev.g, alpha);
    }

    override protected IEnumerator Attack()
    {
        this.attackState = AttackState.Attack;
        if (this.isInvisible)
        {
            this.SetVisibility(1f);
            this.isInvisible = false;
        }

        this.playerState.TakeDamage(this.attackDamage);
        this.attackState = AttackState.Recharge;
        yield return new WaitForSeconds(this.attackSpeed);
        this.attackState = AttackState.Available;
    }
}
