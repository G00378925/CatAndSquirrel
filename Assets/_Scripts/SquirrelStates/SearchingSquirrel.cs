using UnityEngine;

// Searching wastes 0.1 units per an update.
// Squirrel will move marker to marker scouting for trees.
// If a tree is in range, that will be set as the target, and the squirrel will go to the next state.
// Gather preparing for extraction of nuts from the tree.
public class SearchingSquirrel : ISquirrelState
{
    private int TREE_RANGE_THRESHOLD = 5;

    private Squirrel _currentSquirrel;
    public SearchingSquirrel(Squirrel currentSquirrel) { this._currentSquirrel = currentSquirrel; }

    public void Update()
    {
        _currentSquirrel.UpdateEnergy(_currentSquirrel.GetEnergy() - (0.1f * Time.deltaTime));

        Vector3 currentPositionMarker = _currentSquirrel.definedPath[_currentSquirrel.currentPathIndex].transform.position;
        Vector3 currentSquirrelPosition = _currentSquirrel.transform.position;

        if (Vector3.Distance(currentSquirrelPosition, currentPositionMarker) < 5) {
            _currentSquirrel.currentPathIndex++;
            _currentSquirrel.currentPathIndex %= _currentSquirrel.definedPath.Length;
        }

        Vector3 markerDirection = currentPositionMarker - currentSquirrelPosition;
        markerDirection.Normalize();
        _currentSquirrel.transform.position += markerDirection * _currentSquirrel.GetSpeed() * Time.deltaTime;

        foreach (GameObject tree in _currentSquirrel.GetTrees()) {
            if (Vector3.Distance(tree.transform.position, currentSquirrelPosition) < TREE_RANGE_THRESHOLD) {
                _currentSquirrel.SetTargetTree(tree);
                _currentSquirrel.UpdateState(_currentSquirrel.GetState("gatheringSquirrel"));
                return;
            }
        }
    }
}