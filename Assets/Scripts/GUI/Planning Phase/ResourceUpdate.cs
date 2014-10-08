using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ResourceUpdate : MonoBehaviour {
    private Text resourceText;

    void Start()
    {
        GameController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.ResourceChanged += new GameController.ResourceChangedHandler(OnResourceChange);
        this.resourceText = this.GetComponent<Text>();
    }

    public void OnResourceChange(int newCount)
    {
        this.resourceText.text = newCount.ToString();
    }
}