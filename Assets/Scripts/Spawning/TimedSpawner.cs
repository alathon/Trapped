using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using System;

public class TimedSpawner : MonoBehaviour {
    /// <summary>
    /// Signalled when all of the spawners mobs are dead.
    /// </summary>
    public delegate void SpawnerDoneHandler(TimedSpawner spawner);
    public event SpawnerDoneHandler SpawnerDone;

    /// <summary>
    /// Signalled when a mob is spawned
    /// </summary>
    public delegate void MobSpawnedHandler(GameObject mob);
    public event MobSpawnedHandler MobSpawned;

    [SerializeField]
    private float startWait = 0f;

    [SerializeField]
    private GameObject[] prefabs;

    private float timeLeft = 0f;

    void Awake()
    {
        this.timeLeft = this.startWait;
    }

    public float GetTimeLeft()
    {
        return this.timeLeft;
    }

    public float GetInitialTime()
    {
        return this.startWait;
    }

    public void ActivateGraphic()
    {
        
        this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
    }

    public void StartSpawning()
    {
        StartCoroutine(Spawn());
    }

    public void OnMobDeath(GameObject mob)
    {
        if (this.SpawnerDone != null)
        {
            this.SpawnerDone(this);
        }
    }

    IEnumerator Spawn()
    {
        this.timeLeft = this.startWait;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(startWait / 10);
            this.timeLeft -= startWait / 10;
        }
            
        GameObject prefab = this.prefabs[UnityEngine.Random.Range(0, this.prefabs.Length - 1)];
        GameObject mob = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        mob.GetComponent<UnitState>().Death += new UnitState.DeathHandler(OnMobDeath);
        if (this.MobSpawned != null)
        {
            this.MobSpawned(mob);
        }
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0f);
    }
}
