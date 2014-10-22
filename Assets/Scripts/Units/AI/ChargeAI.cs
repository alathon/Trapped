using UnityEngine;
using System.Collections;

public class ChargeAI : AI {
    [SerializeField]
    private float attackSpeed = 2f;

    [SerializeField]
    private float windupSpeed = 1f;

    [SerializeField]
    private float chargeSpeed = 2.5f;

    [SerializeField]
    private float selfStunDuration = 1f;

    [SerializeField]
    private float otherStunDuration = 1f;

    [SerializeField]
    private int chargeDamage = 2;

    [SerializeField]
    private float chargeForce = 2f;

    [SerializeField]
    private float chargeTime = 2f;

    [SerializeField]
    private AudioClip chargeSound;

    private Vector2 chargeDir;
    private float chargeTimeLeft;
    private bool windingUp = false;
    override protected IEnumerator Attack()
    {
        this.attackState = AttackState.Attack;
        this.windingUp = true;
        this.agent.Stop();
        yield return new WaitForSeconds(this.windupSpeed);
        this.GetComponent<AudioSource>().clip = this.chargeSound;
        this.GetComponent<AudioSource>().Play();
        this.windingUp = false;
        var heading = this.player.transform.position - this.transform.position;
        var dist = heading.magnitude;
        var dir = heading / dist;

        // Set charge direction.
        //Debug.DrawLine(this.transform.position, this.transform.position + dir, Color.red, 2f);
        //Debug.Log(dir);
        //Debug.DrawLine(this.transform.position, this.player.transform.position, Color.blue, 2f);

        this.chargeDir = dir;
        this.chargeTimeLeft = this.chargeTime;
        this.agent.Stop();

        // Animate correctly.
        this.GetComponent<Animator>().SetBool("Walking", true);
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            animator.SetFloat("XMovement", Mathf.Sign(dir.x));
            animator.SetFloat("YMovement", 0f);
        }
        else
        {
            animator.SetFloat("XMovement", 0f);
            animator.SetFloat("YMovement", Mathf.Sign(dir.y));
        }
    }

    void FixedUpdate()
    {
        if (this.attackState == AttackState.Attack && !this.windingUp)
        {
            if (this.GetComponent<UnitState>().Stunned)
            {
                StartCoroutine(ResetAttack());
            }
            else
            {
                rigidbody2D.MovePosition(rigidbody2D.position + (chargeDir * this.chargeSpeed * Time.deltaTime));
                //this.Restrict();
                this.chargeTimeLeft -= Time.deltaTime;
                if (this.chargeTimeLeft <= 0f)
                {
                    StartCoroutine(ResetAttack());
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (this.attackState == AttackState.Attack)
        {
            if (col.gameObject.name.Equals("NavObstacle"))
            {
                Debug.Log("Hit a wall.");
                this.GetComponent<UnitState>().StunMe(this.selfStunDuration);
            }
            else
            {
                Transform hitTarget = null;
                if (col.gameObject.tag.Equals("Hitbox")) hitTarget = col.gameObject.transform.parent;
                else if (col.gameObject.tag.Equals("Player")) hitTarget = col.gameObject.transform;
                else return;

                Debug.Log("Hit player or other enemy. Stunning.");
                GameObject target = hitTarget.gameObject;
                target.GetComponent<UnitState>().StunMe(this.otherStunDuration);
                target.GetComponent<Rigidbody2D>().AddForce(this.chargeDir * this.chargeForce);
                if(target.tag.Equals("Player")) target.GetComponent<UnitState>().TakeDamage(this.chargeDamage);
                StartCoroutine(ResetAttack());
            }
        }
    }

    void Restrict()
    {
        if (!this.GetComponent<PolyNavAgent>().polyNav.PointIsValid(transform.position))
            transform.position = this.GetComponent<PolyNavAgent>().polyNav.GetCloserEdgePoint(transform.position);
    }

    IEnumerator ResetAttack()
    {
        this.GetComponent<Animator>().SetBool("Walking", false);
        this.GetComponent<AudioSource>().Stop();
        this.chargeTimeLeft = 0f;
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
