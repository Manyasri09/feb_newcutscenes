using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface defining audio control operations
public interface ISettingsButPanelAudioController
{
    bool IsMusicEnabled { get; }
    bool IsSoundEnabled { get; }
    void ToggleMusic();
    void ToggleSound();
    event System.Action<bool> OnMusicStateChanged;
    event System.Action<bool> OnSoundStateChanged;
}