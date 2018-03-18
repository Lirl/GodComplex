using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Infra {

    private static int _id = 0;

    public static int GenerateId() {
        _id++;
        return _id;
    }
    public static Vector3 RandomVectorInRange(float xMin, float xMax, float yMin, float yMax) {
        return new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax));
    }

}
