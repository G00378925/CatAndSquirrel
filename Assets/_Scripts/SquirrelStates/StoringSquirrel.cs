using UnityEngine;

// When squirrel is depositing the nuts, they will lock the nest, preventing anyone else depositing nuts.
// If squirrel has more than zero nuts, they will deposit them.
// Less than zero nuts, they can go back to the atNest state.
public class StoringSquirrel : ISquirrelState
{
    private Squirrel _currentSquirrel;
    public StoringSquirrel(Squirrel currentSquirrel) { this._currentSquirrel = currentSquirrel; }

    public void Update()
    {
        _currentSquirrel.GetNest().LockNest();

        if (_currentSquirrel.GetNutCount() > 0)
            _currentSquirrel.GetNest().DepositNuts(0.2f * Time.deltaTime, _currentSquirrel);

        if (_currentSquirrel.GetNutCount() <= 0)
            _currentSquirrel.UpdateState(_currentSquirrel.GetState("atNestSquirrel"));

        _currentSquirrel.GetNest().UnlockNest();
    }
}