using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RogueAI : AI {
    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackSpeed = 1.5f;

    [SerializeField]
    private float specialLockout = 10f;

    [SerializeField]
    private float specialDuration = 5f;

    private bool isInvisible = false;
    private bool isLockedOut = false;

    override protected void MoveTowardsPlayer(bool canSee)
    {
        this.agent.SetDestination(this.player.transform.position);
        if (canSee && !this.isInvisible && !isLockedOut)
        {
            this.isLockedOut = true;
            StartCoroutine(Special());
        }
    }

    IEnumerator Special()
    {
        // 1) Spawn smoke
        GameObject smoke = (GameObject)Instantiate(Resources.Load("Smoke Effect"), this.transform.position, Quaternion.identity);

        // 2) Make ourselves invisible.
        this.SetVisibility(0f);
        this.isInvisible = true;
        yield return new WaitForSeconds(this.specialDuration);

        // 3) Make ourselves visible later.
        GameObject.Destroy(smoke);
        this.isInvisible = false;
        this.SetVisibility(1f);

        // 4) Remove lockout.
        yield return new WaitForSeconds(this.specialLockout - this.specialDuration);
        this.isLockedOut = false;
    }

    void SetVisibility(float alpha)
    {
        Color prev = this.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = new Color(prev.r, prev.g, prev.g, alpha);
        Transform background = this.transform.Find("HP Bar/Background");
        Transform fill = this.transform.Find("HP Bar/Background/Fill");
        prev = background.GetComponent<Image>().color;
        background.GetComponent<Image>().color = new Color(prev.r, prev.g, prev.b, alpha);
        prev = fill.GetComponent<Image>().color;
        fill.GetComponent<Image>().color = new Color(prev.r, prev.g, prev.b, alpha);
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
