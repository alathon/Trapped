using UnityEngine;
using System.Collections;

/// <summary>
/// Provides basic WSAD movement, hooking into an Animator
/// which expects XMovement and YMovement to change depending
/// on which direction the player is facing/moving in.
/// 
/// Up/Down movement takes priority, so moving diagonally will always
/// show an up or down animation.
/// </summary>
public class Movement : MonoBehaviour {
    [SerializeField]
    private float movementSpeed = 1f;

    private Animator animator;

    void Start()
    {
        this.animator = this.GetComponent<Animator>();
    }

	void FixedUpdate () {
        Vector2 vel = new Vector3();

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetFloat("XMovement", -1f);
            animator.SetFloat("YMovement", 0f);
            vel += new Vector2(-1, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetFloat("XMovement", 1f);
            animator.SetFloat("YMovement", 0f);
            vel += new Vector2(1, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetFloat("YMovement", 1f);
            animator.SetFloat("XMovement", 0f);
            vel += new Vector2(0, 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animator.SetFloat("YMovement", -1f);
            animator.SetFloat("XMovement", 0f);
            vel += new Vector2(0, -1);
        }

        if (vel.magnitude > 0.001)
        {
            animator.SetBool("Walking", true);
            Vector3.Normalize(vel);
            vel *= this.movementSpeed;
            rigidbody2D.velocity = vel;
        }
        else
        {
            animator.SetBool("Walking", false);
        }
	}
}
