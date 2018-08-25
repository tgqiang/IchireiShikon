using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOfFriendshipEffect : SpiritEffect {

    const int POSITION_OFFSET_X = -8;
    const int POSITION_OFFSET_Y = 4;
    ParticleSystem emitter;

    void Awake() {
        emitter = GetComponent<ParticleSystem>();
        if (emitter.IsTNull()) {
            throw new System.Exception("Particle emitter missing from Spirit of Friendship effect object.");
        }
    }

    void Update() {
        if (GetComponent<SpriteRenderer>().enabled) {
            Vector2Int mapBounds = Tile.gameLevel.levelData.mapBounds;
            int c = Mathf.FloorToInt(transform.position.x - POSITION_OFFSET_X);
            int r = Mathf.FloorToInt(POSITION_OFFSET_Y - transform.position.y);

            if (r.IsBetweenExclusive(0, mapBounds.x) && c.IsBetweenExclusive(0, mapBounds.y)) {
                Tile t = Tile.gameLevel.levelData.tileMap[r][c];
                if (t != null && t.IsVacant(GetComponentInParent<Mergeable>())) {
                    if (!emitter.isEmitting) {
                        emitter.Play();
                    }
                } else {
                    if (emitter.isEmitting) {
                        emitter.Stop();
                    }
                }
            } else {
                if (emitter.isEmitting) {
                    emitter.Stop();
                }
            }
        }
    }

    public void RevealEffectMarker() {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void HideEffectMarker() {
        emitter.Stop();
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void PerformEffect(Tile target) {
        if (target.IsVacant()) {
            int objectLevel = UnityEngine.Random.Range(0, GetComponentInParent<Spirit>().SpiritLevel);
            int objectType = 0;
            if (objectLevel > 0) {
                objectType = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CustomEnums.SpiritType)).Length);
                Spirit spawnedSpirit = FindObjectOfType<ObjectSpawner>().SpawnSpirit(objectType, objectLevel, transform.position).GetComponent<Spirit>();
                spawnedSpirit.CurrentLocation = target;
                Tile.PlaceOnTile(spawnedSpirit, target);
            } else {
                objectType = UnityEngine.Random.Range(0, Enum.GetNames(typeof(CustomEnums.SoulType)).Length);
                Soul spawnedSoul = FindObjectOfType<ObjectSpawner>().SpawnSoul(objectType, transform.position).GetComponent<Soul>();
                spawnedSoul.CurrentLocation = target;
                Tile.PlaceOnTile(spawnedSoul, target);
            }
        }

        target.Purify();
    }
}
