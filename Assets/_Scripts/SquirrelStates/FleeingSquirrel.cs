using UnityEngine;

// The fleeing is implemented by the ChaseController, as per the spec 0.3 unit wastes per update.
public class FleeingSquirrel : ISquirrelState
{
    private Squirrel _currentSquirrel;
    public FleeingSquirrel(Squirrel currentSquirrel) { this._currentSquirrel = currentSquirrel; }

    public void Update()
    {
        _currentSquirrel.UpdateEnergy(_currentSquirrel.GetEnergy() - (0.3f * Time.deltaTime));
    }
}