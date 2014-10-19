using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
[RequireComponent(typeof(UnitState))]
public class DumbAI : AI {
    private bool attacking = false;

    private GameObject player;
    private UnitState playerState;

    private Animator animator;
    private bool flashing = false;

    private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

    [SerializeField]
    private float attackRange = 0.3f;

    [SerializeField]
    private int attackDamage = 1;

    [SerializeField]
    private float attackSpeed = 0.5f;

    void Awake()
    {
        this.GetComponent<UnitState>().LifeChanged += new UnitState.LifeChangedHandler(OnLifeChanged);
    }

    override protected void InitializeAI()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.playerState = this.player.GetComponent<UnitState>();
        this.animator = this.GetComponent<Animator>();
	}

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

    private void OnLifeChanged(int amount, bool tookDamage)
    {
        if (tookDamage) {
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

	void LateUpdate () {
        if (this.GetComponent<UnitState>().Stunned)
        {
            this.agent.Stop();
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
