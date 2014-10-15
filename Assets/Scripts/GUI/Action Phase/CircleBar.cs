using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CircleBar : MonoBehaviour {
    [SerializeField]
    Color start;
    [SerializeField]
    Color end;

    private Image Circle;

    private float startTime;
    private float curTime;

    void Awake()
    {
        this.Circle = this.GetComponent<Image>();
        this.curTime = 100f;
        this.startTime = 100f;
        Circle.material.SetFloat("_Angle", Mathf.Lerp(-3.14f, 3.14f, 0.5f));
        Circle.material.SetColor("_Color", Color.Lerp(start, end, 0.5f));
    }

    public void OnActionDone(float reuseTime)
    {
        Debug.Log("Got message.");
        this.curTime = reuseTime;
        this.startTime = reuseTime;
        this.enabled = true;
        this.Circle.material.color = new Color(255, 255, 255, 1);
    }

    void Deactivate()
    {
        this.enabled = false;
        this.curTime = 0f;
        this.startTime = 0f;
        this.Circle.material.color = new Color(255, 255, 255, 0);
    }

    void Update()
    {
        this.curTime -= Time.deltaTime;
        float value = Mathf.Max(0, this.curTime / this.startTime); 
        if (value <= 0)
        {
            this.Deactivate();
        }
        else
        {
            Circle.material.SetFloat("_Angle", Mathf.Lerp(-3.14f, 3.14f,value));
            Circle.material.SetColor("_Color", Color.Lerp(start, end, value));
        }
    }
}