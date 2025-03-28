using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Useful
{
    public static Button Button(this GameObject g) => g.GetComponent<Button>();

    public static Vector3[] Positions(this GameObject[] gs)
    {
        List<Vector3> vs = new List<Vector3>();
        foreach (GameObject g in gs)
        {
            Vector3 v = g.transform.position;
            vs.Add(new Vector3(v.x, v.y, v.z));
        }

        return vs.ToArray();
    }

    public static T[] Randomize<T>(this T[] arr)
    {
        List<T> ts = new List<T>();
        List<int> index = new List<int>(), ni = new List<int>(); //ni- new indexes
        for (int i = 0; i < arr.Length; i++) index.Add(i);
        while (index.Count > 0)
        {
            int ind = Random.Range(0, index.Count);
            ni.Add(index[ind]);
            index.RemoveAt(ind);
        }

        foreach (int n in ni)
        {
            ts.Add(arr[n]);
        }

        return ts.ToArray();
    }

    public static bool HasComponentInChildren<T>(this GameObject g, bool ParentHasToo = true)
    {
        T[] x = g.GetComponentsInChildren<T>();
        if (x == null || x.Length < 2 && ParentHasToo)
        {
            return false;
        }

        return true;
    }

    public static Vector3 Copy(this Vector3 v) => new Vector3(v.x, v.y, v.z);

    public static Vector3[] Copy(this Vector3[] vs)
    {
        List<Vector3> nv = new List<Vector3>();
        foreach (Vector3 v in vs)
        {
            nv.Add(new Vector3(v.x, v.y, v.z));
        }

        return nv.ToArray();
    }

    public static void SetActive(this GameObject[] gs, bool b)
    {
        foreach (GameObject g in gs)
        {
            g.SetActive(b);
        }
    }

    public static IEnumerator TranslateOverTime(this GameObject g, int milliseconds, Vector3 movement)
    {
        for (int i = 0; i < milliseconds; i++)
        {
            g.transform.Translate(movement / milliseconds, Space.World);
            yield return new WaitForSeconds(.001f);
        }
    }

    public static IEnumerator TranslateOverTime(this GameObject g, Vector3 target, int milliseconds)
    {
        Vector3 movement = target - g.transform.position;
        for (int i = 0; i < milliseconds; i++)
        {
            g.transform.Translate(target / milliseconds, Space.World);
            yield return new WaitForSecondsRealtime(.001f);
        }
    }
}