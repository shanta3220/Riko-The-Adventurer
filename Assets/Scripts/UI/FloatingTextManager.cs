using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{

    // public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    public static FloatingTextManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }


    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);
        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(transform);
            txt.txt = txt.go.GetComponent<Text>();
            floatingTexts.Add(txt);
        }
        return txt;
    }

    public void Show(string msg, int fontSize, Color color, Vector3 postion, Vector3 motion, float duration)
    {
        FloatingText floatingText = GetFloatingText();
        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;
        //transfer world space to screen space so that we can use it in the UI
        floatingText.go.transform.position = Camera.main.WorldToScreenPoint(postion);
        //floatingText.go.transform.position =postion;
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();

    }

    public void CleanList()
    {
        floatingTexts.Clear();
    }

}
