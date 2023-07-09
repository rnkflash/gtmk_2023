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

    [SerializeField] private float noteMoveSpeed = 5.0f;
    [SerializeField] private float playLine = 0f;
    [SerializeField] private float terminatorLine = -0.25f;
    [SerializeField] private float appearLine = 5.0f;

    public ParticleSystem vfxGood;
    public ParticleSystem vfxBad;

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
            note.SetActive(false);
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
            
            if (note.transform.localPosition.y < appearLine && !note.activeSelf)
            {
                note.SetActive(true);
                
            }
            
            if (note.transform.localPosition.y < terminatorLine)
            {
                destructionList.Add(note);
                if (vfxGood != null)
                {
                    vfxGood.Emit(1);
                }
            }
        }

        if (destructionList.Count > 0)
        {
            foreach (var note in destructionList)
            {
                Destroy(note);
                notes.Remove(note);
                //vfxBad.Emit(1);
            }
        }
    }
}
