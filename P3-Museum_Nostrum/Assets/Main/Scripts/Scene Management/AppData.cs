using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DDOL))]
public class AppData : MonoBehaviour {

    private static AppData instance = null;

    public static AppData Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// stores the volume levels for audio.
    /// </summary>
    public static class AudioSettings
    {
        //public because they need to be set from another script
        public static float masterVolume;
        public static float musicVolume;
        public static float soundVolume;
    }

    //initializes values when game launches.
    private void InitializeAudioSettings()
    {
        AudioSettings.masterVolume = 1f;
        AudioSettings.musicVolume = 1f;
        AudioSettings.soundVolume = 1f;
    }

    /// <summary>
    /// stores the enable/disable values for test factors.
    /// </summary>
    public static class TestFactorSettings
    {
        /// <summary>
        /// stores whether audio hints meant for pathfinding and orientation are audible to the player.
        /// </summary>
        public static bool audioBreadcrumsAudible;
        /// <summary>
        /// stores whether light sources meant for pathfinding and orientation are visible to the player.
        /// </summary>
        public static bool lightsShining;
        /// <summary>
        /// stores whether interior furnishing and landmarks meant for pathfinding and orientation are visible to the player.
        /// </summary>
        public static bool interiorFurnishingVisible;
        /// <summary>
        /// stores whether sign posts meant for pathfinding and orientation are visible to the player.
        /// </summary>
        public static bool signPostsVisible;
    }

    //initializes values when game launches.
    private void InitializeTestFactorSettings()
    {
        TestFactorSettings.audioBreadcrumsAudible       = true;
        TestFactorSettings.lightsShining                = true;
        TestFactorSettings.interiorFurnishingVisible    = true;
        TestFactorSettings.signPostsVisible             = true;
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //initialize values when game starts.
            InitializeAudioSettings();
            InitializeTestFactorSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
