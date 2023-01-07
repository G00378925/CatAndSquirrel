using UnityEngine;

// Cat will waste 0.8 units per an update.
// The rest is implemented by the ChaseController.
public class ChasingCat : ICatState
{
    private Cat _currentCat;
    public ChasingCat(Cat currentCat) { this._currentCat = currentCat; }

    public bool IsWalkingCat() { return false; }

    public void Update()
    {
        _currentCat.UpdateEnergy(_currentCat.GetEnergy() - (0.8f * Time.deltaTime));
    }
}
