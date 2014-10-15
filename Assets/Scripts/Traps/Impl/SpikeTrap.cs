﻿using UnityEngine;
using System.Collections;

public class SpikeTrap : TrapAction {
    [SerializeField]
    private GameObject prefab;

    private GameObject prefabInst;

    protected override IEnumerator Action()
    {
        prefabInst = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        prefabInst.transform.SetParent(this.transform);
        prefabInst.transform.RotateAroundLocal(Vector3.right, this.transform.rotation.z);
        yield return new WaitForSeconds(this.duration);
        yield return StartCoroutine(PostAction());
    }

    protected override IEnumerator PostAction()
    {
        GameObject.Destroy(prefabInst);
        return base.PostAction();
    }
}