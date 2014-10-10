using UnityEngine;
using System.Collections;

public class ParticleLayerAdjuster : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for(int i = 0; i < transform.childCount; i++) {
            GameObject child = transform.GetChild(i).gameObject;
            child.renderer.sortingLayerName = "Foreground";
            if (child.particleSystem != null)
            {
                child.particleSystem.renderer.sortingLayerName = "Foreground";
            }
            if (child.particleEmitter != null)
            {
                child.particleEmitter.renderer.sortingLayerName = "Foreground";
            }
        }

        this.renderer.sortingLayerName = "Foreground";
	}
	
}
