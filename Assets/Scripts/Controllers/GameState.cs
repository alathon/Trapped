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
        public GameObject CurrentPlacementPrefab;
        private Dictionary<TrapMetadata, int> unplacedTraps = new Dictionary<TrapMetadata,int>();

        public int AddTraps(TrapMetadata metadata, int count)
        {
            if (!this.unplacedTraps.ContainsKey(metadata))
            {
                this.unplacedTraps.Add(metadata, count);
            } else {
                this.unplacedTraps[metadata] += count;
            }
            return this.unplacedTraps[metadata];
        }

        public int RemoveTraps(TrapMetadata metadata, int count)
        {
            if (this.unplacedTraps.ContainsKey(metadata))
            {
                this.unplacedTraps[metadata] -= count;
                if (this.unplacedTraps[metadata] <= 0)
                {
                    this.unplacedTraps.Remove(metadata);
                    return 0;
                }
                return this.unplacedTraps[metadata];
            }

            return 0;
        }
    }
}
