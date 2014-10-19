using UnityEngine;
using System.Collections;

public class BombTrap : TrapAction {
    [SerializeField]
    private GameObject prefab;

    private GameObject prefabInst;

    protected override IEnumerator Action()
    {
        prefabInst = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        prefabInst.transform.SetParent(this.transform);
        this.controller.GetComponent<AudioController>().PlaySFX(this.actionClip);
        yield return new WaitForSeconds(this.duration);
        yield return StartCoroutine(PostAction());
    }

    protected override IEnumerator PostAction()
    {
        GameObject.Destroy(prefabInst);
        return base.PostAction();
    }

    protected override void Reset()
    {
        GameObject.Destroy(prefabInst);
        this.inUse = false;
        this.StopAllCoroutines();
    }
}
