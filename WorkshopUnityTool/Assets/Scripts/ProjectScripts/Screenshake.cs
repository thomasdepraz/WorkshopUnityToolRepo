using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{

    [Tooltip("The thing to shake -- assumes it has a parent that controls the neutral position.")]
    public Transform _Target;

    [Tooltip("Set this so multiple objects shaking at the same time won't shake the same.")]
    public Vector2 _Seed;

    [Range(0.01f, 50f)]
    public float _Speed = 20f;

    [Tooltip("We won't move further than this distance from neutral.")]
    [Range(0.01f, 10f)]
    public float _MaxMagnitude = 0.3f;

    [Tooltip("0 follows _Direction exactly. 3 mostly ignores _Direction and shakes in all directions.")]
    [Range(0f, 3f)]
    public float _NoiseMagnitude = 0.3f;

    public Vector2 _Direction = Vector2.up;

    float _FadeOut = 1f;

    public float trauma;

    [Tooltip("Maximum trauma that this screenshake can reach. The higher the value, the more nuanced the screenshake can be.")]
    public float maxTrauma;

    [Range(0.1f, 3f)]
    [Tooltip("How fast will the current trauma will drop back to zero. e.g : if trauma = 1 and this = 2 then it will take 0.5 seconds.")]
    public float traumaDecreaseSpeed;
    
    public float rotationalSpread;
    public bool timeBased;
    public bool rotational;
    private float sinTime;


#if UNITY_EDITOR
    void Reset()
    {
        _Target = transform;
    }
    void OnValidate()
    {
        _Direction.Normalize();
    }
#endif // UNITY_EDITOR

    void Update()
    {
        if(trauma > 0)
        {
            trauma -= Time.deltaTime * traumaDecreaseSpeed;
            _FadeOut = trauma * trauma;
            float sin;

            if (timeBased)
              sin = Mathf.Sin(_Speed * (_Seed.x + _Seed.y + Time.time));
            else 
            {
                sinTime += Time.deltaTime;
                sin = Mathf.Sin(_Speed * (_Seed.x + _Seed.y + sinTime));
            }

            if(rotational)
            {
                _Target.localRotation = Quaternion.Euler(0,0, sin * rotationalSpread * _FadeOut) ;
            }

            var direction = _Direction + GetNoise();
            direction.Normalize();
            var delta = direction * sin;
            _Target.localPosition = delta * _MaxMagnitude * _FadeOut;
        }
        if(trauma > maxTrauma)
        {
            trauma = maxTrauma;
        }
        else if(trauma < 0)
        {
            trauma = 0;

            if (timeBased)
                sinTime = 0;
        }

        if (trauma == 0 && _Target.localPosition != Vector3.zero)
        {
            _Target.localPosition = Vector3.zero;
            if (timeBased)
                sinTime = 0;
        }
    }

    Vector2 GetNoise()
    {
        var time = Time.time;
        return _NoiseMagnitude
            * new Vector2(
                Mathf.PerlinNoise(_Seed.x, time) - 0.5f,
                Mathf.PerlinNoise(_Seed.y, time) - 0.5f
                );
    }
}
