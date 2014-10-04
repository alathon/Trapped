using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers;

public class GameController : MonoBehaviour {
    /// <summary>
    /// The prefab used in the planning phase trap panel.
    /// </summary>
    [SerializeField]
    private GameObject trapPanelPrefab;

    private TrapPrefabsManager trapPrefabsManager;
    private GameState state;

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
        Invoke("SetupDummyState", 0.1f);
    }

    private void SetupDummyState()
    {
        // Add 5 of every trap.
        foreach (GameObject trapPrefab in this.trapPrefabsManager.prefabs)
        {
            TrapMetadata meta = trapPrefab.GetComponent<TrapMetadata>();
            TrapCountChanged(meta, this.state.AddTraps(meta, 5));
        }
    }

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
        this.state.CurrentPlacementPrefab = trapPrefabInst;
    }

    void Update()
    {
        if (this.state.currentPhase == Phase.Planning) PlanningPhase_Update();
        else if (this.state.currentPhase == Phase.Action) ActionPhase_Update();
    }

    void PlanningPhase_Update()
    {
        if (Input.GetMouseButtonDown(0) && this.state.CurrentPlacementPrefab != null)
        {
            if (this.state.CurrentPlacementPrefab != null)
            {
                this.PlanningPhase_TryPlaceTrap(this.state.CurrentPlacementPrefab, Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            this.PlanningPhase_Cancel();
        }
    }

    void ActionPhase_Update()
    {

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

    void PlanningPhase_TryPlaceTrap(GameObject trapPrefab, Vector3 mousePos)
    {

    }
}