using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrOverlay : MonoBehaviour
{
    public Camera trackedCamera;

    private float offset = 0.1f;
    

    // Start is called before the first frame update
    void Start()
    {
        var frustrumHeight = 2.0f * offset * Mathf.Tan(trackedCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        var frustrumWidth = frustrumHeight * trackedCamera.aspect;
        RectTransform t = GetComponent<RectTransform>();
        
        // add extra padding just in case
        t.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, frustrumWidth + 0.1f);
        t.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, frustrumHeight + 0.1f);

        GameObject overlay = transform.Find("Overlay").gameObject;
        overlay.transform.localScale = new Vector3(frustrumWidth + 0.1f, frustrumHeight + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (trackedCamera.transform.forward * offset) + trackedCamera.transform.position;
        transform.rotation = trackedCamera.transform.rotation;
    }
}
