using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers;

public class TimedSpawner : MonoBehaviour {
    [SerializeField]
    private float startWait = 0f;
    
    [SerializeField]
    private float intervalBetween = 0f;

    [SerializeField]
    private int amountPerSpawn = 0;

    [SerializeField]
    private int maxAlive = 0;

    [SerializeField]
    private int totalAmount = 0;

    [SerializeField]
    private GameObject[] prefabs;

    private float timeCounter = 0f;
    private List<GameObject> spawnedMobs = new List<GameObject>();
    private GameController controller;
    private bool shutdown = true;

    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChange);
    }

    void OnPhaseChange(Phase newPhase)
    {
        if (newPhase == Phase.Action)
        {
            this.shutdown = false;
            StartCoroutine(Spawn());
        }
        else
        {
            this.shutdown = true;
            this.RemoveMobs();
        }
    }

    void OnMobDeath(GameObject mob)
    {
        if (this.spawnedMobs.Contains(mob))
        {
            this.spawnedMobs.Remove(mob);
            GameObject.Destroy(mob);
        }
    }

    void RemoveMobs()
    {
        foreach (var mob in this.spawnedMobs)
        {
            GameObject.Destroy(mob);
        }
        this.spawnedMobs = new List<GameObject>();
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            if (this.shutdown)
            {
                yield break;
            }

            int diff = Mathf.Min(maxAlive - spawnedMobs.Count, this.totalAmount);
            if (timeCounter++ >= intervalBetween && diff > 0)
            {
                for (int i = 0; i < Mathf.Min(diff, amountPerSpawn); i++)
                {
                    GameObject prefab = this.prefabs[Random.Range(0, this.prefabs.Length - 1)];
                    GameObject mob = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
                    this.spawnedMobs.Add(mob);
                    mob.GetComponent<UnitState>().Death += new UnitState.DeathHandler(OnMobDeath);
                    this.totalAmount--;
                }

                if (this.totalAmount <= 0)
                {
                    yield break;
                }

                timeCounter = 0f;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
