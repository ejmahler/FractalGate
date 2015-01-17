using UnityEngine;
using System.Collections;

public class FractalParameters : MonoBehaviour {

    public float completionPercent { get; set; }
    public Vector2 center { get; set; }

    [SerializeField]
    private float _scale = 1.0f;
    public float scale { get { return _scale; } }

    [SerializeField]
    private float _rotationDegrees = 0.0f;
    public float rotation { get { return _rotationDegrees; } }

    [SerializeField]
    private int _iterations = 50;
    public int iterations { get { return _iterations; } }

    [SerializeField]
    private Gradient _fractalColors = null;

    [SerializeField]
    private GradientSpline _pulseGradient;

    [SerializeField]
    private float _beatPulseDuration;
    public float beatPulseDuration { get { return _beatPulseDuration; } }

    private float pulseRatio = 0.0f;

	private Texture2D _fractalColorsTexture;
    public Texture2D fractalColors { get {
		if(_fractalColorsTexture == null) {
			int textureWidth = 1024;
			_fractalColorsTexture = new Texture2D(textureWidth, 1, TextureFormat.RGB24, false, true);
		}
		UpdateFractalColorTexture();
		return _fractalColorsTexture;
    } }

	public void Beat(float distanceFromBeat) {

		System.Action<float> updateFunction = (ratio) => {
            pulseRatio = ratio;
		};

        float percentRemaining = distanceFromBeat / _beatPulseDuration;

        LeanTween.value(transform.gameObject, updateFunction, 1 - percentRemaining, 1.0f, _beatPulseDuration * percentRemaining)
            .setOnComplete( () => {
                LeanTween.value(transform.gameObject, updateFunction, 1.0f, 0.0f, _beatPulseDuration * 2); 
            }
		);
	}

    private void UpdateFractalColorTexture()
    {
		for (int i = 0; i < _fractalColorsTexture.width; i++)
        {
			float t = ((float)i) / _fractalColorsTexture.width;

            //we have two different colors - one for pulsing and one for non pulsing. grab them both
            var pulseColor = _pulseGradient.Evaluate(completionPercent, t);
            var normalColor = _fractalColors.Evaluate(t);

            //blend between the current pulse color and the normal color based on the "pulse ratio"
            var actualColor = Color.Lerp(normalColor, pulseColor, pulseRatio);

            _fractalColorsTexture.SetPixel(i, 0, actualColor);
        }
		_fractalColorsTexture.wrapMode = TextureWrapMode.Clamp;
		_fractalColorsTexture.Apply();
    }
}
