using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections.Concurrent;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
public class Sound
{
    public string id;

    [Tooltip("One or more clips. One will be chosen at random.")]
    public AudioClip[] clips;

    [Header("Playback")]
    public bool loop = false;

    [Header("Range")]
    public float minDistance = 1f;
    public float maxDistance = 1000f;
}

    public static AudioManager instance;

    [Header("Sounds")]
    public Sound[] allSounds;

    [Header("Pooling")]
    [SerializeField] private int initialPoolSize = 16;

    private readonly Dictionary<string, Sound> _soundDict = new();
    private readonly Queue<AudioSource> _sourcePool = new();

    // Thread-safe main-thread dispatch
    private readonly ConcurrentQueue<Action> _mainThreadQueue = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var s in allSounds)
        {
            if (!string.IsNullOrEmpty(s.id))
                _soundDict[s.id] = s;
        }

        for (int i = 0; i < initialPoolSize; i++)
            _sourcePool.Enqueue(CreateSource());
    }

    private void Update()
    {
        while (_mainThreadQueue.TryDequeue(out var action))
            action();
    }

    // -------------------- PUBLIC API --------------------

    public void Play(string id, Vector3 position)
    {
        if (IsMainThread())
            PlayInternal(id, position);
        else
            _mainThreadQueue.Enqueue(() => PlayInternal(id, position));
    }

    // -------------------- INTERNAL --------------------

    private void PlayInternal(string id, Vector3 position)
{
    if (!_soundDict.TryGetValue(id, out var sound)) return;
    if (sound.clips == null || sound.clips.Length == 0) return;

    var src = GetSource();

    src.clip = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
    src.loop = sound.loop;

        src.transform.position = position;
        src.spatialBlend = 1f; // full 3D
        src.minDistance = sound.minDistance;
        src.maxDistance = sound.maxDistance; // adjust as needed
        src.rolloffMode = AudioRolloffMode.Logarithmic;

    src.Play();

    if (!sound.loop)
        StartCoroutine(ReturnWhenFinished(src));
}

    private AudioSource GetSource()
    {
        return _sourcePool.Count > 0 ? _sourcePool.Dequeue() : CreateSource();
    }

    private AudioSource CreateSource()
    {
        var go = new GameObject("AudioSource");
        go.transform.SetParent(transform);

        var src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;

        return src;
    }

    private System.Collections.IEnumerator ReturnWhenFinished(AudioSource src)
    {
        yield return new WaitWhile(() => src.isPlaying);

        src.clip = null;
        _sourcePool.Enqueue(src);
    }

    private static bool IsMainThread()
    {
        return System.Threading.Thread.CurrentThread.ManagedThreadId == 1;
    }

        public void StopAllLoops()
    {
        foreach (Transform child in transform)
        {
            var src = child.GetComponent<AudioSource>();
            if (src != null && src.loop && src.isPlaying)
            {
                src.Stop();
                src.clip = null;
                _sourcePool.Enqueue(src);
            }
        }
    }
}
