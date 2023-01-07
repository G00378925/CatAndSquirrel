using System;
using UnityEngine;

// This state will check if the squirrel has enough space for the nuts to be collected,
// if there isn't go back to the nest to deposit them.
// If there is space, extract the nuts from the tree, and the squirrel will hold them.
public class GatheringSquirrel : ISquirrelState
{
    private Squirrel _currentSquirrel;
    public GatheringSquirrel(Squirrel currentSquirrel) { this._currentSquirrel = currentSquirrel; }

    public void Update()
    {
        Tree tree = _currentSquirrel.GetTargetTree().GetComponent<Tree>();

        if (tree.GetNutCount() <= 0 || _currentSquirrel.GetNutCount() >= _currentSquirrel.GetMaxNutCount())
            _currentSquirrel.UpdateState(_currentSquirrel.GetState("atNestSquirrel"));
        else {
            float nutCount = _currentSquirrel.GetMaxNutCount() < tree.GetNutCount() ? _currentSquirrel.GetMaxNutCount() : tree.GetNutCount();
            tree.TakeNuts(nutCount, _currentSquirrel);
        }
    }
}