using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
    private int currentLife = 2;

    public int CurrentLife
    {
        get
        {
            return this.currentLife;
        }

        set
        {
            this.currentLife = Mathf.Max(0, value);
            if (this.currentLife == 0) this.Die();
        }
    }

    protected virtual void Die()
    {
        GameObject.Destroy(this.transform.gameObject);
    }

    public virtual void TakeDamage(int amount)
    {
        this.CurrentLife -= amount;
    }

    void Start() {
        InitializeAI();
        InvokeRepeating("AIRoutine", 0f, 0.25f);
    }

    protected virtual void InitializeAI()
    {

    }
    protected virtual void AIRoutine()
    {

    }
}
