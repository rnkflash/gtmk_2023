using System;
using System.Collections;
using System.Collections.Generic;
using _Content.Scripts.rhythm;
using UnityEngine;

public class LaneVisualizator : MonoBehaviour
{
    public GameObject notePrefab;
    public Lane lane;

    private List<GameObject> notes = new List<GameObject>();

    private float noteMoveSpeed = 333.0f;
    private float playLine = -333.0f;
    private float terminatorLine = -524.0f;

    private bool playing = false;

    public void ImportLane()
    {
        if (lane.timeStamps.Count <= 0)
            return;
        
        foreach (var timeStamp in lane.timeStamps)
        {
            var note = Instantiate(notePrefab, transform);
            note.transform.localPosition = new Vector3(0.0f,playLine + noteMoveSpeed * (float)timeStamp,0.0f);
            notes.Add(note);
        }
    }

    public void StartSong()
    {
        playing = true;
    }

    private void Update()
    {
        if (!playing)
            return;
        if (notes.Count <= 0)
            return;
        var destructionList = new List<GameObject>();
        foreach (var note in notes)
        {
            note.transform.Translate(Vector3.down * Time.deltaTime * noteMoveSpeed, Space.Self);
            if (note.transform.position.y < terminatorLine)
            {
                destructionList.Add(note);
            }
        }

        if (destructionList.Count > 0)
        {
            foreach (var note in destructionList)
            {
                Destroy(note);
                notes.Remove(note);
            }
        }
    }
}
