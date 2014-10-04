using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlanningPhaseUIManager : MonoBehaviour {
    private GameController controller;
    private Dictionary<TrapMetadata, GameObject> panels = new Dictionary<TrapMetadata,GameObject>();

    /// <summary>
    /// The prefab used for traps in the trap menu.
    /// </summary>
    [SerializeField]
    private GameObject trapMenuPrefab;

    /// <summary>
    /// The panel that contains traps (The trap menu panel).
    /// </summary>
    private GameObject trapPanel;

    void Awake()
    {
        this.trapPanel = GameObject.FindGameObjectWithTag("PlanningPhase_TrapsPanel");
        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        this.controller.TrapCountChanged += new GameController.TrapCountChangedHandler(OnTrapCountChanged);
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

    public void OnTrapCountChanged(TrapMetadata meta, int newCount)
    {
        if (newCount > 0)
        {
            // New trap.
            if (!this.panels.ContainsKey(meta))
            {
                GameObject gObj = (GameObject)Instantiate(this.trapMenuPrefab);
                gObj.transform.SetParent(trapPanel.transform, false);
                // Base offset = 25px; Size of actual trap widget is 35px, so a 5px buffer between each.
                gObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25 - (this.panels.Count * 40));
                this.panels.Add(meta, gObj);
                
            }

            GameObject trap = this.panels[meta];

            trap.GetComponent<Button>().onClick.AddListener(delegate { OnClickTrapBar(meta.trapName); });
            // Alternative way:
            // UnityEngine.Events.UnityAction action = () => { OnClickTrapBar(meta.name); };
            // trap.GetComponent<Button>().onClick.AddListener(action);
            foreach (Image img in trap.GetComponentsInChildren<Image>())
            {
                if (img.gameObject == trap) continue;
                img.sprite = meta.menuSprite;
            }
            foreach (Text txt in trap.GetComponentsInChildren<Text>())
            {
                if (txt.gameObject == trap) continue;
                txt.text = newCount.ToString();
            }
        }
        else
        {
            if (this.panels.ContainsKey(meta))
            {
                GameObject gObj = this.panels[meta];
                this.panels.Remove(meta);
                GameObject.Destroy(gObj);
            }
        }
    }
}