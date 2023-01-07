using UnityEngine;

// When Squirrel goes into this state, they will head back to their nest.
// When they are at the nest, they can begin eating nuts stored in the nest.
// If the squirrel has nuts, they can put them in the nest if isn't locked.
// When the squirrel has enought ate, they can go back to searching for trees.
public class AtNestSquirrel : ISquirrelState
{
    private Squirrel _currentSquirrel;
    public AtNestSquirrel(Squirrel currentSquirrel) { this._currentSquirrel = currentSquirrel; }

    public void Update()
    {
        if (_currentSquirrel.GetEnergy() > _currentSquirrel.GetMaxEnergy())
        {
            _currentSquirrel.UpdateState(_currentSquirrel.GetState("searchingSquirrel"));
            return;
        }

        Vector3 nestPosition = _currentSquirrel.GetNest().transform.position;
        Vector3 currentSquirrelPosition = _currentSquirrel.transform.position;

        if (Vector3.Distance(nestPosition, currentSquirrelPosition) < 3) {
            float amountOfNutsNeeded = 0.2f * Time.deltaTime;
            _currentSquirrel.UpdateEnergy(_currentSquirrel.GetEnergy() + amountOfNutsNeeded);

            if (_currentSquirrel.GetNutCount() > 0 && !_currentSquirrel.GetNest().IsNestLocked())
                _currentSquirrel.UpdateState(_currentSquirrel.GetState("storingSquirrel"));
            else if (_currentSquirrel.GetEnergy() < _currentSquirrel.GetMaxEnergy()) {
                if (_currentSquirrel.GetNest().GetNutsCollected() >= amountOfNutsNeeded)
                    _currentSquirrel.GetNest().EatNuts(amountOfNutsNeeded, _currentSquirrel);
            }
        } else {
            Vector3 nestDirection = nestPosition - currentSquirrelPosition;
            nestDirection.Normalize();
            _currentSquirrel.transform.position += nestDirection * _currentSquirrel.GetSpeed() * Time.deltaTime;
        }
    }
}