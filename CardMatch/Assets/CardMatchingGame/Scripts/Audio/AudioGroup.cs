using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject representing a group of audio clips.
/// Provides methods to retrieve audio clips and group information.
/// </summary>
[CreateAssetMenu(fileName = "AudioGroup", menuName = "ScriptableObjects/AudioGroup")]
internal class AudioGroup : ScriptableObject
{
    #region Serialized Fields

    [SerializeField] private string _groupName; // Name of the audio group
    [SerializeField] private List<AudioClipData> _audioClipsData; // List of audio clip data

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets all audio clip data in the group.
    /// </summary>
    /// <returns>IEnumerable of AudioClipData.</returns>
    internal IEnumerable<AudioClipData> GetClipsData() => _audioClipsData;

    /// <summary>
    /// Gets an audio clip by its name.
    /// </summary>
    /// <param name="name">Name of the audio clip.</param>
    /// <returns>AudioClip if found, otherwise null.</returns>
    internal AudioClip GetClip(string name)
    {
        if (_audioClipsData != null)
        {
            foreach (var _audioData in _audioClipsData)
            {
                if (_audioData._audioName.Equals(name))
                    return _audioData._audioClip;
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the name of the audio group.
    /// </summary>
    /// <returns>Group name as string.</returns>
    internal string GetGroupName() => _groupName;

    #endregion
}

/// <summary>
/// Serializable class representing audio clip data.
/// Stores the name and reference to an AudioClip.
/// </summary>
[System.Serializable]
internal class AudioClipData
{
    #region Serialized Fields

    [SerializeField] internal string _audioName; // Name of the audio clip
    [SerializeField] internal AudioClip _audioClip; // Reference to the audio clip

    #endregion
}