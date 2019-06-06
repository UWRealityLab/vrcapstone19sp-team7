using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    public Shader shader;

    public AudioSource source;
    private int backgroundSampleLength = 64;
    private int foregroundSampleLength = 512;

    private Texture2D backgroundTexture;
    private Texture2D foregroundTexture;

    private float[] backgroundSamples;
    private float[] foregroundSamples;

    // Start is called before the first frame update
    void Start()
    {
        // float[] arr = source.GetOutputData(128, 0);
        // float[] spectrum = source.GetOutputData(128, 0);
        Material mat = new Material(shader);
        Renderer renderer = GetComponent<Renderer>();

        AudioClip clip = source.clip;
        // float[] samples = new float[clip.samples * clip.channels];
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);
        Debug.Log("samples: " + clip.samples);

        backgroundSamples = new float[backgroundSampleLength];
        foregroundSamples = new float[foregroundSampleLength];


        backgroundTexture = new Texture2D(backgroundSampleLength, 1, TextureFormat.RGBA32, false);
        foregroundTexture = new Texture2D(foregroundSampleLength, 1, TextureFormat.RGBA32, false);
        
        // mat.SetTexture("_AudioTex", foregroundTexture);
        mat.SetTexture("_AudioTex", foregroundTexture);
        if (renderer != null)
        {
            renderer.material = mat;
        }

        source.Play();
    }

    void Update()
    {
        AudioListener.GetOutputData(backgroundSamples, 0);
        for (int i = 0; i < backgroundTexture.width; i++)
        {
            float db = (foregroundSamples[i] + 1f) / 2f;
            Color c = new Color(db, db, db);
            // does y position matter
            backgroundTexture.SetPixel(i, 0, c);
        }
        backgroundTexture.Apply();

        source.GetOutputData(foregroundSamples, 0);
        for (var i = 0; i < foregroundTexture.width; i++)
        {
            // range -1 to 1
            float db = (foregroundSamples[i] + 1f) / 2f;
            Color c = new Color(db, db, db);
            foregroundTexture.SetPixel(i, 1, c);
        }

        foregroundTexture.Apply();
    }

}
