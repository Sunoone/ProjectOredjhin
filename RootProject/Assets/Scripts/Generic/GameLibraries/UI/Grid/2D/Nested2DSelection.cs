using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nested2DSelection
{
    // We need an identifier to know which ones we can swap and which we can't.
    /*public NestedSelectable Master;

    public NestedSelectable NestedHost;
    public void SetNestHost(NestedSelectable selection) { NestedHost = selection; }
    public void ClearNestHost(NestedSelectable seleciton) { NestedHost = null; }


    public NestedSelectable CurrentSelection;
    //public List<NestedSelectable> NestedSelections;
    
    // Can deal with any nested selections itself. Anything outside should be solved by the grid.


    //public NestedSelectable NestHost;

    const int cDefaultAmount = 1;
    public int MaxHorizontal;
    public int MaxVertical = int.MaxValue;

    private bool active = false;
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
            if (!value)
                Deselect();
        }
    }
    private int HorizontalIndex = 1;
    private int VerticalIndex = 0;

    public int GetVerticalIndex() { return VerticalIndex; }

    public void Select(List<List<NestedSelectable>> controllerSelectables)
    {
        if (Active)
        {
            CurrentSelection.Deselect();
            CurrentSelection = controllerSelectables[VerticalIndex][HorizontalIndex];
            CurrentSelection.Select(Deselect);
        }
    }
    public void Deselect()
    {
        if (CurrentSelection == null)
            return;

        CurrentSelection.Deselect();
        CurrentSelection = default(NestedSelectable);
    }


    public bool MoveUp()
    {
        if (CurrentSelection == null) // There is no nested up available.
        {
            // What to do exacty here?
            return false;
        }

        //VerticalIndex = VerticalIndexInput(1);
        
        NestedSelectable newSelection = CurrentSelection.NestedUp(); // This is the new selectable
        newSelection.SetBackTarget(CurrentSelection, NestedSelectable.MoveDirection.Down);
        HorizontalIndex = 0;
        Select(newSelection);
        return true;
    }

    public void Select(NestedSelectable selectable)
    {
        Deselect(CurrentSelection);
        CurrentSelection = selectable;
        selectable.Select(Deselect);
    }

    public void Deselect(NestedSelectable selectable)
    {
        selectable.Deselect();
    }
































    private int HorizontalIndexInput(int steps) { return Mathf.Clamp(HorizontalIndex + steps, 0, MaxHorizontal); }
    private int VerticalIndexInput(int steps) { return Mathf.Clamp(VerticalIndex + steps, 0, MaxVertical); }*/
}


