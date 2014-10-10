using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndPhase : MonoBehaviour {
	void Start () {
        GameObject gObj = GameObject.FindGameObjectWithTag("PlanningPhase_GUI");
        this.GetComponent<Button>().onClick.AddListener(() => { gObj.GetComponent<PlanningPhaseUIManager>().OnClickEndPhaseBtn(); });
	}
}
