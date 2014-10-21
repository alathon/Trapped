using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(UnitState))]

public class RangedBowAI : AI {
    [SerializeField]
    private float attackSpeed = 3f;

    [SerializeField]
    private float windupSpeed = 1f;

    [SerializeField]
    private GameObject projectilePrefab;

    override protected IEnumerator Attack()
    {
        this.attackState = AttackState.Attack;
        yield return new WaitForSeconds(this.windupSpeed);
        var dir = this.player.transform.position - this.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();

        GameObject projectile = (GameObject)Instantiate(this.projectilePrefab, new Vector3(-100, -100), Quaternion.identity);
        projectile.GetComponent<ProjectileEffect>().SetFiredByPlayer(false);
        projectile.transform.position = this.transform.position;
        projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        projectile.GetComponent<Rigidbody2D>().velocity = dir * projectile.GetComponent<ProjectileEffect>().speed;

        this.attackState = AttackState.Recharge;
        yield return new WaitForSeconds(this.attackSpeed);
        this.attackState = AttackState.Available;
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
