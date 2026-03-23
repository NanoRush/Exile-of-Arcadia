using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private Transform Dagger;
    public Transform daggerTransform;
    private int daggerCount = 0;
    private AudioSource source;
    public AudioClip daggerSwipeSound;

    private float daggerCooldown = 2.5f;
    static public float maxDaggerCooldown = 2.5f;
    public AudioClip cooldownSound;

    public InputActionReference ThrowAction;
    public Cursor CursorScript;

    public event Action<float> OnCooldownChanged;
    public event Action OnCooldownComplete;
    public event Action OnCooldownStarted;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ThrowAction.action.triggered && daggerCount == 0 && !PauseMenu.isPaused)
        {
            ThrowDagger();
            daggerCooldown = 0f;

            OnCooldownStarted?.Invoke();
        }

        if (daggerCooldown < maxDaggerCooldown)
        {
            daggerCooldown += Time.deltaTime;
            if (daggerCooldown > maxDaggerCooldown)
            {
                daggerCooldown = maxDaggerCooldown;
                OnCooldownComplete?.Invoke();
            }

            OnCooldownChanged?.Invoke(daggerCooldown / maxDaggerCooldown);
        }
    }

    public void ThrowDagger()
    {
        Transform dagger = Instantiate(Dagger, daggerTransform.position, Quaternion.identity);
        dagger.GetComponent<DaggerScript>().Setup(CursorScript.AimDirection,CursorScript.transform.rotation);
        daggerCount++;
        source.PlayOneShot(daggerSwipeSound);
    }

    public void daggerReset()
    {
        daggerCount = 0;
    }

    public void PlayCooldownSound()
    {
        source.PlayOneShot(cooldownSound);
    }

    public void fillCooldown()
    {
        daggerCooldown = maxDaggerCooldown;

        OnCooldownChanged?.Invoke(1f);
        OnCooldownComplete?.Invoke();
    }
}
