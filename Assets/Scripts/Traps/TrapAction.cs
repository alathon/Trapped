﻿using UnityEngine;
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

    public bool CanUse()
    {
        return !this.inUse;
    }

    public bool IsReuseable()
    {
        return reuseTime > 0;
    }

    public virtual void Trigger()
    {
        if (inUse) return;

        StartCoroutine(Windup());
    }

    void Start()
    {
        this.SetTrapAlpha(0.5f);
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChange);
    }

    void OnDestroy()
    {
        this.controller.PhaseChanged -= OnPhaseChange;
    }

    public void OnPhaseChange(Phase newPhase)
    {
        this.Reset();
    }

    protected virtual void Reset()
    {
        this.inUse = false;
        this.StopAllCoroutines();
        SendMessage("OnUseUpdate", this.inUse, SendMessageOptions.DontRequireReceiver);
    }

    protected virtual IEnumerator Windup()
    {
        this.inUse = true;
        SendMessage("OnUseUpdate", this.inUse, SendMessageOptions.DontRequireReceiver);
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
        SendMessage("OnUseUpdate", this.inUse, SendMessageOptions.DontRequireReceiver);
    }

    protected virtual IEnumerator Action()
    {
        yield return new WaitForSeconds(this.duration);
        yield return StartCoroutine(PostAction());
    }
}
