using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
public class DumbAI : AI {
    private GameObject player;
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

    override protected void InitializeAI()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.animator = this.GetComponent<Animator>();
	}

    override protected void AIRoutine()
    {
        this.agent.SetDestination(player.transform.position);
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

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        StartCoroutine(FlashRed());
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
