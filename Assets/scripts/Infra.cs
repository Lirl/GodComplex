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

    public static List<Character> GetPeasantsInRange(Vector3 position, float radius) {
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

    public static T FindComponentInChildWithTag<T>(GameObject parent, string tag) where T : Component {
        Transform t = parent.transform;
        foreach (Transform tr in t) {
            if (tr.tag == tag) {
                return tr.GetComponent<T>();
            }
        }
        return null;
    }

    public static List<T> FindComponentsInChildWithTag<T>(GameObject parent, string tag) where T : Component {
        Transform t = parent.transform;
        List<T> res = new List<T>();
        foreach (Transform tr in t) {
            if (tr.tag == tag) {
                res.Add(tr.GetComponent<T>());
            }
        }
        return res;
    }
}
