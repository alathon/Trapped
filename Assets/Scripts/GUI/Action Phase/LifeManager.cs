using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeManager : MonoBehaviour {
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite halfHeart;
    [SerializeField]
    private Sprite emptyHeart;
    [SerializeField]
    private int numberOfHearts;
    [SerializeField]
    private GameObject heartImagePrefab;

    private GameObject[] hearts;

    private Sprite GetSprite(int i)
    {
        if (i <= 0) return emptyHeart;
        else if (i == 1) return halfHeart;
        else return fullHeart;
    }

    // Each heart is worth 2 points, filling from the
    // left to the right.
    void SetLife(int newLife)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            int modifier = i*2;
            Sprite heart = this.GetSprite(newLife - modifier);
            this.hearts[i].GetComponent<Image>().sprite = heart;
        }
    }

    void ChangeLifeMax(int newMax)
    {
        this.numberOfHearts = newMax / 2;
        this.hearts = new GameObject[this.numberOfHearts];
        for (int i = 0; i < this.hearts.Length; i++)
        {
            GameObject heartObj = (GameObject)Instantiate(this.heartImagePrefab);
            heartObj.transform.SetParent(this.transform, false);
            heartObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(10 + (i * 32), 0);
        }
    }

	void Start () {
        GameObject gObj = GameObject.FindGameObjectWithTag("GameController");
        GameController controller = gObj.GetComponent<GameController>();
        this.ChangeLifeMax(controller.GetLifeMax());
        controller.LifeChanged += new GameController.LifeChangedHandler(OnLifeChanged);
	}

    public void OnLifeChanged(int newLife)
    {
        this.SetLife(newLife);
    }
}
