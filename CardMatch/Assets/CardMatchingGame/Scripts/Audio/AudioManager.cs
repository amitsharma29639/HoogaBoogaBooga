using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Serializable data container that binds an AudioSource with its group and identifier.
/// </summary>
[Serializable]
internal class AudioSourceData
{
    #region Fields

    [SerializeField] private string _audioSourceName; // Unique identifier for this audio source
    [SerializeField] private string _group;           // Group this source belongs to (e.g., BGM, SFX)
    [SerializeField] private AudioSource _audioSource; // Unity AudioSource component

    #endregion


    #region Public Getters

    /// <summary>
    /// Gets the name/identifier of this audio source.
    /// </summary>
    internal string GetAudioSourceName() => _audioSourceName;

    /// <summary>
    /// Gets the group this audio source belongs to.
    /// </summary>
    internal string GetGroup() => _group;

    /// <summary>
    /// Gets the Unity AudioSource component linked to this data.
    /// </summary>
    internal AudioSource GetSource() => _audioSource;

    #endregion
}


/// <summary>
/// Centralized Audio Manager for handling all music and sound effects.
/// Provides control over playback, volume, mute, and loading/unloading of audio data.
/// Implements a Singleton pattern.
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Singleton

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // Load sound/music preferences
        var v_soundKey = PlayerPrefs.GetInt("SoundValue", 1);
        var v_musicKey = PlayerPrefs.GetInt("MusicValue", 1);

        _isSound = v_soundKey != 0;
        _isMusic = v_musicKey != 0;

        SetSound(_isSound);
        SetMusic(_isMusic);
    }

    #endregion


    #region Fields

    private bool _isSound;  // Whether sound effects are enabled
    private bool _isMusic;  // Whether background music is enabled

    [SerializeField] private List<AudioSourceData> _sourceDataList;        // All available AudioSources
    [SerializeField] private AudioGroupMasterData _audioGroupMasterData;   // Reference to all audio groups

    #endregion


    #region Public Getters/Setters

    /// <summary>
    /// Gets whether sound effects are enabled.
    /// </summary>
    internal bool GetSound() => _isSound;

    /// <summary>
    /// Gets whether music is enabled.
    /// </summary>
    internal bool GetMusic() => _isMusic;

    /// <summary>
    /// Enables or disables sound effects globally.
    /// </summary>
    internal void SetSound(bool value)
    {
        _isSound = value;
        PlayerPrefs.SetInt("SoundValue", value ? 1 : 0);

        foreach (var v_sourceData in _sourceDataList)
        {
            if (v_sourceData.GetGroup().Equals(AudioGroupConstants.BGM))
            {
                v_sourceData.GetSource().mute = !value;
            }
        }
    }

    /// <summary>
    /// Enables or disables music globally.
    /// </summary>
    internal void SetMusic(bool value)
    {
        _isMusic = value;
        PlayerPrefs.SetInt("MusicValue", value ? 1 : 0);

        foreach (var v_sourceData in _sourceDataList)
        {
            if (v_sourceData.GetGroup().Equals(AudioGroupConstants.GAMEPLAYSFX))
            {
                v_sourceData.GetSource().mute = !value;
            }
        }
    }

    #endregion


    #region Playback Controls

    /// <summary>
    /// Pauses a specific audio source.
    /// </summary>
    internal void Pause(string sourceName) => GetAudioSource(sourceName)?.Pause();

    /// <summary>
    /// Stops a specific audio source.
    /// </summary>
    internal void Stop(string sourceName) => GetAudioSource(sourceName)?.Stop();

    /// <summary>
    /// Plays an audio clip on a specific source.
    /// </summary>
    public void Play(string sourceName, string clipName, string groupName = default, float volume = 0.8f, float delay = 0f)
    {
        var v_clip = GetClip(clipName, groupName);
        if (v_clip == null || v_clip.loadState != AudioDataLoadState.Loaded)
        {
            Debug.LogFormat("Audio Clip is either null or not loaded {0}, {1}", v_clip, v_clip?.loadState);
            return;
        }

        var v_audioSource = GetAudioSource(sourceName);
        if (v_audioSource == null) return;

        v_audioSource.Stop();
        v_audioSource.clip = v_clip;
        v_audioSource.volume = volume;

        if (delay > 0f)
            v_audioSource.PlayDelayed(delay);
        else
            v_audioSource.Play();
    }

    /// <summary>
    /// Plays a one-shot audio clip on a specific source.
    /// </summary>
    public void PlayOneShot(string sourceName, string clipName, string groupName = default, float volume = 1f, float delay = 0f)
    {
        var v_clip = GetClip(clipName, groupName);
        if (v_clip == null || v_clip.loadState != AudioDataLoadState.Loaded)
        {
            Debug.LogFormat("Audio Clip is either null or not loaded {0}, {1}", v_clip, v_clip?.loadState);
            return;
        }

        var v_audioSource = GetAudioSource(sourceName);
        if (v_audioSource == null) return;

        if (delay > 0f)
            StartCoroutine(DelayedCall(delay, () => v_audioSource.PlayOneShot(v_clip, volume)));
        else
            v_audioSource.PlayOneShot(v_clip, volume);
    }

    /// <summary>
    /// Gets the length of an audio clip by name and group.
    /// </summary>
    internal float GetClipLength(string clipName, string groupName = default)
    {
        var v_clip = GetClip(clipName, groupName);
        return v_clip == null ? 0f : v_clip.length;
    }

    #endregion


    #region Audio Loading/Unloading

    /// <summary>
    /// Loads all clips in a specific audio group into memory.
    /// </summary>
    internal void LoadAudioGroup(string groupName)
    {
        var v_audioGroup = _audioGroupMasterData.GetGroup(groupName);
        var v_clips = v_audioGroup?.GetClipsData();

        foreach (var v_clip in v_clips)
        {
            if (v_clip._audioClip.loadState != AudioDataLoadState.Loaded)
            {
                v_clip._audioClip.LoadAudioData();
            }
        }
    }

    /// <summary>
    /// Unloads all clips in a specific audio group from memory.
    /// </summary>
    internal void UnloadAudioGroup(string groupName)
    {
        var v_audioGroup = _audioGroupMasterData.GetGroup(groupName);
        var v_clips = v_audioGroup?.GetClipsData();

        foreach (var v_clip in v_clips)
        {
            if (v_clip._audioClip.loadState != AudioDataLoadState.Unloaded)
            {
                v_clip._audioClip.UnloadAudioData();
            }
        }
    }

    /// <summary>
    /// Unloads all clips across all audio groups from memory.
    /// </summary>
    internal void UnloadAll()
    {
        var v_groups = _audioGroupMasterData.GetGroups();
        if (v_groups == null) return;

        foreach (var v_group in v_groups)
        {
            var v_clips = v_group.GetClipsData();
            if (v_clips == null) continue;

            foreach (var v_clip in v_clips)
            {
                if (v_clip._audioClip.loadState != AudioDataLoadState.Unloaded)
                {
                    v_clip._audioClip.UnloadAudioData();
                }
            }
        }
    }

    #endregion


    #region Private Helpers

    /// <summary>
    /// Finds and returns an audio clip by name, optionally within a specific group.
    /// </summary>
    private AudioClip GetClip(string clipName, string audioGroup)
    {
        AudioClip v_clip = null;

        if (string.IsNullOrEmpty(audioGroup))
        {
            var v_groups = _audioGroupMasterData.GetGroups();
            foreach (var v_data in v_groups)
            {
                v_clip = v_data.GetClip(clipName);
                if (v_clip != null) break;
            }
        }
        else
        {
            var v_group = _audioGroupMasterData.GetGroup(audioGroup);
            v_clip = v_group?.GetClip(clipName);
        }

        return v_clip;
    }

    /// <summary>
    /// Finds and returns an AudioSource by its name.
    /// </summary>
    private AudioSource GetAudioSource(string sourceName)
    {
        if (_sourceDataList == null) return null;

        return _sourceDataList.Find(x => x.GetAudioSourceName().Equals(sourceName))?.GetSource();
    }

    /// <summary>
    /// Executes a callback after a delay.
    /// </summary>
    private IEnumerator DelayedCall(float delay, Action onComplete)
    {
        yield return new WaitForSecondsRealtime(delay);
        onComplete?.Invoke();
    }

    #endregion
}
