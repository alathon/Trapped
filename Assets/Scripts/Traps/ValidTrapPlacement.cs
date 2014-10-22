using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class ValidTrapPlacement : MonoBehaviour {
    private GameController controller;

    private GameController Controller
    {
        get
        {
            if (controller == null)
            {
                controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            }
            return controller;
        }
    }

    private bool isValid = true;
    public bool IsValid
    {
        get { 
            return this.isValid; 
        }

        set {
            if (value == this.isValid)
            {
                return;
            }

            this.isValid = value;
            this.gameObject.SendMessage("OnPlacementValidChange", this.isValid);
        }
    }

    private Collider2D trapCollider;

    void Awake()
    {
        trapCollider = this.GetComponent<Collider2D>();
    }

    private int collisions = 0;

    bool ShouldBlock(GameObject gObj)
    {
        if(gObj.name.Equals("NavObstacle") || gObj.tag.Equals("Trap")) return true;
        if(gObj.name.Equals("TimedSpawnerParent")) {
            Transform wave = gObj.transform.parent.parent;
            if (Convert.ToInt32(wave.name) == Controller.GetCurrentWave())
            {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj == this.gameObject) return;

        if (ShouldBlock(gObj))
        {
            this.IsValid = false;
            this.collisions += 1;
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject gObj = other.gameObject;
        if (gObj == this.gameObject) return;

        if (ShouldBlock(gObj))
        {
            this.collisions -= 1;
        }

        if (this.collisions <= 0)
        {
            this.collisions = 0;
            this.IsValid = true;
        }
    }
}