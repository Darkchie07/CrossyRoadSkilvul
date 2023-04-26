using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private List<Terrain> terrainList = new List<Terrain>();
    [SerializeField] private List<Coin> coinList = new List<Coin>();
    [SerializeField] private int initialGrassCount = 5;
    [SerializeField] private int horizontalSize;
    [SerializeField] private int backViewDistance = -4;
    [SerializeField] private int forwardViewDistance = 15;
    [SerializeField] private float initialTimer = 10;
    [SerializeField] private int coin;
    private Dictionary<int, Terrain> activeTerrainDict = new Dictionary<int, Terrain>(20);
    [SerializeField] private int travelDistance;
    public UnityEvent<int, int> OnUpdateTerrainLimit;
    public UnityEvent<int> OnScoreUpdate;

    private void Start()
    {
        for (int i = backViewDistance; i < initialGrassCount; i++)
        {
            var terrain = Instantiate(terrainList[0]);
            terrain.transform.localPosition = new Vector3(0, 0, i);
            if (terrain is Grass grass)
            {
                grass.SettreeProbability(i < -1 ? 1 : 0);
            }
            terrain.Generate(horizontalSize);
            activeTerrainDict[i] = terrain;
        }

        for (int i = initialGrassCount; i < forwardViewDistance; i++)
        {
            SpawnRandomTerrain(i);
        }
        
        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistance);
    }

    private Terrain SpawnRandomTerrain(int zPos)
    {
        Terrain terrainCheck = null;
        int randomIndex;
        Terrain terrain = null;
        for (int i = -1; i >= -3; i--)
        {
            var checkPos = zPos + i;
            if (terrainCheck == null)
            {
                terrainCheck = activeTerrainDict[checkPos];
                continue;
            }
            else if (terrainCheck.GetType() != activeTerrainDict[checkPos].GetType())
            {
                randomIndex = UnityEngine.Random.Range(0, terrainList.Count);
                SpawnTerrain(terrainList[randomIndex], zPos);
                return terrain;
            }
            else
            {
                continue;
            }
        }

        var candidateTerrain = new List<Terrain>(terrainList);
        for (int i = 0; i < candidateTerrain.Count; i++)
        {
            if (terrainCheck.GetType() == candidateTerrain[i].GetType())
            {
                candidateTerrain.Remove(candidateTerrain[i]);
                break;
            }
        }
        randomIndex = UnityEngine.Random.Range(0, candidateTerrain.Count);
        return SpawnTerrain(candidateTerrain[randomIndex], zPos);
    }

    public Terrain SpawnTerrain(Terrain terrain, int zPos)
    {
        terrain = Instantiate(terrain);
        terrain.transform.position = new Vector3(0, 0, zPos);
        terrain.Generate(horizontalSize);
        activeTerrainDict[zPos] = terrain;
        SpawnCoin(horizontalSize, zPos);
        return terrain;
    }

    public Coin SpawnCoin(int horizontalSize, int zPos, float probability = 0.2f)
    {
        if (probability == 0)
        {
            return null;
        }

        List<Vector3> spawnPosCandidateList = new List<Vector3>();
        for (int i = -horizontalSize/2; i <= horizontalSize/2; i++)
        {
            var spawnPos = new Vector3(i, 0, zPos);
            if (Tree.AllPositions.Contains(spawnPos) == false)
            {
                spawnPosCandidateList.Add(spawnPos);
            }
        }
        
        if (probability >= UnityEngine.Random.value)
        {
            var index = UnityEngine.Random.Range(0, coinList.Count);
            var spawnPosIndex = UnityEngine.Random.Range(0, spawnPosCandidateList.Count);
            return Instantiate(coinList[index], spawnPosCandidateList[spawnPosIndex], Quaternion.identity);
        }

        return null;
    }

    public void UpdateTravelDistance(Vector3 targetPosition)
    {
        if (targetPosition.z > travelDistance)
        {
            travelDistance = Mathf.CeilToInt(targetPosition.z);
            UpdateTerrain();
            OnScoreUpdate.Invoke(GetScore());
        }
    }

    public void AddCoin(int value = 1)
    {
        this.coin += value;
    }

    private int GetScore()
    {
        return travelDistance + coin;
    }

    public void UpdateTerrain()
    {
        var destroyPos = travelDistance - 1 + backViewDistance;
        Destroy(activeTerrainDict[destroyPos].gameObject);
        activeTerrainDict.Remove(destroyPos);

        var spawnPosition = travelDistance - 1 + forwardViewDistance;
        SpawnRandomTerrain(spawnPosition);
        
        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistance);
    }
}
