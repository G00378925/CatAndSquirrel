using UnityEngine;

// I implemeneted the state machine pattern using different classes for each state.
// https://www.c-sharpcorner.com/article/understanding-state-design-pattern-by-implementing-finite-state/
// Each state must fullill the requirements of the ISquirrelState interface.
// Each state implements the Update method, which is called per a frame.
// The state machine for the Squirrel is not implemented much differently from what is being done here.
// # # # # # # # # # # # # # # # # # #
// When the cat is spawned its bed will also be spawned under it.
// As the states change I have it so the cat will iterate through black and blue colours to indicate this.
// The Squirrel also does this.
public class Cat : MonoBehaviour
{
    private ICatState _currentCatState;

    // These are the states of the Cat
    private ChasingCat _chasingCat;
    private EatingCat _eatingCat;
    private RestingCat _restingCat;
    private WalkingCat _walkingCat;

    [SerializeField] public GameObject[] definedPath;
    [SerializeField] private GameObject _bedPrefab;
    [SerializeField] private float _energyValue = 35;
    [SerializeField] private float _speed = 5f;

    private float _maxEnergyValue = 0;
    private GameObject _bed;
    private bool _isBlue = false;

    public int currentPathIndex { get; set; }

    public float GetSpeed() { return this._speed; }
    public GameObject GetBed() { return this._bed; }
    public float GetEnergy() { return this._energyValue; }
    public float GetMaxEnergy() { return this._maxEnergyValue; }

    public void TakeMoveTurn(Vector3 squirrelPosition)
    {
        Vector3 squirrelDirection = squirrelPosition - transform.position;
        squirrelDirection.Normalize();

        transform.position += squirrelDirection * 4 * Time.deltaTime;
    }

    public void UpdateEnergy(float newEnergyValue)
    {
        this._energyValue = newEnergyValue;

        if (this.GetEnergy() < (this.GetMaxEnergy() * 0.3))
            this.UpdateState(this.GetState("restingCat"));
    }

    // // // // // GAME OBJECT LIFECYCLE METHODS // // // // //
    void Start()
    {
        this._restingCat = new RestingCat(this);
        this._walkingCat = new WalkingCat(this);
        this._chasingCat = new ChasingCat(this);
        this._eatingCat = new EatingCat(this);

        this.UpdateState(this.GetState("restingCat"));
        this._maxEnergyValue = this._energyValue;

        this._bed = Instantiate(_bedPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
        this._bed.name = "CatBed";
    }

    void Update() { this._currentCatState.Update(); }

    // // // // // STATE RELATED METHODS // // // // //
    public bool IsWalkingCat() { return this._currentCatState.IsWalkingCat(); }

    public ICatState GetState(string catStateName)
    {
        switch (catStateName)
        {   
            case "chasingCat": return _chasingCat;
            case "eatingCat": return _eatingCat;
            case "restingCat": return _restingCat;
            case "walkingCat": return _walkingCat;
            default: return null;
        }
    }

    public void UpdateState(ICatState newState)
    {
        this._isBlue = !this._isBlue;
        this.GetComponent<Renderer>().material.color = this._isBlue ? Color.blue : Color.black;

        _currentCatState = newState;
    }
}
