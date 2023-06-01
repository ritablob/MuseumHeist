using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles all sound effects during gameplay
/// receives message which sound to play via event manager
/// then plays oneshot of corresponding sound
/// has a reference to each clip and their volume
/// click and select have a cooldown so they can't be triggered too closely
/// all functions that play a oneshot are also available to play via the editor for easier mixing
/// </summary>

public class SFX : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float cooldown = 0.1f;

    [SerializeField] private AudioClip click;
    [SerializeField] [Range (0, 1)] private float clickVol = 0.7f;
    private bool canPlayClick;

    [SerializeField] private AudioClip select;
    [SerializeField][Range(0, 1)] private float selectVol = 0.7f;
    private bool canPlaySelect;

    [SerializeField] private AudioClip invisibleOn;
    [SerializeField][Range(0, 1)] private float invisibleOnVol = 0.7f;

    [SerializeField] private AudioClip invisibleOff;
    [SerializeField][Range(0, 1)] private float invisibleOffVol = 0.7f;

    [SerializeField] private AudioClip puzzle;
    [SerializeField][Range(0, 1)] private float puzzleVol = 0.7f;

    [SerializeField] private AudioClip collect;
    [SerializeField][Range(0, 1)] private float collectVol = 0.7f;

    void Start()
    {
        canPlayClick = true;
        canPlaySelect = true;
        audioSource = GetComponent<AudioSource>();

        EventManager.Instance.AddEventListener("AUDIO", AudioListener);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener("AUDIO", AudioListener);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPlayClick) PlayClick();
    }

    void AudioListener (string eventName, object param)
    {
        if (eventName == "Click" && canPlayClick) PlayClick();
        else if (eventName == "Select" && canPlaySelect) PlaySelect();
        else if (eventName == "InvisibleOn") InvisibleOn();
        else if (eventName == "InvisibleOff") InvisibleOff();
        else if (eventName == "Puzzle") Puzzle();
        else if (eventName == "Collect") Collect();
    }

    [ContextMenu("click")]
    public void PlayClick()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(click, clickVol);
        StartCoroutine(ClickCooldown());
    }

    [ContextMenu("select")]
    public void PlaySelect()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(select, selectVol);
        StartCoroutine(SelectCooldown());
    }

    [ContextMenu("invisible on")]
    public void InvisibleOn()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(invisibleOn, invisibleOnVol);
    }

    [ContextMenu("invisible off")]
    public void InvisibleOff()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(invisibleOff, invisibleOffVol);
    }

    [ContextMenu("puzzle")]
    public void Puzzle()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(puzzle, puzzleVol);
    }

    [ContextMenu("collect")]
    public void Collect()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(collect, collectVol);
    }

    IEnumerator ClickCooldown()
    {
        canPlayClick = false;
        yield return new WaitForSeconds(cooldown);
        canPlayClick = true;
    }

    IEnumerator SelectCooldown()
    {
        canPlaySelect = false;
        yield return new WaitForSeconds(cooldown);
        canPlaySelect = true;
    }
}
