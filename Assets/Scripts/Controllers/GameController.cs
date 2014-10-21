using UnityEngine;
using System.Collections;
using Assets.Scripts.Controllers;

public class GameController : MonoBehaviour {
    private TrapPrefabsManager trapPrefabsManager;
    private GameState state;
    private GameObject player;

    /// <summary>
    /// Signalled when the phase changes.
    /// </summary>
    /// <param name="newPhase"></param>
    public delegate void PhaseChangedHandler(Phase newPhase);
    public event PhaseChangedHandler PhaseChanged;

    /// <summary>
    /// Signalled when the wave changes.
    /// </summary>
    /// <param name="newWave"></param>
    public delegate void WaveChangedHandler(int newWave);
    public event WaveChangedHandler WaveChanged;

    /// <summary>
    /// Signalled when the amount of player resources available changes.
    /// </summary>
    /// <param name="newCount"></param>
    public delegate void ResourceChangedHandler(int newCount);
    public event ResourceChangedHandler ResourceChanged;

    /// <summary>
    /// Signalled when the player has placed all traps.
    /// </summary>
    public delegate void OutOfTrapsHandler();
    public event OutOfTrapsHandler OutOfTraps;

    /// <summary>
    /// Signalled when the amount of unplaced traps changes.
    /// </summary>
    /// <param name="metadata">
    /// The trap's metadata (name etc).
    /// </param>
    /// <param name="newTotal">
    /// The new amount of traps available.
    /// </param>
    public delegate void TrapCountChangedHandler(TrapMetadata metadata, int change, int newTotal);
    public event TrapCountChangedHandler TrapCountChanged;

    public Phase GetCurrentPhase()
    {
        return this.state.currentPhase;
    }

    void Start()
    {
        this.GetComponent<AudioController>().PlayIngameBackgroundClip();
    }

    IEnumerator OnLevelWasLoaded(int lvl)
    {
        if (this.player == null)
        {
            this.player = GameObject.FindGameObjectWithTag("Player");
            this.player.GetComponent<UnitState>().Death += new UnitState.DeathHandler(OnPlayerDeath);
        }

        if (this.trapPrefabsManager == null)
        {
            this.trapPrefabsManager = this.GetComponent<TrapPrefabsManager>();
        }

        if (this.state == null)
        {
            state = new GameState();
        }

        yield return new WaitForSeconds(0.1f);

        this.SetState(Phase.Planning);
        this.state.spawnersLeft = 0;
        Transform waves = GameObject.FindGameObjectWithTag("Waves").transform;
        this.state.maxWave = waves.childCount;
        this.SetWave(1);
    }

    void SetLevel(int level)
    {
        // Change level.
        this.state.currentLevel = level;
        Application.LoadLevel(level.ToString());
    }

    // TODO:
    void CompleteGame()
    {
        
    }
    /// <summary>
    /// Changes the current wave and triggers appropriate event.
    /// </summary>
    /// <param name="wave">
    /// New wave number.
    /// </param>
    void SetWave(int wave)
    {
        this.state.currentWave = wave;
        if (this.WaveChanged != null)
        {
            WaveChanged(wave);
        }
    }

    /// <summary>
    /// Changes the state of the game and triggers an event for it.
    /// </summary>
    /// <param name="newPhase">
    /// The phase to enter into.
    /// </param>
    void SetState(Phase newPhase)
    {
        this.state.currentPhase = newPhase;
        if (this.PhaseChanged != null)
        {
            this.PhaseChanged(this.state.currentPhase);
        }

        if (newPhase == Phase.Action)
        {
            PlanningPhase_End();
            ActionPhase_Start();
        }
        else
        {
            ActionPhase_End();
            PlanningPhase_Start();
        }
    }

    IEnumerator NextWave()
    {
        // Level complete.
        if (this.state.maxWave == this.state.currentWave)
        {
            // Game complete.
            if (this.state.currentLevel == this.state.maxLevel)
            {
                this.CompleteGame();
            }
            else
            {
                this.SetLevel(this.state.currentLevel + 1);
            }
        }
        else // Load next wave.
        {
            GameObject txt = (GameObject)Instantiate(Resources.Load("Invaders defeated text"));
            txt.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
            yield return new WaitForSeconds(3.5f); // Oh god the horror! Magic number to match with animation length + extra.. Eghhhh!
            GameObject.Destroy(txt);
            this.SetState(Phase.Planning);
            this.SetWave(this.state.currentWave + 1);
        }
    }

    public void OnSpawnerDone(TimedSpawner spawner)
    {
        
        this.state.spawnersLeft -= 1;
        // Wave complete. No spawners left.
        if (this.state.spawnersLeft == 0)
        {
            StartCoroutine(this.NextWave());
        }
    }

    public void OnMobSpawned(GameObject mob)
    {
        mob.GetComponent<UnitState>().Death += new UnitState.DeathHandler(OnMobDeath);
    }

    public void OnMobDeath(GameObject mob)
    {
        this.GainMana(Random.Range(350, 700) * Application.loadedLevel);
        GameObject.Destroy(mob);
    }

    void GainMana(int amount)
    {
        this.state.mana += amount;
        if (this.ResourceChanged != null)
        {
            this.ResourceChanged(this.state.mana);
        }
    }

    public void OnPlayerDeath(GameObject gObj)
    {
        this.SetLevel(Application.loadedLevel);
    }



    /// <summary>
    /// Removes a certain number of a kind of trap.
    /// </summary>
    /// <param name="gObj">
    /// The trap prefab.
    /// </param>
    /// <param name="count">
    /// The amount of traps to remove.
    /// </param>
    public void RemoveTraps(GameObject gObj, int count)
    {
        TrapMetadata meta = gObj.GetComponent<TrapMetadata>();
        int newCount = this.state.RemoveTraps(meta.trapName, count);
        if (TrapCountChanged != null)
        {
            TrapCountChanged(meta, count, newCount);
        }  

        if (this.state.OutOfTrapsToPlace())
        {
            if (OutOfTraps != null)
            {
                OutOfTraps();
            }
        }
    }

    /// <summary>
    /// Adds a certain number of a kind of trap.
    /// </summary>
    /// <param name="gObj">
    /// The trap prefab.
    /// </param>
    /// <param name="count">
    /// The amount of traps to add.
    /// </param>
    public void AddTraps(GameObject gObj, int count)
    {
        TrapMetadata meta = gObj.GetComponent<TrapMetadata>();
        int newCount = this.state.AddTraps(meta.trapName, count);
        if (TrapCountChanged != null)
        {
            TrapCountChanged(meta, count, newCount);
        }   
    }



    public Phase GetPhase()
    {
        return this.state.currentPhase;
    }

    void LateUpdate()
    {
        if (this.state.currentPhase == Phase.Planning) PlanningPhase_Update();
        else if (this.state.currentPhase == Phase.Action) ActionPhase_Update();
    }

    // ACTION PHASE

    /// <summary>
    /// Checks for clicks to trigger traps.
    /// </summary>
    void ActionPhase_Update()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1 << LayerMask.NameToLayer("Clickables");
        var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 100f, layerMask);
        if (hit.collider == null) return;
        GameObject gObj = hit.collider.transform.gameObject;

        // Okay, we've hit a trap.
        if (gObj.GetComponent<TrapMetadata>() != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                gObj.GetComponent<TrapAction>().Trigger();
            }
        }

    }

    /// <summary>
    /// Called when the action phase begins. Shows the action phase canvas and
    /// changes the gamestate to be in the action phase.
    /// </summary>
    void ActionPhase_Start()
    {
        Transform waveParent = GameObject.FindGameObjectWithTag("Waves").transform;
        Transform waveObj = waveParent.transform.Find(this.state.currentWave.ToString() + "/Spawners");
        foreach (Transform child in waveObj)
        {
            this.state.spawnersLeft++;
            Transform actualSpawner = child.Find("TimedSpawner");
            TimedSpawner spawner = actualSpawner.GetComponent<TimedSpawner>();
            spawner.MobSpawned += new TimedSpawner.MobSpawnedHandler(OnMobSpawned);
            spawner.SpawnerDone += new TimedSpawner.SpawnerDoneHandler(OnSpawnerDone);
            spawner.StartSpawning();
        }

        GameObject.FindGameObjectWithTag("ActionPhase_GUI").GetComponent<ActionPhaseUIManager>().ActivateGUI();
    }


    /// <summary>
    /// Called when the action phase should end. Hides the action phase canvas.
    /// </summary>
    void ActionPhase_End()
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

    /// <summary>
    /// During Planning Phase, when the 'End Phase' button is clicked.
    /// </summary>
    public void PlanningPhase_OnClickEndPhaseBtn()
    {
        this.SetState(Phase.Action);
    }

    /// <summary>
    /// Ends the planning phase, hiding the planning phase canvas.
    /// </summary>
    void PlanningPhase_End()
    {
        GameObject.FindGameObjectWithTag("PlanningPhase_GUI").GetComponent<PlanningPhaseUIManager>().DeactivateGUI();
    }

    void PlanningPhase_Start()
    {
        GameObject.FindGameObjectWithTag("PlanningPhase_GUI").GetComponent<PlanningPhaseUIManager>().ActivateGUI();
        Transform waveParent = GameObject.FindGameObjectWithTag("Waves").transform;
        Transform waveObj = waveParent.transform.Find(this.state.currentWave.ToString() + "/Spawners");
        foreach (Transform child in waveObj)
        {
            Transform actualSpawner = child.Find("TimedSpawner");
            TimedSpawner spawner = actualSpawner.GetComponent<TimedSpawner>();
            spawner.ActivateGraphic();
        }
    }

    /// <summary>
    /// Checks for button clicks, used to place or pick up traps during the planning phase.
    /// 
    /// </summary>
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
        trapPrefab.GetComponent<TrapAction>().SetTrapAlpha(1f);
    }

    void PlanningPhase_TryPlaceTrap()
    {
        GameObject trapPrefab = this.state.CurrentPlacementPrefab;
        if (trapPrefab.GetComponent<ValidTrapPlacement>().IsValid)
        {
            if (trapPrefab.GetComponent<TrapMetadata>().isDirectional)
            {
                trapPrefab.GetComponent<FollowMouse>().state = FollowMouse.PlacementState.Angling;
            }
            else
            {
                this.PlanningPhase_TryFinalizeTrapPosition();
            }
        }
        else
        {
            // TODO: Play a 'bad' sound or somesuch.
            Debug.Log("Not valid!! TODO: Play bad placement sound.");
        }
    }

    void PlanningPhase_TryFinalizeTrapPosition()
    {
        
        GameObject trapPrefab = this.state.CurrentPlacementPrefab;
        if (trapPrefab.GetComponent<ValidTrapPlacement>().IsValid)
        {
            trapPrefab.GetComponent<FollowMouse>().state = FollowMouse.PlacementState.Placed;
            trapPrefab.GetComponent<TrapAction>().SetTrapAlpha(0.5f);
            this.state.CurrentPlacementPrefab = null;
            this.RemoveTraps(trapPrefab, 1);
            this.GetComponent<AudioController>().PlaySFX(trapPrefab.GetComponent<TrapMetadata>().placementSound);
        }
        else
        {
            // TODO: Play a 'bad' sound or somesuch.
            Debug.Log("Not valid!!");
        }
    }
}