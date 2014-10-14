using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers;

public class GameController : MonoBehaviour {
    private TrapPrefabsManager trapPrefabsManager;
    private GameState state;

    /// <summary>
    /// Signalled when the phase changes.
    /// </summary>
    /// <param name="newPhase"></param>
    public delegate void PhaseChangedHandler(Phase newPhase);
    public event PhaseChangedHandler PhaseChanged;

    /// <summary>
    /// Signalled when the amount of life changes.
    /// </summary>
    /// <param name="newLife"></param>
    public delegate void LifeChangedHandler(int newLife);
    public event LifeChangedHandler LifeChanged;

    /// <summary>
    /// Signalled when the amount of player resources available changes.
    /// </summary>
    /// <param name="newCount"></param>
    public delegate void ResourceChangedHandler(int newCount);
    public event ResourceChangedHandler ResourceChanged;

    /// <summary>
    /// Signalled when the amount of unplaced traps changes.
    /// </summary>
    /// <param name="metadata">
    /// The trap's metadata (name etc).
    /// </param>
    /// <param name="newTotal">
    /// The new amount of traps available.
    /// </param>
    public delegate void TrapCountChangedHandler(TrapMetadata metadata, int newTotal);
    public event TrapCountChangedHandler TrapCountChanged;

    void Awake()
    {
        this.trapPrefabsManager = this.GetComponent<TrapPrefabsManager>();
        state = new GameState();
        Invoke("SetupDummyState", 0.5f);
    }

    /// <summary>
    /// Used by e.g. LifeManager to know how much life is the maximum.
    /// </summary>
    /// <returns>
    /// Total amount of life (should be divisible by 2!).
    /// </returns>
    public int GetLifeMax()
    {
        return this.state.lifeMax;
    }

    private void SetupDummyState()
    {
        this.PlanningPhase_Start();
        // Add 5 of every trap.
        foreach (GameObject trapPrefab in this.trapPrefabsManager.prefabs)
        {
            this.AddTraps(trapPrefab, 5);
        }
    }

    void RemoveTraps(GameObject gObj, int count)
    {
        TrapMetadata meta = gObj.GetComponent<TrapMetadata>();
        if (TrapCountChanged != null)
        {
            TrapCountChanged(meta, this.state.RemoveTraps(meta.trapName, count));
        }
    }

    void AddTraps(GameObject gObj, int count)
    {
        TrapMetadata meta = gObj.GetComponent<TrapMetadata>();
        if (TrapCountChanged != null)
        {
            TrapCountChanged(meta, this.state.AddTraps(meta.trapName, count));
        }   
    }


    void Update()
    {
        if (this.state.currentPhase == Phase.Planning) PlanningPhase_Update();
        else if (this.state.currentPhase == Phase.Action) ActionPhase_Update();
    }

    // ACTION PHASE
    void ActionPhase_Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Clickables");
            var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100f, layerMask);
            if (hit.collider == null) return;

            GameObject gObj = hit.collider.transform.gameObject;

            // Okay, we've hit a trap.
            if (gObj.GetComponent<TrapMetadata>() != null)
            {
                gObj.GetComponent<TrapAction>().Trigger();
            }
        }
    }

    void ActionPhase_Start()
    {
        GameObject.FindGameObjectWithTag("ActionPhase_GUI").GetComponent<ActionPhaseUIManager>().ActivateGUI();
        this.SetState(Phase.Action);
    }

    void SetState(Phase newPhase)
    {
        this.state.currentPhase = newPhase;
        this.PhaseChanged(this.state.currentPhase);
    }

    void ActionPhase_TryEndPhase()
    {
        GameObject.FindGameObjectWithTag("ActionPhase_GUI").GetComponent<ActionPhaseUIManager>().DeactivateGUI();
    }

    // PLANNING PHASE

    /// <summary>
    /// During Planning Phase, when trap bar icon is clicked on to place a trap.
    /// </summary>
    /// <param name="trapName"></param>
    public void PlanningPhase_OnClickTrapBar(string trapName)
    {
        if (this.state.CurrentPlacementPrefab != null)
        {
            GameObject.Destroy(this.state.CurrentPlacementPrefab);
        }

        GameObject trapPrefabInst = Instantiate(this.trapPrefabsManager.GetTrapObjByName(trapName)) as GameObject;
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        trapPrefabInst.transform.position = new Vector3(mousePosInWorld.x, mousePosInWorld.y, 0f);
        trapPrefabInst.GetComponent<FollowMouse>().state = FollowMouse.PlacementState.Placing;
        this.state.CurrentPlacementPrefab = trapPrefabInst;
    }


    public void PlanningPhase_OnClickEndPhaseBtn()
    {
        this.PlanningPhase_TryEndPhase();
        this.ActionPhase_Start();
    }

    void PlanningPhase_TryEndPhase()
    {
        // TODO: Any criterion for ending the phase???
        GameObject.FindGameObjectWithTag("PlanningPhase_GUI").GetComponent<PlanningPhaseUIManager>().DeactivateGUI();
    }

    void PlanningPhase_Start()
    {
        GameObject.FindGameObjectWithTag("PlanningPhase_GUI").GetComponent<PlanningPhaseUIManager>().ActivateGUI();
        this.SetState(Phase.Planning);
    }

    void PlanningPhase_Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (this.state.CurrentPlacementPrefab != null)
            {
                FollowMouse.PlacementState trapState = this.state.CurrentPlacementPrefab.GetComponent<FollowMouse>().state;
                if (trapState == FollowMouse.PlacementState.Placing)
                {
                    this.PlanningPhase_TryPlaceTrap();
                }
                else if (trapState == FollowMouse.PlacementState.Angling)
                {
                    this.PlanningPhase_TryFinalizeTrapPosition();
                }
            } else {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100f);
                if (hit.collider == null) return;

                GameObject gObj = hit.collider.transform.gameObject;

                // Okay, we've hit a trap.
                if(gObj.GetComponent<TrapMetadata>() != null) {
                    FollowMouse fMouse = gObj.GetComponent<FollowMouse>();
                    fMouse.state = FollowMouse.PlacementState.Placing;
                    this.state.CurrentPlacementPrefab = gObj;
                    this.AddTraps(this.state.CurrentPlacementPrefab, 1);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            this.PlanningPhase_Cancel();
        }
    }



    /// <summary>
    /// Cancel any on-going action.
    /// </summary>
    void PlanningPhase_Cancel()
    {
        if (this.state.CurrentPlacementPrefab != null)
        {
            GameObject.Destroy(this.state.CurrentPlacementPrefab);
        }
    }

    public void PlanningPhase_PickupTrap(GameObject trapPrefab)
    {
        if (this.state.CurrentPlacementPrefab != null) return;
        this.state.CurrentPlacementPrefab = trapPrefab;
    }

    void PlanningPhase_TryPlaceTrap()
    {
        GameObject trapPrefab = this.state.CurrentPlacementPrefab;
        if (trapPrefab.GetComponent<ValidTrapPlacement>().IsValid)
        {
            trapPrefab.GetComponent<FollowMouse>().state = FollowMouse.PlacementState.Angling;
        }
        else
        {
            // TODO: Play a 'bad' sound or somesuch.
            Debug.Log("Not valid!!");
        }
    }

    void PlanningPhase_TryFinalizeTrapPosition()
    {
        
        GameObject trapPrefab = this.state.CurrentPlacementPrefab;
        if (trapPrefab.GetComponent<ValidTrapPlacement>().IsValid)
        {
            trapPrefab.GetComponent<FollowMouse>().state = FollowMouse.PlacementState.Placed;
            this.state.CurrentPlacementPrefab = null;
            this.RemoveTraps(trapPrefab, 1);
        }
        else
        {
            // TODO: Play a 'bad' sound or somesuch.
            Debug.Log("Not valid!!");
        }
    }
}