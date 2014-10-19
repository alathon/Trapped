using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public enum Phase { Planning, Action };

    class GameState
    {
        public Phase currentPhase = Phase.Planning;

        public int currentLevel = 1;
        public int maxLevel = 4;

        public int currentWave = 1;
        public int maxWave = 0;

        // Action Phase
        public int spawnersLeft = 0;

        // Planning Phase
        public int mana = 0;

        public GameObject CurrentPlacementPrefab;
        private Dictionary<string, int> unplacedTraps = new Dictionary<string, int>();

        public bool OutOfTrapsToPlace()
        {
            return unplacedTraps.Keys.Count <= 0;
        }

        public int AddTraps(String name, int count)
        {
            if (!this.unplacedTraps.ContainsKey(name))
            {
                this.unplacedTraps.Add(name, count);
            } else {
                this.unplacedTraps[name] += count;
            }
            return this.unplacedTraps[name];
        }

        public int RemoveTraps(String name, int count)
        {
            if (this.unplacedTraps.ContainsKey(name))
            {
                this.unplacedTraps[name] -= count;
                if (this.unplacedTraps[name] <= 0)
                {
                    this.unplacedTraps.Remove(name);
                    return 0;
                }
                return this.unplacedTraps[name];
            }

            return 0;
        }
    }
}
