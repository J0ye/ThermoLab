﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionList
{
    private List<Vector3> positions = new List<Vector3>();
    private int margin;
    private Vector2 xRange;
    private Vector2 zRange;

    private void FillList()
    {
        for(float i = xRange.x; i <= xRange.y; i += margin)
        {
            for(float j = zRange.x; j <= zRange.y; j +=margin)
            {
                positions.Add(new Vector3(i, 0, j));
                //Debug.Log("Position (" + i + ",0," + j +") added.");
            }
        }
    }

    public PositionList(int newMargin, Vector2 newXRange, Vector2 newZRange)
    {
        margin = newMargin;
        xRange = newXRange;
        zRange = newZRange;

        FillList();
    }

    public Vector3 GetRandomPosition()
    {
        if(positions.Count <= 0)
        {
            FillList();
        }
        int rand = UnityEngine.Random.Range(0, positions.Count -1);
        Vector3 pos = positions[rand];
        positions.RemoveAt(rand);

        return pos;        
    }

    public int GetPositionCount()
    {
        return positions.Count;
    }
}
