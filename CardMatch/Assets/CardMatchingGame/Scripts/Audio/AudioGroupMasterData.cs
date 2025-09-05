using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds and manages a collection of audio groups.
/// Provides access to specific groups and their audio clips.
/// </summary>
[CreateAssetMenu(fileName = "AudioGroupMasterData", menuName = "ScriptableObjects/AudioGroupMasterData")]
internal class AudioGroupMasterData : ScriptableObject
{
    #region Fields

    [SerializeField] 
    private List<AudioGroup> _audioGroups; // List of all audio groups stored in this master data

    #endregion


    #region Public Methods

    /// <summary>
    /// Retrieves an <see cref="AudioGroup"/> by its name.
    /// </summary>
    /// <param name="name">The name of the audio group.</param>
    /// <returns>The matching audio group, or null if not found.</returns>
    internal AudioGroup GetGroup(string name)
    {
        if (string.IsNullOrEmpty(name))
            return default;

        // Perform case-insensitive search for the group
        return _audioGroups.Find(x => 
            x.GetGroupName().ToLower().Equals(name.ToLower()));
    }

    /// <summary>
    /// Returns all audio groups stored in this master data.
    /// </summary>
    internal IEnumerable<AudioGroup> GetGroups() => _audioGroups;

    /// <summary>
    /// Retrieves an audio clip by its name within a specific audio group.
    /// </summary>
    /// <param name="audioName">The name of the audio clip.</param>
    /// <param name="audioGroup">The group the clip belongs to.</param>
    /// <returns>The matching <see cref="AudioClip"/>, or null if not found.</returns>
    internal AudioClip GetClip(string audioName, string audioGroup)
    {
        if (string.IsNullOrEmpty(audioGroup))
            return null;

        var group = GetGroup(audioGroup);
        return group?.GetClip(audioName);
    }

    #endregion
}