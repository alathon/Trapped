using UnityEngine;
using System.Collections;

public class UnitState : MonoBehaviour {
    public delegate void DeathHandler(GameObject gObj);
    public event DeathHandler Death;
    public delegate void LifeChangedHandler(int newLife, bool tookDamage);
    public event LifeChangedHandler LifeChanged;

    private RectTransform hpBarRect;

    [SerializeField]
    private int currentLife = 8;
    [SerializeField]
    private int maximumLife = 8;

    private bool isStunned = false;

    public bool Stunned
    {
        get
        {
            return this.isStunned;
        }
    }

    /// <summary>
    /// This is a pretty bad implementation that can't handle multiple overlapping stuns ;)
    /// </summary>
    private IEnumerator Stun(float duration)
    {
        this.isStunned = true;
        yield return new WaitForSeconds(duration);
        this.isStunned = false;
    }

    public void StunMe(float duration)
    {
        StartCoroutine(Stun(duration));
    }

    [SerializeField]
    private string hpBarPath = "HP Bar/Background/Fill";

    public int MaximumLife
    {
        get
        {
            return this.maximumLife;
        }
    }

    public int CurrentLife
    {
        get
        {
            return this.currentLife;
        }

        set
        {
            if (value != this.currentLife) { 
                if(LifeChanged != null)
                    LifeChanged(value, value < this.currentLife);
            }

            this.currentLife = Mathf.Max(0, value);
            if (this.currentLife == 0)
            {
                this.Die();
            }
            else if (this.hpBarRect != null)
            {
                this.hpBarRect.localScale = new Vector3((float)this.currentLife / (float)this.maximumLife,
                    this.hpBarRect.localScale.y, this.hpBarRect.localScale.z);
            }
        }
    }

    protected virtual void Die()
    {
        if(Death != null) Death(this.gameObject);
    }

    public bool IsDead()
    {
        return this.currentLife == 0;
    }

    public virtual void TakeDamage(int amount)
    {
        this.CurrentLife -= amount;
    }

    public virtual void Heal(int amount)
    {
        this.CurrentLife += amount;
    }

    void Awake()
    {
        if (this.hpBarPath != null)
        {
            this.FindHPBar();
        }
    }

    private void FindHPBar()
    {
        Transform fill = this.transform.Find(this.hpBarPath);
        if (fill != null)
        {
            this.hpBarRect = fill.GetComponent<RectTransform>();
        }
    }

}
