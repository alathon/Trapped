using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AI : MonoBehaviour {
    void Start() {
        InitializeAI();
        InvokeRepeating("AIRoutine", 0f, 0.1f);
    }

    protected virtual void InitializeAI()
    {

    }
    protected virtual void AIRoutine()
    {

    }
}
