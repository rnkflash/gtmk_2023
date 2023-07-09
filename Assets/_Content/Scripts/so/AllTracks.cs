using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace _Content.Scripts.so
{
    public class AllTracks : Singleton<AllTracks>
    {
        public Track[] tracks;
	
        protected override void Created()
        {
            base.Created();
            tracks = Resources.LoadAll<Track>("tracks");

        }

        public Track GetTrack(string id)
        {
            return Array.Find(tracks, element => element.id == id);
        }
    }
}