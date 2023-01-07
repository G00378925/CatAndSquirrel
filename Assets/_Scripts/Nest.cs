using UnityEngine;

// This class implements the functionality required of the nest.
// Squirrels will aquire a lock to the nest and then deposit their nuts.
// Other squirrels must find something else to do when the lock is in place
public class Nest : MonoBehaviour
{
    [SerializeField] private float _nutsCollected = 20;
    [SerializeField] private bool _nestLock = false;

    // // // // // Nuts Related // // // // //
    public void DepositNuts(float nutCount, Squirrel squirrel) {
        squirrel.UpdateNutCount(squirrel.GetNutCount() - nutCount);
        this._nutsCollected += nutCount;
    }

    public float GetNutsCollected() { return this._nutsCollected; }

    public void EatNuts(float nutCount, Squirrel squirrel) {
        squirrel.UpdateEnergy(squirrel.GetEnergy() + nutCount);
        this._nutsCollected -= nutCount;
    } 

    // // // // // Nest Locking // // // // //
    public bool IsNestLocked() { return this._nestLock; }
    public void LockNest() { this._nestLock = true; }
    public void UnlockNest() { this._nestLock = false; }
}