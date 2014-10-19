using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
    [SerializeField]
    private GameObject planningPhasePrefab;

    [SerializeField]
    private GameObject actionPhasePrefab;

    [SerializeField]
    private GameObject bombTrapPrefab;

    [SerializeField]
    private GameObject spikeTrapPrefab;

    [SerializeField]
    private GameObject fireballTrapPrefab;

    public void OnCloseTutorialWnd(GameObject wnd)
    {
        GameObject.Destroy(wnd);
    }

    public void ShowBombTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(bombTrapPrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    public void ShowSpikeTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(spikeTrapPrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    public void ShowFireballTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(fireballTrapPrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    public void ShowPlanningPhaseTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(planningPhasePrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    public void ShowActionPhaseTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(actionPhasePrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    public void FirstTimeTrap(GameObject gObj)
    {
        if (gObj.name.Equals("Bomb Trap"))
        {
            this.ShowBombTutorial();
        }
        else if (gObj.name.Equals("Spike Trap"))
        {
            this.ShowSpikeTutorial();
        }
        else if (gObj.name.Equals("Fireball Trap"))
        {
            this.ShowFireballTutorial();
        }
    }
}
