using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource SFX;
    public bool playOnAwake;
    public float pitchA;
    public float pitchB;

    public bool destroyAfterPlayed;
    float lifetime;
    float currentLifetime;

    public bool playOnCollision;
    private void OnCollisionEnter(Collision collision)
    {
        if (playOnCollision) Play(Mathf.Abs((collision.relativeVelocity.x+ collision.relativeVelocity.y+ collision.relativeVelocity.z)/10));
    }
    void Start()
    {
        if (playOnAwake) Play();
        if (destroyAfterPlayed) lifetime = SFX.clip.length;
    }
    public void Play(float fixedPitch)
    {
        SFX.pitch = fixedPitch;
        SFX.Play();
    }
    public void Play() //Useful for UnityEvent calling
    {
        SFX.pitch = Random.Range(pitchA, pitchB);
        SFX.Play();
    }
    public void RefreshPitch()
    {
        SFX.pitch = Random.Range(pitchA, pitchB);
    }
    void Update()
    {
        if (destroyAfterPlayed)
        {
            currentLifetime += Time.deltaTime;
            if (currentLifetime > lifetime) Destroy(gameObject);
        }
    }
}
