using UnityEngine;
using System.Collections;

public class OrthographicSize : MonoBehaviour {
    [SerializeField]
    private float pixelPerUnit = 100f;

    public void Awake()
    {
        this.camera.orthographicSize = 2;
        //this.camera.orthographicSize = (Screen.height / pixelPerUnit / 2.0f); // 100f is the PixelPerUnit that you have set on your sprite. Default is 100.
    }
}
