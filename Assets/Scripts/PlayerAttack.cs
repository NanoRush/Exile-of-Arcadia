using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private Transform Dagger;
    public Transform daggerTransform;
    private int daggerCount = 0;
    private AudioSource source;
    public AudioClip daggerSwipeSound;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && daggerCount == 0 && !PauseMenu.isPaused)
        {
            ThrowDagger();
        }
    }

    public void ThrowDagger()
    {
        Instantiate(Dagger, daggerTransform.position, Quaternion.identity);
        daggerCount++;
        source.PlayOneShot(daggerSwipeSound);
    }

    public void daggerReset()
    {
        daggerCount = 0;
    }
}
