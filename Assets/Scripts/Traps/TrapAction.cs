using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers;

public class TrapAction : MonoBehaviour {
    [SerializeField]
    protected AudioClip actionClip;

    [SerializeField]
    protected AudioClip windupClip;

    [SerializeField]
    protected float reuseTime = 0f;

    [SerializeField]
    protected float windupTime = 0f;

    [SerializeField]
    protected float duration = 1f;

    protected bool inUse = false;
    protected GameController controller;

    public bool IsReuseable()
    {
        return reuseTime > 0;
    }

    public virtual void Trigger()
    {
        if (inUse) return;

        if (windupTime > 0)
        {
            StartCoroutine(Windup());
        }
        else
        {
            StartCoroutine(Action());
        }
    }

    void Start()
    {
        this.SetTrapAlpha(0.5f);
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChange);
    }

    public void OnPhaseChange(Phase newPhase)
    {
        this.Reset();
    }

    protected virtual void Reset()
    {

    }
    protected virtual IEnumerator Windup()
    {
        this.inUse = true;
        SendMessage("OnUseUpdate", this.inUse);
        if (this.windupClip != null)
        {
            this.controller.GetComponent<AudioController>().PlaySFX(this.windupClip);
        }

        yield return new WaitForSeconds(this.windupTime);
        yield return StartCoroutine(Action());
    }

    public void SetTrapAlpha(float a)
    {
        SpriteRenderer sRend = this.GetComponent<SpriteRenderer>();
        Color c = sRend.color;
        sRend.color = new Color(c.r, c.b, c.g, a);
    }

    protected virtual IEnumerator PostAction()
    {
        yield return new WaitForSeconds(this.reuseTime);
        this.inUse = false;
        SendMessage("OnUseUpdate", this.inUse);
    }

    protected virtual IEnumerator Action()
    {
        yield return new WaitForSeconds(this.duration);
        yield return StartCoroutine(PostAction());
    }
}
