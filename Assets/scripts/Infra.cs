using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Infra {

    private static int _id = 0;

    public static int GenerateId() {
        _id++;
        return _id;
    }
}
