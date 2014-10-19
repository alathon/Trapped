using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OKClickSound : MonoBehaviour {
    private AudioController audioCtrl;

    void Start()
    {
        GameObject ctrl = GameObject.FindGameObjectWithTag("GameController");
        this.audioCtrl = ctrl.GetComponent<AudioController>();
        this.GetComponent<Button>().onClick.AddListener(() => { this.OnClick(); });
    }

    public void OnClick()
    {
        Debug.Log("Click!");
        this.audioCtrl.PlayButtonClickClip();
    }
}
