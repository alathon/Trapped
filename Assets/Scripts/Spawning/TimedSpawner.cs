using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controllers;

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
        yield return new WaitForSeconds(startWait);
        GameObject prefab = this.prefabs[Random.Range(0, this.prefabs.Length - 1)];
        GameObject mob = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        mob.GetComponent<UnitState>().Death += new UnitState.DeathHandler(OnMobDeath);
        if (this.MobSpawned != null)
        {
            this.MobSpawned(mob);
        }
    }
}
