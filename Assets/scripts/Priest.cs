﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Character {

    private void Awake() {
        ConvertRadius = 1f;
        Invoke("Convert", 1.5f);
    }

    void FixedUpdate() {
        Movement();
        SetColor();
    }

    public override void Convert() {
        List<Character> toConvert = Infra.GetPeasantsInRange(this.transform.position, ConvertRadius);
        for (int i = 0; i < toConvert.Count; i++) {
            toConvert[i].SetAlliance(Alliance);
        }
        Invoke("Convert", 3f);
    }

}
