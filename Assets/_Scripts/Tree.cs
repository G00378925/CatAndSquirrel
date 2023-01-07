using UnityEngine;

// Squirrels will access this class when they find their tree.
// They can check the amount of nuts in the tree using GetNutCount.
// And determine the amount of nuts they will take.
public class Tree : MonoBehaviour
{
    [SerializeField] private float nutsBeingStored;

    void Awake() { this.nutsBeingStored = Random.Range(1, 5); }

    public float GetNutCount() { return this.nutsBeingStored; }

    public void TakeNuts(float nutsToBeTaken, Squirrel squirrel)
    {
        this.nutsBeingStored -= nutsToBeTaken;
        squirrel.UpdateNutCount(squirrel.GetNutCount() + nutsToBeTaken);
    }
}
