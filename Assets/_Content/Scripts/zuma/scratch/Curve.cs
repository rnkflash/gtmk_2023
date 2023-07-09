using System;
using System.Collections.Generic;
using _Content.Scripts.zuma.scratch.bezier;
using UnityEngine;
using Random = System.Random;

namespace _Content.Scripts.zuma.scratch
{
    public class Curve : MonoBehaviour
    {
        private List<Slime> slimes;
        private CurvePoint[] points;

        public GameObject prefab;

        private void Start()
        {
            var path = GetComponent<PathPlacer>().CalculatePath();
            var list = new List<CurvePoint>();
            foreach (var p in path)
            {
                var cp = new CurvePoint()
                {
                    position = p
                };
                list.Add(cp);
            }
            points = list.ToArray();

            slimes = new List<Slime>();
            
            AddSlime(prefab, 0, 0);
            AddSlime(prefab, 1, 1);
            AddSlime(prefab, 2, 2);
            
            AddSlime(prefab, 4, 3);
            AddSlime(prefab, 5, 4);
            
            AddSlime(prefab, 17, 5);
        }

        public CurvePoint GetTile(int tile)
        {
            if (tile < 0 || tile >= points.Length)
                return null;
            return points[tile];
        }

        public void AddSlime(GameObject slimePrefab, int index, int listIndex = 0)
        {
            var p = GetTile(index);
            var gameObject = Instantiate(slimePrefab, p.position, Quaternion.identity);
            var slime = gameObject.GetComponent<Slime>();
            p.slime = slime;
            slimes.Insert(listIndex, slime);
            slime.tile = index;
            slime.curve = this;
            slime.SetType(UnityEngine.Random.Range(0, 4));
            slime.UpdateSortingOrder();
        }

        private void Update()
        {
            /*if (Input.anyKeyDown)
            {
                MoveSlimes();
            }
            */
        }

        public void MoveSlimes()
        {
            //immediately finish all previous movements by teleportation or something
            //iterate all slimes
            //each slime prepares to jump, back or idle
            //jump if its connected to first corps
            //back if not
            //idle if its corps but someone goes back
            //try to spawn new slime, dont spawn if tile is occupied
            
            //priority:
            //slime going back
            //slime going forward
            //spawning slime
            
            //iterate all slimes:
            //from last to first: set command jump back if free tile back, occupy tile
            //from first to last: set command jump forward if free tile ahead, occupy tile

            foreach (var slime in slimes)
            {
                slime.FinishMovements();
            }
            
            foreach (var slime in slimes)
            {
                if (!IsFirstCorps(slime) && !slime.moved)
                {
                    TryMoveSlime(slime, slime.tile - 1);
                }
            }
            
            slimes.Reverse();
            foreach (var slime in slimes)
            {
                if (IsFirstCorps(slime) && !slime.moved)
                    TryMoveSlime(slime, slime.tile + 1);
            }
            slimes.Reverse();

            if (GetTile(0).slime == null)
            {
                AddSlime(prefab, 0);
            }
            
            foreach (var slime in slimes)
            {
                if (!slime.moved)
                    slime.Jump();
            }
        }

        private bool IsFirstCorps(Slime slime)
        {
            var prevTileIndex = slime.tile - 1;
            if (prevTileIndex < 0)
                return true;
            var isFirstCorps = true;
            for (int i = 0; i <= prevTileIndex; i++)
            {
                if (GetTile(i).slime == null)
                {
                    isFirstCorps = false;
                    break;
                }
            }
            return isFirstCorps;
        }
        
        private void TryMoveSlime(Slime slime, int nextTileIndex)
        {
            if (nextTileIndex >= points.Length || nextTileIndex < 0)
                return;
            var nextTile = GetTile(nextTileIndex);
            var currentTile = GetTile(slime.tile);
            if (nextTile.slime == null)
            {
                currentTile.slime = null;
                nextTile.slime = slime;
                slime.tile = nextTileIndex;
                slime.Move(nextTile.position);
                slime.SetSortingOrder(10000 - (int)(nextTile.position.y * 10.0f));
            }
        }

        public void ChainKillSlimes(Slime slime)
        {
            var index = slimes.IndexOf(slime);

            var neighbours = new List<Slime>();
            if (index + 1 < slimes.Count)
            {
                int connectedTile = slime.tile;
                for (int i = index + 1; i < slimes.Count; i++)
                {
                    if (slimes[i].type == slime.type && Math.Abs(connectedTile - slimes[i].tile) <= 1)
                    {
                        neighbours.Add(slimes[i]);
                        connectedTile = slimes[i].tile;
                    }
                    else
                        break;
                }
            }

            if (index - 1 >= 0)
            {
                int connectedTile = slime.tile;
                for (int i = index - 1; i >= 0; i--)
                {
                    if (slimes[i].type == slime.type && Math.Abs(connectedTile - slimes[i].tile) <= 1)
                    {
                        neighbours.Add(slimes[i]);
                        connectedTile = slimes[i].tile;
                    }
                    else
                        break;
                }
            }

            KillSlime(slime);

            foreach (var slimeI in neighbours)
            {
                KillSlime(slimeI);
            }
        }

        private void KillSlime(Slime slime)
        {
            var tile = GetTile(slime.tile);
            tile.slime = null;
            slimes.Remove(slime);
            slime.curve = null;
            slime.isAlive = false;
            slime.GetComponentInChildren<Collider2D>().enabled = false;
            slime.DieWithAnimation();
            //Destroy(slime.gameObject);
        }
        
    }
}
