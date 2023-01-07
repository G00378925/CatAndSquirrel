using UnityEngine;

// As per the spec, the cat will waste 0.4 units of energy per update when walking.
// Cat will move from marker to marker, scouting for squirrels.
// ChaseController will implement the Squirrel detection, that isn't implemented here.
public class WalkingCat : ICatState
{
    private Cat _currentCat;
    public WalkingCat(Cat currentCat) { this._currentCat = currentCat; }

    public bool IsWalkingCat() { return true; }

    public void Update()
    {
        _currentCat.UpdateEnergy(_currentCat.GetEnergy() - (0.4f * Time.deltaTime));

        Vector3 currentPositionMarker = _currentCat.definedPath[_currentCat.currentPathIndex].transform.position;
        Vector3 currentCatPosition = _currentCat.transform.position;

        if (Vector3.Distance(currentCatPosition, currentPositionMarker) < 1) {
            _currentCat.currentPathIndex++;
            _currentCat.currentPathIndex %= _currentCat.definedPath.Length;
        }

        Vector3 markerDirection = currentPositionMarker - currentCatPosition;
        markerDirection.Normalize();
        _currentCat.transform.position += markerDirection * _currentCat.GetSpeed() * Time.deltaTime;
    }
}
