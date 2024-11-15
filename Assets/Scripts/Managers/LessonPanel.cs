using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LessonPanel : MonoBehaviour
{
    [Serializable]
    public struct Lessons
    {
        public string Name;
    }

    [SerializeField] Lessons[] allLessons;

    void Start()
    {
        GameObject LessonButton = transform.GetChild (0).gameObject;
        GameObject l;

        int N = allLessons.Length;

        for (int i = 0; i < N; i++)
        {
            l = Instantiate (LessonButton, transform);
            /*l.transform.Find("LessonTitle") = allLessons[i].Name;*/
            l.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = allLessons[i].Name;
        }

        Destroy(LessonButton);
    }
}
