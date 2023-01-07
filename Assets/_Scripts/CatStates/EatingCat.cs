// The cat will enter this state after it has gotten into the killing range of squirrel.
// Not much happens, cat gets full energy and goes back to walking.
public class EatingCat : ICatState
{
    private Cat _currentCat;
    public EatingCat(Cat currentCat) { this._currentCat = currentCat; }

    public bool IsWalkingCat() { return false; }

    public void Update()
    {
        _currentCat.UpdateEnergy(_currentCat.GetMaxEnergy());
        _currentCat.UpdateState(_currentCat.GetState("walkingCat"));
    }
}
