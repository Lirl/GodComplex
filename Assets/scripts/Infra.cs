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

    public static List<Character> GetEnemiesInRange(Vector3 position, float radius) {
        List<Character> res = new List<Character>();
        var current = GameObject.FindGameObjectsWithTag("Peasants");

        if (current.Length == 0) {
            Debug.LogError("Enemies is empty");
        }

        for (int i = 0; i < current.Length; i++) {
            // Check creature in radius
            if ((current[i].transform.position - position).magnitude <= radius) {
                var e = current[i].GetComponent<Character>();
                if (e) {
                    res.Add(current[i].GetComponent<Character>());
                }
                else {
                    Debug.LogWarning("Could not get script Character from an object in Game.GetInstance().Enemies");
                }
            }
        }

        return res;
    }

}
