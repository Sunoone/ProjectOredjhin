using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T> where T : class
{
    public List<List<T>> GetGrid() { return grid; }
    public void Create(int horizontalMax, int verticalMax)
    {
        int width = horizontalMax;
        for (int x = 0; x < width; x++)
        {
            grid.Add(new List<T>());
            int height = verticalMax;
            for (int y = 0; y < height; y++)
            {
                grid[x].Add(default(T));
            }
        }
    }
    public void Create(List<List<T>> gridList) { grid = gridList; }

    List<List<T>> grid = new List<List<T>>();
    public void EmptyHorizontal(int vertical, int horizontal) { grid[vertical][horizontal] = null; }
    public void AddToHorizontal(int verticalIndex, T selectable, bool autoFill = false)
    {
        if (autoFill)
            FillVertical(verticalIndex);
        grid[verticalIndex].Add(selectable);
    }
    public void SetHorizontal(int verticalIndex, List<T> list, bool autoFill = false)
    {
        if (autoFill)
            FillVertical(verticalIndex);
        grid[verticalIndex] = list;
    }
    public void RemoveFromHorizontal(int verticalIndex, T selectable) { grid[verticalIndex].Remove(selectable); }

    public void FillVertical(int verticalIndex)
    {
        while (verticalIndex < grid.Count)
            AddVertical();
    }
    private void AddVertical() { grid.Add(new List<T>()); }
    
    private void RemoveVertical(int verticalIndex) { grid.RemoveAt(verticalIndex); }
    public void EmptyVertical(int verticalIndex) { grid[verticalIndex] = null; }

    public T GetGridObject(int horizontalIndex, int verticalIndex) { return grid[verticalIndex][horizontalIndex]; }

}