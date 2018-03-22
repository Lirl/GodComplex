using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    public static string[,] cardImages = new string[4, 13] {
        { "carddeck_34", "carddeck_44", "carddeck_55", "carddeck_2", "carddeck_9", "carddeck_17", "carddeck_26", "carddeck_35", "carddeck_45", "carddeck_56", "carddeck_3", "carddeck_10", "carddeck_18" },
        { "carddeck_21", "carddeck_30", "carddeck_40", "carddeck_51", "carddeck_13", "carddeck_22", "carddeck_31", "carddeck_41", "carddeck_52", "carddeck_6", "carddeck_14", "carddeck_23", "carddeck_32" },
        { "carddeck_42", "carddeck_53", "carddeck_0", "carddeck_7",  "carddeck_15", "carddeck_24", "carddeck_33", "carddeck_43", "carddeck_54", "carddeck_1", "carddeck_8", "carddeck_16", "carddeck_25" },
        { "carddeck_27", "carddeck_36", "carddeck_46", "carddeck_57", "carddeck_4", "carddeck_11", "carddeck_19", "carddeck_28", "carddeck_37", "carddeck_47", "carddeck_5", "carddeck_12", "carddeck_20" }
    };

    public int Owner = 0; // Id of player owning the card, where 0 is no player

    public SuitEnum Suit;
    public int Rank;

    public bool FaceUp = true;
    private Image _image;

    private static Sprite[] carddeck;

    public void Init() {
        if (carddeck == null) {
            carddeck = Resources.LoadAll<Sprite>("carddeck");
        }

        _image = this.transform.GetComponent<Image>();
        var resource = carddeck.Where(c => c.name == "carddeck_50").ToList()[0];
        _image.sprite = Instantiate(resource);
        Debug.LogError("card init");
    }

    public void Set(SuitEnum suit, int rank) {
        Rank = rank;
        Suit = suit;
        if (FaceUp) {
            _image.sprite = carddeck.Where(c => c.name == cardImages[(int)suit - 1, rank - 1]).ToList()[0];
        }
    }

    /*public void ToggleCard() {
        if(FaceUp) {
            _image.sprite = Resources.Load("carddeck_50") as Sprite;
        }
        else {
            _image.sprite = Resources.Load(cardImages[(int)suit - 1, rank - 1]) as Sprite;
        }
    }*/
}