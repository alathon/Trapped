using UnityEngine;
using System.Collections;

public class TrapAction : MonoBehaviour {
    [SerializeField]
    protected float reuseTime = 0f;

    [SerializeField]
    protected float windupTime = 0f;

    [SerializeField]
    protected float duration = 1f;

    public bool IsReuseable()
    {
        return reuseTime > 0;
    }

    public virtual void Trigger()
    {
        if (windupTime > 0)
        {
            StartCoroutine(Windup());
        }
        else
        {
            StartCoroutine(Action());
        }
    }

    protected virtual IEnumerator Windup()
    {
        yield return new WaitForSeconds(this.windupTime);
        yield return StartCoroutine(Action());
    }

    protected virtual IEnumerator Action()
    {
        yield return new WaitForSeconds(this.duration);
    }
}
