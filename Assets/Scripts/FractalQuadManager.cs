using UnityEngine;
using System.Collections;

public class FractalQuadManager : MonoBehaviour {

    private FractalParameters fractalParameters;

	// Use this for initialization
	void Start () {
        fractalParameters = GameObject.FindGameObjectWithTag("FractalParameters").GetComponent<FractalParameters>();
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.SetMatrix("_Transformation", getTransform());
        renderer.material.SetInt("_Iterations", fractalParameters.iterations);

        var colorTexture = fractalParameters.fractalColors;

        renderer.material.SetTexture("_ColorTexture", colorTexture);
        renderer.material.SetColor("_InsideColor", colorTexture.GetPixel(colorTexture.width, 0));
	}

    private Matrix4x4 getTransform()
    {
        float aspectRatio = transform.localScale.x / transform.localScale.y;

        Vector3 position = new Vector3(-fractalParameters.center.x, -fractalParameters.center.y, 0.0f);
        Quaternion rotation = Quaternion.AngleAxis(fractalParameters.rotation, Vector3.forward);
        Vector3 scale = new Vector3(fractalParameters.scale * aspectRatio, fractalParameters.scale, 1.0f);

        Matrix4x4 mainMatrix = Matrix4x4.TRS(position, rotation, scale);
        Matrix4x4 offsetMatrix = Matrix4x4.TRS(new Vector3(-1.0f, -0.5f, 0.0f), Quaternion.identity, Vector3.one);

        return mainMatrix * offsetMatrix;
    }
}
