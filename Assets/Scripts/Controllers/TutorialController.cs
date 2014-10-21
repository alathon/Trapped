using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Controllers;

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

    private GameController controller;
    void Start()
    {
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.TrapCountChanged += new GameController.TrapCountChangedHandler(OnTrapCountChanged);
        this.controller.WaveChanged += new GameController.WaveChangedHandler(OnWaveChanged);
        this.controller.PhaseChanged += new GameController.PhaseChangedHandler(OnPhaseChanged);
    }

    IEnumerator StartPlanningTutorial()
    {
        GameObject mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        yield return new WaitForSeconds(0.5f);
        GameObject gObj = (GameObject)Instantiate(Resources.Load("Movement Popup"));
        gObj.transform.SetParent(mainCanvas.transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);
        yield return new WaitForSeconds(6f);
        gObj = (GameObject)Instantiate(Resources.Load("Click Popup"));
        gObj.transform.SetParent(mainCanvas.transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);
        yield return new WaitForSeconds(6f);
        gObj = (GameObject)Instantiate(Resources.Load("Place Traps Popup"));
        gObj.transform.SetParent(mainCanvas.transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-55, 450);
    }

    IEnumerator StartActionTutorial()
    {
        GameObject mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        yield return new WaitForSeconds(0.5f);
        GameObject gObj = (GameObject)Instantiate(Resources.Load("Activate Trap Popup"));
        gObj.transform.SetParent(mainCanvas.transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -120);
        yield return new WaitForSeconds(6f);
    }

    public void OnPhaseChanged(Phase newPhase)
    {
        if (newPhase == Phase.Action && this.controller.GetCurrentWave() == 1 && Application.loadedLevelName.Equals("1"))
        {
            StartCoroutine(StartActionTutorial());
        }
    }

    public void OnWaveChanged(int waveNum)
    {
        string level = Application.loadedLevelName;
        if (level.Equals("1") && waveNum == 1)
        {
            StartCoroutine(StartPlanningTutorial());
            //StartCoroutine(ShowPlanningPhaseTutorial());
        }
    }

    public void OnTrapCountChanged(TrapMetadata trap, int change, int newTotal)
    {
        if (newTotal == change && change > 0)
        {
            if (trap.name.Equals("Bomb Trap"))
            {
                this.ShowBombTutorial();
            }
            else if (trap.name.Equals("Spike Trap"))
            {
                this.ShowSpikeTutorial();
            }
            else if (trap.name.Equals("Fireball Trap"))
            {
                this.ShowFireballTutorial();
            }
        }
    }

    public void OnCloseTutorialWnd(GameObject wnd)
    {
        GameObject.Destroy(wnd);
    }

    void ShowBombTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(bombTrapPrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    void ShowSpikeTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(spikeTrapPrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    void ShowFireballTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(fireballTrapPrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    IEnumerator ShowPlanningPhaseTutorial()
    {
        yield return new WaitForSeconds(0.05f);
        GameObject gObj = (GameObject)Instantiate(planningPhasePrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }

    void ShowActionPhaseTutorial()
    {
        GameObject gObj = (GameObject)Instantiate(actionPhasePrefab);
        gObj.transform.SetParent(GameObject.FindGameObjectWithTag("MainCanvas").transform);
        gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        GameObject okBtn = gObj.transform.Find("OKBtn").gameObject;
        okBtn.GetComponent<Button>().onClick.AddListener(() => { this.OnCloseTutorialWnd(gObj); });
    }
}
