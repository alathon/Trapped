using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AI : MonoBehaviour {
    protected Animator animator;
    protected GameObject player;
    protected UnitState playerState;
    protected bool flashing = false;

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

    protected virtual void AIRoutine()
    {

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
