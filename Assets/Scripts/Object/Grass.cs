using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Terrain
{
    [SerializeField] List<GameObject> obstaclePrefabList;
    [SerializeField, Range(0,1)] float treeProbability;

    public void SetTreePercentage(float newProbability)
    {
        this.treeProbability = Mathf.Clamp01(newProbability);
    }
    
    public override void Generate(int size)
    {
        base.Generate(size);

        var limit = Mathf.FloorToInt((float) size / 2);
        var treeCount = Mathf.FloorToInt((float) size * treeProbability);

        // membuat daftar posisi yang masih kosong
        List<int> emptyPosition = new List<int>();
        for (int i = -limit; i <= limit; i++)
        {
            emptyPosition.Add(i);
        }

        for (int i = 0; i < treeCount; i++)
        {
            // memilih posisi kosong secara random      
            var randomIndex = Random.Range(0, emptyPosition.Count);
            var pos = emptyPosition[randomIndex];
            
            // posisi yang terpilih hapus dari daftar posisi kosong
            emptyPosition.RemoveAt(randomIndex);

            SpawnRandomTree(pos);
        }

        // selalu ada pohon diujung kanan kiri
        SpawnRandomTree( -limit -1);
        SpawnRandomTree( limit +1);
    }

    private void SpawnRandomTree(int pos)
    {
         // pilih prefab pohon secara random
        var randomIndex = Random.Range(0, obstaclePrefabList.Count);
        var prefab = obstaclePrefabList[randomIndex];

        // set pohon ke posisi terpilih
        var tree = Instantiate(prefab, new Vector3(pos, 0, this.transform.position.z), Quaternion.identity, transform);
        
        tree.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
    
}
