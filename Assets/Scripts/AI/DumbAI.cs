using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PolyNavAgent))]
public class DumbAI : MonoBehaviour {
    private GameObject player;

    private PolyNavAgent _agent;
	public PolyNavAgent agent{
		get
		{
			if (!_agent)
				_agent = GetComponent<PolyNavAgent>();
			return _agent;			
		}
	}

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("AI", 0f, 0.25f);
	}

    void AI()
    {
        this.agent.SetDestination(player.transform.position);
    }

    //Message from Agent
    void OnDestinationReached()
    {

        //do something here...
    }

    //Message from Agent
    void OnDestinationInvalid()
    {

    }

	void Update () {
	    
	}
}
