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

    public Image cooldownBar;
    private float daggerCooldown = 2.5f;
    static public float maxDaggerCooldown = 2.5f;
    public Color cooldownColor;
    public AudioClip cooldownSound;
    private bool cooldownFilled = true;

    public InputActionReference ThrowAction;
    public Cursor CursorScript;
 
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
            cooldownBar.color = Color.red;
            ThrowDagger();
            daggerCooldown = 0f;
        }

        if (daggerCooldown < maxDaggerCooldown)
        {
            daggerCooldown += Time.deltaTime;
            if (daggerCooldown > maxDaggerCooldown)
            {
                daggerCooldown = maxDaggerCooldown;
            }
        }

        CooldownBarFiller();

        if (cooldownFilled)
        {
            cooldownBar.color = cooldownColor;
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

    public void CooldownBarFiller()
    {
        cooldownBar.fillAmount = daggerCooldown / maxDaggerCooldown;
        if (cooldownBar.fillAmount == 1)
        {
            cooldownFilled = true;
        }
        else
        {
            cooldownFilled = false;
        }
    }

    public void PlayCooldownSound()
    {
        source.PlayOneShot(cooldownSound);
    }

    public void fillCooldown()
    {
        daggerCooldown = maxDaggerCooldown;
        CooldownBarFiller();
    }
}
