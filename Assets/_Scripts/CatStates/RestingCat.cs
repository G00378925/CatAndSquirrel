using UnityEngine;

// As per the spec, the cat will restore 0.3 of unity when resting.
// If the cat isn't at their bed they need to get to it.
// When the cat is full rested they can go back to walking.
public class RestingCat : ICatState
{
    private Cat _currentCat;
    public RestingCat(Cat currentCat) { this._currentCat = currentCat; }

    public bool IsWalkingCat() { return false; }

    public void Update()
    {
        if (_currentCat.GetEnergy() >= _currentCat.GetMaxEnergy())
        {
            _currentCat.UpdateState(_currentCat.GetState("walkingCat"));
            return;
        }

        Vector3 currentCatPosition = _currentCat.transform.position;
        Vector3 catBedPosition = _currentCat.GetBed().transform.position;

        if (Vector3.Distance(currentCatPosition, catBedPosition) < 1)
            _currentCat.UpdateEnergy(_currentCat.GetEnergy() + (0.3f * Time.deltaTime));
        else
            _currentCat.transform.position = Vector3.MoveTowards(currentCatPosition, catBedPosition, _currentCat.GetSpeed() * Time.deltaTime);
    }
}
