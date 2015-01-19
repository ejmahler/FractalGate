using UnityEngine;
using System.Collections.Generic;

//Gradient Spline is exactly what it sounds like, a spline of Gradients. In this case, a linear spline.
//in the unity editor, add keys and gradients - the key is a T, usually ranging from 0 to 1
//when Evaluate is called, if a splineT is given that falls between two keys,
//it interpolates between the two closest corresponding gradients

//the goal is to allow a smooth transition from one gradient to the next, in cases where a single gradient isn't flexible enough

[System.Serializable]
public class GradientSpline {

    [SerializeField]
    private List<float> keys;

    [SerializeField]
    private List<Gradient> gradients;

    public Color Evaluate(float splineT, float gradientT)
    {
        keys.Sort();

        //find which pair of gradients to use
        int gradientIndex = keys.BinarySearch(splineT);
        if (gradientIndex < 0)
        {
            gradientIndex = ~gradientIndex;
        }

        //if the splineT is below the smallest key or above the largest, clamp it to either end
        if (gradientIndex == keys.Count)
        {
            return gradients[keys.Count - 1].Evaluate(gradientT);
        }
        else if (gradientIndex == 0)
        {
            return gradients[0].Evaluate(gradientT);
        }

        //we are between two gradients, so interpolate between them
        Color beforeColor = gradients[gradientIndex - 1].Evaluate(gradientT);
        Color afterColor = gradients[gradientIndex].Evaluate(gradientT);

        float beforeT = keys[gradientIndex - 1];
        float afterT = keys[gradientIndex];

        float colorRatio = (splineT - beforeT) / (afterT - beforeT);

        return Color.Lerp(beforeColor, afterColor, colorRatio);
    }
}
