using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AI : MonoBehaviour {
    protected Animator animator;
    protected GameObject player;
    protected UnitState playerState;
    protected bool flashing = false;
    protected enum AttackState { Attack, Recharge, Available };
    protected AttackState attackState = AttackState.Available;
    protected RoomController roomCtrl;

    protected PolyNavAgent _agent;
    public PolyNavAgent agent
    {
        get
        {
            if (!_agent)
                _agent = GetComponent<PolyNavAgent>();
            return _agent;
        }
    }

    [SerializeField]
    protected float minAttackRange = 0f;

    [SerializeField]
    protected float maxAttackRange = 0.5f;

    [SerializeField]
    protected float forgetRange = 3.5f;

    private void OnLifeChanged(int amount, bool tookDamage)
    {
        if (tookDamage)
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        if (this.flashing) yield return 0;

        this.flashing = true;
        SpriteRenderer rend = this.GetComponent<SpriteRenderer>();
        Color prev = rend.color;
        rend.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        rend.color = prev;
        this.flashing = false;
    }

    void Start() {
        this.roomCtrl = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomController>();
        InitializeAI();
        InvokeRepeating("AIRoutine", 0f, 0.1f);
    }

    protected void InitializeAI()
    {
        this.GetComponent<UnitState>().LifeChanged += new UnitState.LifeChangedHandler(OnLifeChanged);
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.playerState = this.player.GetComponent<UnitState>();
        this.animator = this.GetComponent<Animator>();
    }

    virtual protected void AIRoutine()
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
        bool blocked = this.CanSee(this.player.transform);

        // First: Can we attack the player?
        // Are we a) Able to attack, b) Not blocked by LoS, c) Within attack range?
        if(this.attackState == AttackState.Available) {
            if(!blocked) {
                if (dist >= this.minAttackRange && dist <= this.maxAttackRange)
                {
                    this.agent.Stop();
                    StartCoroutine(Attack());
                    return;
                }
            }
        }

        // We can't attack. Can we see the player??
        if (!blocked)
        {
            this.agent.SetDestination(this.player.transform.position);
        }
        else // We can't see the player. Are we close enough to roam towards them, or do we go somewhere random??
        {
            if (!this.agent.hasPath)
            {
                // Set destination on player so we can measure path length.
                this.agent.SetDestination(this.player.transform.position);
                float pathLen = this.GetPathLength(this.agent.activePath);
                
                Vector2 targetPosition = Vector2.zero;
                if (pathLen >= this.forgetRange)
                {
                    targetPosition = this.roomCtrl.GetRandomRoom().transform.position;
                }
                else
                {
                    targetPosition = this.player.transform.position;
                }

                this.agent.SetDestination(targetPosition);
            }
        }
    }

    float GetPathLength(List<Vector2> path)
    {
        float total = 0f;
        Vector2 pastVect = this.transform.position;
        foreach (Vector2 v in path)
        {
            total += Vector2.Distance(pastVect, v);
            pastVect = v;
        }
        return total;
    }

    virtual protected IEnumerator Attack()
    {
        yield break;
    }

    protected bool CanSee(Transform target)
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
        return blocked;
    }

    void LateUpdate()
    {
        if (this.GetComponent<UnitState>().Stunned || this.playerState.IsDead())
        {
            this.agent.Stop();
            animator.SetBool("Walking", false);
            return;
        }

        // Animation. This should be elsewhere, really, in a different component!!
        if (this.agent.hasPath)
        {
            animator.SetBool("Walking", true);
            Vector3 next = this.agent.activePath[0];
            Vector3 diff = this.transform.position - next;
            Vector3 vel = new Vector3(Mathf.Sign(-diff.x), Mathf.Sign(-diff.y), 0f);

            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
            {
                animator.SetFloat("XMovement", vel.x);
                animator.SetFloat("YMovement", 0f);
            }
            else
            {
                animator.SetFloat("XMovement", 0f);
                animator.SetFloat("YMovement", vel.y);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }
}
