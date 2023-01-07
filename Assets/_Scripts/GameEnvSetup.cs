using UnityEngine;

// The purpose of this class is to setup the scene with trees and squirrels.
// The amount of tree and squirrels are adjustable in the inspector.
// The trees spawned are passed to the squirrels so they know where they are.
// This class will self destruct after it purpose is fulfilled.
public class GameEnvSetup : MonoBehaviour
{
    [SerializeField] private GameObject _treePrefab, _squirrelPrefab;
    [SerializeField] private int _treeCount = 10, _squirrelCount = 4; 

    void Start()
    {
        float xBegin = GameObject.Find("Wall2").transform.position.x;
        float xEnd = GameObject.Find("Wall4").transform.position.x;
        float zBegin = GameObject.Find("Wall3").transform.position.z;
        float zEnd = GameObject.Find("Wall1").transform.position.z;

        var nestPosition = GameObject.Find("SquirrelNest").transform.position;
        GameObject[] treeArray = new GameObject[_treeCount];

        for (int i = 0; i < this._treeCount;) {
            float treeX = Random.Range(xBegin, xEnd), treeZ = Random.Range(zBegin, zEnd);
            var treePosition = new Vector3(treeX, 0, treeZ);

            if (Vector3.Distance(treePosition, nestPosition) < 8)
                continue;

            treeArray[i] = Instantiate(_treePrefab, treePosition, Quaternion.identity);
            treeArray[i].name = "Tree" + i;
            i++;
        }

        for (int i = 0; i < this._squirrelCount; i++) {
            float squirrelX = Random.Range(xBegin, xEnd), squirrelZ = Random.Range(zBegin, zEnd);

            var squirrelObj = Instantiate(_squirrelPrefab, new Vector3(squirrelX, 0, squirrelZ), Quaternion.identity);
            squirrelObj.GetComponent<Squirrel>().SetTrees(treeArray);
            squirrelObj.name = "Squirrel" + i;
            squirrelObj.tag = "Squirrel";
        }

        Destroy(gameObject);
    }
}
