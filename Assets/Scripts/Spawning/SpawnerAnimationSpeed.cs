using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SpawnerAnimationSpeed : MonoBehaviour {
    private Animator animator;
    private TimedSpawner spawner;

    private float initialTime = 0f;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponent<Animator>();
        this.spawner = this.GetComponent<TimedSpawner>();
        this.initialTime = this.spawner.GetInitialTime();
	}

    void Update()
    {
        float timeLeft = this.spawner.GetTimeLeft();
        float factor = (initialTime - timeLeft) / initialTime;
        // Speed increase from 25% (spawner just started) up to 500% (no time left)
        this.animator.speed = 0.25f + (5f * factor);
    }
}
