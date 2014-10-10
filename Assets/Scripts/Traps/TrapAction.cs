using UnityEngine;
using System.Collections;

public class TrapAction : MonoBehaviour {
    [SerializeField]
    private float reuseTime = 0f;

    [SerializeField]
    private float windupTime = 0f;

    [SerializeField]
    private float duration = 1f;

    public bool IsReuseable()
    {
        return reuseTime > 0;
    }

    public virtual void Trigger()
    {

    }
}
