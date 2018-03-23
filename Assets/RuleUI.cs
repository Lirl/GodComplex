using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleUI : MonoBehaviour {

    public Rule Rule;

    public Text Headline { get; private set; }
    public Text Description { get; private set; }
    public Button ChooseButton;

    public void SetRule(Rule r) {
        Rule = r;

        Headline = Infra.FindComponentInChildWithTag<Text>(gameObject, "Text");
        Description = Infra.FindComponentInChildWithTag<Text>(gameObject, "Description");
        ChooseButton = Infra.FindComponentInChildWithTag<Button>(gameObject, "Button");

        ChooseButton.onClick.AddListener(_informBoard);

        Headline.text = r.Name;
        Description.text = r.Description;

        var _image = transform.GetComponent<Image>();
        if(r.Image != null) {
            Debug.LogError("Attempting to create image " + r.Image);
            _image.sprite = Instantiate(Resources.Load<Sprite>(r.Image));
        }
    }

    private void _informBoard() {
        EventManager.TriggerEvent("ExtraRuleChosen", new Hashtable() { { "Rule", Rule } });
    }
}
