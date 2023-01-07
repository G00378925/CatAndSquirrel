using UnityEngine;
using UnityEngine.UI;

// This class implements the ChaseController singleton.
// There is a GetInstance method to retrieve the instance of the singleton.
// When the class is not in the chaseInProgress state,
// it will iterate through all Cats and Squirrels in the game, if any of them are in the SQUIRREL_DETECTION_RANGE
// chaseInProgress is set to true and the chase begins.
// # # # # # # # # # # # # # # # # # # # # # # # # # # #
// The cat and the squirrel will each take turns moving.
// The cat will move torwads the squirrel, and the squirrel will move towards the nest.
// If the squirrel comes in the SQUIRREL_KILL_RANGE the cat will destroy the squirrel and the chase will end,
// state of the cat being updated accordingly.
// # # # # # # # # # # # # # # # # # # # # # # # # # # #
// If the squirrel gets to the nest before the cat kills it,
// the chase will end and the states of the cat and squirrel being updated accordingly.
// # # # # # # # # # # # # # # # # # # # # # # # # # # #
// Throughout the chase, the colours of the Cat and Squirrel will be updated to reflect their energy level.
// If the level goes bellow 30% the colour of cat will turn to red.
// If the level goes bellow 60% the colour of cat will turn to yellow.
// If the level is anything else it will turn to green.
// # # # # # # # # # # # # # # # # # # # # # # # # # # #
public class ChaseController : MonoBehaviour
{
    [SerializeField] private int SQUIRREL_DETECTION_RANGE = 10, SQUIRREL_KILL_RANGE = 6;

    private bool chaseInProgress = false, catsTurn = false;
    private GameObject catObjChasing, squirrelObjBeingChased;

    private static ChaseController instance;
    public static ChaseController GetInstance() { return instance; }
    void Awake() { instance = this; }

    bool IsSquirrelInRange(float range) 
    {
        return Vector3.Distance(catObjChasing.transform.position, squirrelObjBeingChased.transform.position) < range;
    }

    void UpdateGUIText(string newGUIText)
    {
        GameObject.Find("Canvas").transform.Find("GUIText").GetComponent<Text>().text = newGUIText;
    }

    void SetEnergyColor(MeshRenderer meshRenderer, double energyValue, double maxEnergyValue)
    {
        if (energyValue < (maxEnergyValue * 0.3))
            meshRenderer.material.SetColor("_Color", Color.red);
        else if (energyValue < (maxEnergyValue * 0.6))
            meshRenderer.material.SetColor("_Color", Color.yellow);
        else
            meshRenderer.material.SetColor("_Color", Color.green);
    }

    void Update()
    {
        if (!this.chaseInProgress)
        {
            GameObject[] catArray = GameObject.FindGameObjectsWithTag("Cat");
            GameObject[] squirrelArray = GameObject.FindGameObjectsWithTag("Squirrel");

            foreach (GameObject catGameObject in catArray)
            {
                foreach (GameObject squirrelGameObject in squirrelArray)
                {
                    this.catObjChasing = catGameObject;
                    this.squirrelObjBeingChased = squirrelGameObject;

                    if (IsSquirrelInRange(SQUIRREL_DETECTION_RANGE))
                    {
                        Cat cat = catObjChasing.GetComponent<Cat>();
                        Squirrel squirrel = squirrelObjBeingChased.GetComponent<Squirrel>();

                        if (cat.IsWalkingCat() && !squirrel.ReachedTheNest())
                        {
                            cat.UpdateState(cat.GetState("chasingCat"));
                            squirrel.UpdateState(squirrel.GetState("fleeingSquirrel"));

                            UpdateGUIText("Chase in progress");
                            this.chaseInProgress = true;
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            catsTurn = !catsTurn;

            Cat cat = catObjChasing.GetComponent<Cat>();
            Squirrel squirrel = squirrelObjBeingChased.GetComponent<Squirrel>();

            if (catsTurn) cat.TakeMoveTurn(squirrel.transform.position);
            else squirrel.TakeMoveTurn();

            SetEnergyColor(catObjChasing.GetComponent<MeshRenderer>(), cat.GetEnergy(), cat.GetMaxEnergy());
            SetEnergyColor(squirrelObjBeingChased.GetComponent<MeshRenderer>(), squirrel.GetEnergy(), squirrel.GetMaxEnergy());

            if (IsSquirrelInRange(SQUIRREL_KILL_RANGE))
            {
                UpdateGUIText("Squirrel gets eaten");
                this.chaseInProgress = false;
                cat.UpdateState(cat.GetState("eatingCat"));
                Destroy(squirrelObjBeingChased);
                return;
            }
            else if (squirrel.ReachedTheNest())
            {
                UpdateGUIText("Squirrel escapes");
                this.chaseInProgress = false;
                cat.UpdateState(cat.GetState("restingCat"));
                squirrel.UpdateState(squirrel.GetState("atNestSquirrel"));
                return;
            }
        }
    }
}