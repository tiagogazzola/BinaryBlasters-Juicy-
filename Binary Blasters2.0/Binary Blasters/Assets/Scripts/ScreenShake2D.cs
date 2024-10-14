using UnityEngine;

//https://www.youtube.com/watch?v=BPiur9EzI6Q
//https://github.com/ProgramandoGames/ScreenShake/blob/master/Assets/ScreenShake2D.cs

public class ScreenShake2D : MonoBehaviour
{
    float rotationZ = 0;

    public float maxAngleZ = 10;

    float offsetX = 5;
    float offsetY = 5;

    public float maxOffsetX = 10;
    public float maxOffsetY = 10;

    Vector3 originalPosition;

    public float intensity = 0;

    public float growthIntensity = 0.5f;
    public float decayIntensity = 0.5f;

    float seedX;
    float seedY;
    float seedZ;

    public float speed = 5;

    void Start()
    {

        seedX = Random.Range(-1000, 1000);
        seedY = Random.Range(-1000, 1000);
        seedZ = Random.Range(-1000, 1000);

        originalPosition = transform.position;

    }

    void Update()
    {

        var dt = Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            //Para aumentar gradativamente
            //intensity += growthIntensity * dt;

            //Para aumentar instantaneamente
            intensity = 1; 
        }
        else
        {
            intensity -= decayIntensity * dt;
        }

        intensity = Mathf.Clamp(intensity, 0, 1);

        var intensityExp = intensity * intensity;

        var time = Time.time * speed;

        rotationZ = intensityExp * maxAngleZ * PerlinNoise(seedZ, time);

        offsetX = intensityExp * maxOffsetX * PerlinNoise(seedX, time);
        offsetY = intensityExp * maxOffsetY * PerlinNoise(seedY, time);

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        transform.position = originalPosition + new Vector3(offsetX, offsetY);

    }

    float PerlinNoise(float seed, float time)
    {
        return (1 - 2 * Mathf.PerlinNoise(seed + time, seed + time));
    }
}