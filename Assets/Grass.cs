using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Terrain
{
    [SerializeField] private List<GameObject> treePrefab;
    [SerializeField, Range(0, 1)] private float treePropability;

    public void SettreeProbability(float newProbability)
    {
        this.treePropability = Mathf.Clamp01(newProbability);
    }
    public override void Generate(int size)
    {
        base.Generate(size);
        var limit = Mathf.FloorToInt((float) size / 2);
        var treeCount = Mathf.FloorToInt((float) size * treePropability);
        List<int> emptyPosition = new List<int>();
        for (int i = -limit; i <= limit; i++)
        {
            emptyPosition.Add(i);
        }

        for (int i = 0; i < treeCount; i++)
        {
            var randomIndex = Random.Range(0, emptyPosition.Count);
            var pos = emptyPosition[randomIndex];
            emptyPosition.RemoveAt(randomIndex);
            
           SpawnRandomTree(pos);
        }
        SpawnRandomTree(-limit -1);
        SpawnRandomTree(limit +1);
    }

    private void SpawnRandomTree(int pos)
    {
        var randomIndex = Random.Range(0, treePrefab.Count);
        var prefab = treePrefab[randomIndex];
        
        Instantiate(prefab, new Vector3(pos, 0, this.transform.position.z), Quaternion.identity, transform);
    }
}
