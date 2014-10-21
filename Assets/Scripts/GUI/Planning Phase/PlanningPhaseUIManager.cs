using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlanningPhaseUIManager : MonoBehaviour {
    private GameController controller;
    private Dictionary<string, GameObject> panels = new Dictionary<string,GameObject>();

    /// <summary>
    /// The prefab used for traps in the trap menu.
    /// </summary>
    [SerializeField]
    private GameObject trapMenuPrefab;

    /// <summary>
    /// The panel that contains traps (The trap menu panel).
    /// </summary>
    private GameObject trapPanel;
    private GameObject planningPhaseGUI;

    void ShowStartBtn()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        GameObject startBtn = canvas.transform.Find("Planning Phase/StartBtn").gameObject;
        GameObject startBtnText = startBtn.transform.Find("Text").gameObject;

        startBtn.GetComponent<Button>().interactable = true;
        Color prevColor = startBtn.GetComponent<Image>().color;
        startBtn.GetComponent<Image>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 1f);

        prevColor = startBtnText.GetComponent<Text>().color;
        startBtnText.GetComponent<Text>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 1f);
    }

    void HideStartBtn()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        GameObject startBtn = canvas.transform.Find("Planning Phase/StartBtn").gameObject;
        GameObject startBtnText = startBtn.transform.Find("Text").gameObject;

        startBtn.GetComponent<Button>().interactable = false;
        Color prevColor = startBtn.GetComponent<Image>().color;
        startBtn.GetComponent<Image>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 0f);

        prevColor = startBtnText.GetComponent<Text>().color;
        startBtnText.GetComponent<Text>().color = new Color(prevColor.r, prevColor.g, prevColor.b, 0f);
    }

    void Start()
    {
        this.trapPanel = GameObject.FindGameObjectWithTag("PlanningPhase_TrapsPanel");
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.TrapCountChanged += new GameController.TrapCountChangedHandler(OnTrapCountChanged);
        this.controller.OutOfTraps += new GameController.OutOfTrapsHandler(OnOutOfTraps);
    }

    public void ActivateGUI()
    {
        this.GetComponent<CanvasGroup>().alpha = 1f;
        this.GetComponent<CanvasGroup>().interactable = true;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void DeactivateGUI()
    {
        this.GetComponent<CanvasGroup>().alpha = 0f;
        this.GetComponent<CanvasGroup>().interactable = false;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnOutOfTraps()
    {
        this.ShowStartBtn();
    }

    public void OnClickEndPhaseBtn()
    {
        this.controller.PlanningPhase_OnClickEndPhaseBtn();
    }

    /// <summary>
    /// Called when player clicks a trap on the trap bar.
    /// </summary>
    /// <param name="trapName">
    /// The name of the trap being clicked.
    /// </param>
    public void OnClickTrapBar(string trapName)
    {
        this.controller.PlanningPhase_OnClickTrapBar(trapName);
    }

    /// <summary>
    /// Called when the number of available traps of a specific kind changes.
    /// </summary>
    /// <param name="meta">
    /// The trap metadata.
    /// </param>
    /// <param name="newCount">
    /// The amount of traps.
    /// </param>
    public void OnTrapCountChanged(TrapMetadata metadata, int added, int newCount)
    {
        if (added > 0)
        {
            this.HideStartBtn();
        }

        string trapName = metadata.trapName;
        if (newCount > 0)
        {
            // New trap.
            if (!this.panels.ContainsKey(trapName))
            {
                GameObject gObj = (GameObject)Instantiate(this.trapMenuPrefab);

                // Move to correct parent canvas object.
                gObj.transform.SetParent(trapPanel.transform, false);

                // Register click handler.
                gObj.GetComponent<Button>().onClick.AddListener(delegate { OnClickTrapBar(trapName); });

                // Alternative way:
                // UnityEngine.Events.UnityAction action = () => { OnClickTrapBar(meta.name); };
                // trap.GetComponent<Button>().onClick.AddListener(action);

                // Set position on screen.
                // Base offset = 45px; Size of actual trap widget is 35px, so a 5px buffer between each = 40px.
                gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -45 - (this.panels.Count * 40));

                // Set child image (actual trap image) to proper menu sprite item.
                foreach (Image img in gObj.GetComponentsInChildren<Image>())
                {
                    if (img.gameObject == gObj) continue;
                    img.sprite = metadata.menuSprite;
                }
                this.panels.Add(trapName, gObj);   
            }

            GameObject trap = this.panels[trapName];

            // Change text in case count has changed.
            foreach (Text txt in trap.GetComponentsInChildren<Text>())
            {
                if (txt.gameObject == trap) continue;
                txt.text = newCount.ToString();
            }
        }
        else
        {
            // Remove trap.
            if (this.panels.ContainsKey(trapName))
            {
                GameObject gObj = this.panels[trapName];
                this.panels.Remove(trapName);
                GameObject.Destroy(gObj);
            }

            // Re-order existing, to prevent gaps in the trap menu.
            int count = 0;
            foreach (KeyValuePair<string, GameObject> entry in this.panels)
            {
                entry.Value.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25 - (count * 40));
                count += 1;
            }
        }
    }
}