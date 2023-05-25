using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    protected int horizontalSize;

    public virtual void Generate(int size)
    {
        size = 20;
        
        if (size == 0)
            return;

        if ((float) size % 2 == 0)
        {
            size -= 1;
        }
        
        int movLimit = Mathf.FloorToInt((float) size / 2);

        for (int i = -movLimit; i <= movLimit; i++)
        {
            SpawnTile(i);
        }
        
        for (int i = movLimit; i > -5; i--)
        {
            var leftBoundaryTile = SpawnTile(-movLimit - i);
            var rightBoundaryTile = SpawnTile(movLimit + i);
        
            DarkenObject(leftBoundaryTile);
            DarkenObject(rightBoundaryTile);
        }
    }

    private GameObject SpawnTile(int xPos)
    {
        var go = Instantiate(
            tilePrefab,
            transform);
           
            go.transform.localPosition = new Vector3(xPos, 0, 0);

            return go;
    }

    private void DarkenObject(GameObject go) 
    {
        var renderers = go.GetComponentsInChildren<MeshRenderer>();
        foreach (var rend in renderers)
        {
            rend.material.color *= Color.grey;
        }
    }
}
