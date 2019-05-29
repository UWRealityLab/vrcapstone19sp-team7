using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MasterLoop))]
public class MelodyPuzzle : MonoBehaviour
{

    public GameObject melodyPrefab;

    public PuzzleMonitor monitor;

    public Flock flock;

    public GameObject boidEmitterPrefab;

    public MasterLoop masterLoop;

    private List<PathBeat> allPaths;


    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        allPaths = new List<PathBeat>();

        var path = CreateAndSetupPath("p1", MasterLoop.loopTime + 5f, 0);
        // path.gameObject.SetActive(true);
        var path1 = CreateAndSetupPath("p2", MasterLoop.loopTime, 1);
        path1.gameObject.SetActive(false);

        var path2 = CreateAndSetupPath("p3", MasterLoop.loopTime, 2);
        path2.gameObject.SetActive(false);

        var path3 = CreateAndSetupPath("p4", MasterLoop.loopTime, 2.5f);
        path3.gameObject.SetActive(false);


        foreach (var p in allPaths)
        {
            var go = p.obj.gameObject;
            MelodyObject mo = go.GetComponent<MelodyObject>();
            masterLoop.onNewLoop.AddListener(delegate ()
            {
                mo.NewLoop();
            });
        }

        path.onSuccessful.AddListener(delegate ()
        {
            path1.gameObject.SetActive(true);
            Debug.Log("success");
        });

        path1.onSuccessful.AddListener(delegate ()
        {
            path2.gameObject.SetActive(true);
        });

        path2.onSuccessful.AddListener(delegate ()
        {
            path3.gameObject.SetActive(true);
        });

        monitor.onPuzzleCompleted.AddListener(delegate ()
        {
            Debug.Log("All puzzles were completed");
        });
    }

    PathBeat InstantiatePath(string fileName, float t = 3, float beatOffset = 0)
    {
        GameObject go = Instantiate(melodyPrefab, transform);

        go.transform.SetParent(transform);
        PathBeat pathBeat = go.GetComponent<PathBeat>();
        pathBeat.LoadFromFile(fileName);

        monitor.Register(pathBeat.onSuccessful);

        pathBeat.SetCompletionTime(t);
        GameObject melodyObject = pathBeat.transform.Find("MelodyObject").gameObject;
        melodyObject.GetComponent<MelodyObject>().beatOffset = beatOffset;
        return pathBeat;
    }

    // Creates and path and puts it into all paths list
    PathBeat CreateAndSetupPath(string fileName, float t = 3, float beatOffset = 0)
    {
        PathBeat pathBeat = InstantiatePath(fileName, t, beatOffset);

        // BoidEmitter emitter = AddBoidEmitter(pathBeat);

        pathBeat.onReachedEnd.AddListener(delegate ()
        {
            Debug.Log("end was reached");
        });

        pathBeat.onSuccessful.AddListener(delegate ()
        {
            Debug.Log("successfully completed");
            // emitter.EmitBoids(6);
        });

        pathBeat.onFailed.AddListener(delegate ()
        {
            Debug.Log("failed");
        });

        allPaths.Add(pathBeat);
        return pathBeat;
    }

    BoidEmitter AddBoidEmitter(PathBeat path)
    {
        GameObject go = Instantiate(boidEmitterPrefab);
        go.transform.parent = path.obj.transform;

        BoidEmitter emitter = go.GetComponent<BoidEmitter>();
        emitter.flock = flock;

        go.transform.position = path.obj.transform.position;
        return emitter;
    }


}
