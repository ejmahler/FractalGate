using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GradientSpline {

    [SerializeField]
    private List<float> keys;

    [SerializeField]
    private List<Gradient> gradients;

    public Color Evaluate(float splineT, float gradientT)
    {
        keys.Sort();

        //find which pair of gradiens to use
        int gradientIndex = keys.BinarySearch(splineT);
        if (gradientIndex < 0)
        {
            gradientIndex = ~gradientIndex;
        }

        //if the splineT is out of range, clamp it to either end
        if (gradientIndex == keys.Count)
        {
            return gradients[keys.Count - 1].Evaluate(gradientT);
        }

        if (gradientIndex == 0)
        {
            return gradients[0].Evaluate(gradientT);
        }

        //we are between teo gradients, so interpolate between them
        Color beforeColor = gradients[gradientIndex - 1].Evaluate(gradientT);
        Color afterColor = gradients[gradientIndex].Evaluate(gradientT);

        float beforeT = keys[gradientIndex - 1];
        float afterT = keys[gradientIndex];

        return Color.Lerp(beforeColor, afterColor, (splineT - beforeT) / (afterT - beforeT));
    }
}
