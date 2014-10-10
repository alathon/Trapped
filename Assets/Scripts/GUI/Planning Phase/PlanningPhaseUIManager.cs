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
    private CanvasGroup canvasGroup;

    void Start()
    {
        this.planningPhaseGUI = GameObject.FindGameObjectWithTag("PlanningPhase_GUI");
        this.trapPanel = GameObject.FindGameObjectWithTag("PlanningPhase_TrapsPanel");
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.TrapCountChanged += new GameController.TrapCountChangedHandler(OnTrapCountChanged);
        this.canvasGroup = this.planningPhaseGUI.GetComponent<CanvasGroup>();
    }

    public void ActivateGUI()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void DeactivateGUI()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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
    public void OnTrapCountChanged(TrapMetadata metadata, int newCount)
    {
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
                // Base offset = 25px; Size of actual trap widget is 35px, so a 5px buffer between each.
                gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25 - (this.panels.Count * 40));

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