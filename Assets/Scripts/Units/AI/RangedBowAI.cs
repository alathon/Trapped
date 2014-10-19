﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(UnitState))]

public class RangedBowAI : AI {
    private enum AttackState { Attack, Recharge, Available };

    private AttackState attackState = AttackState.Available;

    [SerializeField]
    private float attackRangeMax = 4f;

    [SerializeField]
    private float attackRangeMin = 0.5f;

    [SerializeField]
    private float attackSpeed = 3f;

    [SerializeField]
    private float windupSpeed = 1f;

    [SerializeField]
    private GameObject projectilePrefab;

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

        if (attackState == AttackState.Attack)
        {
            return;
        }

        float dist = Vector2.Distance(this.transform.position, this.player.transform.position);
        if (dist > this.attackRangeMin)
        {
            if (dist <= this.attackRangeMax && attackState == AttackState.Available)
            {
                // Logic: We need to test that the *sides* of the projectile can LoS all the way to the player.
                // So we need a linecast from *3* places: Middle of enemy, 'left' of enemy, 'right' of enemy.
                // To get left and right, we get the direction between enemy and player, and then we move in (dir.x, -dir.y) and (-dir.x, dir.y)
                // and fire lines from those spots. If any of the three linecasts hits a wall, its no-go!
                RaycastHit2D[] hits = new RaycastHit2D[5];
                int layerMask = (1 << LayerMask.NameToLayer("Obstacles"));

                
                float distFromCenter = 0.08f;
                Vector2 enemyToPlayerDir = this.player.transform.position - this.transform.position;
                Vector2[] locations = new Vector2[3];
                locations[0] = new Vector2(this.transform.position.x - (Mathf.Sign(enemyToPlayerDir.x) * distFromCenter), this.transform.position.y + Mathf.Sign(enemyToPlayerDir.y) * distFromCenter);
                locations[1] = new Vector2(this.transform.position.x + (Mathf.Sign(enemyToPlayerDir.x) * distFromCenter), this.transform.position.y - Mathf.Sign(enemyToPlayerDir.y) * distFromCenter);
                locations[2] = this.transform.position;

                bool blocked = false;
                for (int i = 0; i < 3; i++)
                {
                    Vector2 spot = locations[i];

                    //Color lineClr;
                    //if(i == 0) lineClr = Color.red;
                    //else if(i == 1) lineClr = Color.green;
                    //else lineClr = Color.blue;
                    //Debug.DrawLine(spot, this.player.transform.position, lineClr, 1f);

                    int hitCount = Physics2D.LinecastNonAlloc(spot, this.player.transform.position, hits, layerMask);
                    if (hitCount > 0)
                    {
                        blocked = true;
                        break;
                    }
                }

                if(blocked)
                {
                    this.agent.SetDestination(player.transform.position);
                }
                else
                {
                    this.agent.Stop();
                    StartCoroutine(Attack());
                }
            }
            else
            {
                this.agent.SetDestination(player.transform.position);
            }
        }
    }

    IEnumerator Attack()
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
        projectile.GetComponent<Rigidbody2D>().velocity = dir * 7.5f;

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
