using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(FractalParameters))]
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 _positionStart;
    public Vector2 positionStart { get { return _positionStart; } }

    [SerializeField]
    private Vector2 _positionEnd;
    public Vector2 positionEnd { get { return _positionEnd; } }

    [SerializeField]
    private float _travelTime;
    public float travelTime { get { return _travelTime; } }

    private FractalParameters fractalParameters;
    private List<float> beatTimes = new List<float>();

    // Use this for initialization
    void Start()
    {
        Screen.showCursor = false;

        fractalParameters = GetComponent<FractalParameters>();

        StartMusic();
        StartMovement();
    }

    private void StartMusic()
    {
        //figure out all the times that a beat occurs, so that we know if the player is pressing space at the correct time

        //these are magic numbers, unfortunately. they were determined by examining the game's song in audacity
        float beatStart = 29.354f + 0.15f;
        float beatEnd = 213.0f;
        float beatInterval = 1.77777f;

        for (float t = beatStart; t < beatEnd; t += beatInterval)
        {
            beatTimes.Add(Time.time + t);
        }
        beatTimes.Sort();

        GameObject.FindGameObjectWithTag("MainCamera").audio.Play();
    }

    private void StartMovement()
    {
        //each time this is called, place the fractal parameters' center between the start and end - how far in between is detemined by completionPercent
        System.Action<float> updateFunction = (completionPercent) =>
        {
            var displacement = _positionEnd - _positionStart;
            var traveled = completionPercent * displacement;

            fractalParameters.completionPercent = completionPercent;
            fractalParameters.center = _positionStart + traveled;
        };

        //tween between 0% completion and 100% completion, effectively moving the fractalparameter center from the start to the end
        LeanTween.value(transform.gameObject, updateFunction, 0.0f, 1.0f, _travelTime);
    }

    // Update is called once per frame
    void Update()
    {
        //if the user presses space, see if we're within like 0.1 second of a beat. if so, make the fractal pulse red
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //find the time of the next upcoming beat
            int beatIndex = beatTimes.BinarySearch(Time.time);
            if (beatIndex < 0)
            {
                beatIndex = ~beatIndex;
            }

            if (beatIndex < beatTimes.Count)
            {

                //find how long before the next beat occurs
                float distance = beatTimes[beatIndex] - Time.time;

                if (distance < fractalParameters.beatPulseDuration)
                {
                    fractalParameters.Beat(distance);
                }
            }
        }

        if (Time.timeSinceLevelLoad > 234 || Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}