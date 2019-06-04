using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all players regardless of platform need perspective shift, and the gameobject controls the transform
public class AgnosticPlayer : MonoBehaviour
{
    public GameObject steamPlayer;

    public GameObject oculusPlayer;

    public bool useOculus;

    private static GameObject player;

    void Awake()
    {
        if (AgnosticPlayer.player != null) {
            Debug.Log("There should not be more than one agnostic player gameobject");
        }
        AgnosticPlayer.player = useOculus ? oculusPlayer : steamPlayer;

        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public static GameObject GetPlayer() {
        return player;
    }
}
