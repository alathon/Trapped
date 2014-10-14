using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AI : MonoBehaviour {
    public delegate void DeathHandler(GameObject gObj);
    public event DeathHandler Death;

    private RectTransform hpBarRect;

    [SerializeField]
    private int currentLife = 100;
    [SerializeField]
    private int maximumLife = 100;

    public int CurrentLife
    {
        get
        {
            return this.currentLife;
        }

        set
        {
            this.currentLife = Mathf.Max(0, value);
            if (this.currentLife == 0)
            {
                this.Die();
            }
            else
            {
                this.hpBarRect.localScale = new Vector3((float)this.currentLife / (float)this.maximumLife, this.hpBarRect.localScale.y, this.hpBarRect.localScale.z);
            }
        }
    }

    protected virtual void Die()
    {
        Death(this.gameObject);
    }

    public virtual void TakeDamage(int amount)
    {
        this.CurrentLife -= amount;
    }

    void Start() {
        FindHPBar();
        InitializeAI();
        InvokeRepeating("AIRoutine", 0f, 0.25f);
    }

    private void FindHPBar()
    {
        this.hpBarRect = this.transform.Find("HP Bar/Background/Fill").GetComponent<RectTransform>();
    }

    protected virtual void InitializeAI()
    {

    }
    protected virtual void AIRoutine()
    {

    }
}
