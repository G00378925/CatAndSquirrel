using UnityEngine;

// As defined in the spec, if the squirrels energy goes below a certain threshold (SQUIRREL_NEEDS_REST_THRESHOLD),
// they must rest, as seen in the UpdateEnergy method.
// All the squirrels share the same nest.
// State changes will be shown by going between black and blue.
public class Squirrel : MonoBehaviour
{
    private ISquirrelState _currentSquirrelState;
    [SerializeField] private string _currentStateName;

    private AtNestSquirrel _atNestSquirrel;
    private FleeingSquirrel _fleeingSquirrel;
    private GatheringSquirrel _gatheringSquirrel;
    private SearchingSquirrel _searchingSquirrel;
    private StoringSquirrel _storingSquirrel;

    [SerializeField] private float SQUIRREL_NEEDS_REST_THRESHOLD = 0.3f;
    [SerializeField] private float _maxNutCount = 5, _nutCount = 0, _energyValue = 10, _speed = 5f;

    private bool _isBlue = false;
    private GameObject _nest, _tree;
    private GameObject[] _trees;
    private float _maxEnergyValue = 0;

    public GameObject[] definedPath { get; set; }
    public int currentPathIndex { get; set; }

    public float GetSpeed() { return this._speed; }
    public float GetMaxNutCount() { return this._maxNutCount; }
    public float GetNutCount() { return this._nutCount; }
    public void UpdateNutCount(float newNutCount) { this._nutCount = newNutCount; }

    public Nest GetNest() { return this._nest.GetComponent<Nest>(); }

    public GameObject GetTargetTree() { return this._tree; }
    public GameObject[] GetTrees() { return this._trees; }

    public void SetTargetTree(GameObject tree) { this._tree = tree; }
    public void SetTrees(GameObject[] trees) { this._trees = trees; }

    public float GetEnergy() { return this._energyValue; }
    public float GetMaxEnergy() { return this._maxEnergyValue; }

    public void UpdateEnergy(float newEnergyValue)
    {
        this._energyValue = newEnergyValue;

        if (this.GetEnergy() < (this.GetMaxEnergy() * SQUIRREL_NEEDS_REST_THRESHOLD))
            this.UpdateState(this.GetState("atNestSquirrel"));
    }

    public bool ReachedTheNest()
    {
        return Vector3.Distance(transform.position, _nest.transform.position) < 3;
    }

    public void TakeMoveTurn()
    {
        Vector3 nestDirection = _nest.transform.position - transform.position;
        nestDirection.Normalize();

        transform.position += nestDirection * (this.GetSpeed() * 1.25f) * Time.deltaTime;
    }

    // // // // // GAME OBJECT LIFECYCLE METHODS // // // // //
    void Start()
    {
        this._atNestSquirrel = new AtNestSquirrel(this);
        this._fleeingSquirrel = new FleeingSquirrel(this);
        this._gatheringSquirrel = new GatheringSquirrel(this);
        this._searchingSquirrel = new SearchingSquirrel(this);
        this._storingSquirrel = new StoringSquirrel(this);

        this.UpdateState(this.GetState("atNestSquirrel"));
        this._maxEnergyValue = this._energyValue;

        definedPath = new GameObject[4];
        for (int pathIndex = 0; pathIndex < definedPath.Length; pathIndex++) {
            definedPath[pathIndex] = GameObject.Find("SquirrelPath" + pathIndex.ToString());
        }

        for (int i = 0; i < definedPath.Length; i++) {
            GameObject tempGO = definedPath[i];

            int randomIndex = Random.Range(i, definedPath.Length);

            definedPath[i] = definedPath[randomIndex];
            definedPath[randomIndex] = tempGO;
        }

        this._nest = GameObject.Find("SquirrelNest");
    }

    void Update() { _currentSquirrelState.Update(); }

    // // // // // STATE RELATED METHODS // // // // //
    public ISquirrelState GetState(string squirrelStateName)
    {
        switch (squirrelStateName)
        {
            case "atNestSquirrel": return _atNestSquirrel;
            case "fleeingSquirrel": return _fleeingSquirrel;
            case "gatheringSquirrel": return _gatheringSquirrel;
            case "searchingSquirrel": return _searchingSquirrel;
            case "storingSquirrel": return _storingSquirrel;
            default: return null;
        }
    }

    public void UpdateState(ISquirrelState newState) {
        this._isBlue = !this._isBlue;
        this.GetComponent<Renderer>().material.color = this._isBlue ? Color.blue : Color.black;

        _currentSquirrelState = newState;
    }
}
