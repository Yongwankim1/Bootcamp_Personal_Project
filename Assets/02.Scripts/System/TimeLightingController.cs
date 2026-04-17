using System;
using UnityEngine;

public class TimeLightingController : MonoBehaviour
{
    public static TimeLightingController Instance;
    [SerializeField] private float playDay;
    [SerializeField] private float timeLightingValue;
    public float PlayDay => playDay;
    public float TimeLightingValue => timeLightingValue;

    [SerializeField] float cycleDuration;

    public float CycleDuration => cycleDuration;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        playDay = Time.time / cycleDuration;
        timeLightingValue = Time.time % cycleDuration;
    }
}
