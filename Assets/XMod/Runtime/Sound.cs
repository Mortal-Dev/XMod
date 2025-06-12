using FMOD;
using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace XMod
{
    [Serializable]
    public class Sound
    {
        public static float GlobalVolumeMultiplier
        {
            get => globalVolumeMultiplier;
            set
            {
                globalVolumeMultiplier = value;
                
                foreach (Sound sound in activeSounds)
                    sound.EventInstance.setVolume(sound.volume * globalVolumeMultiplier);
            }
        }

        private static float globalVolumeMultiplier = 1.0f;

        public static IReadOnlyList<Sound> ActiveSounds => activeSounds;

        private static readonly List<Sound> activeSounds = new();

        [field: SerializeField]
        public EventReference EventReference { get; private set; }

        [field: SerializeField]
        public GameObject FollowGameObject { get; private set; }

        [field: SerializeField]
        public bool PlayOnce { get; private set; }

        public float Volume 
        { 
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                EventInstance.setVolume(volume * globalVolumeMultiplier);
            }
        }

        [SerializeField]
        private float volume = 1.0f;

        [field: SerializeField]
        public bool Playing { get; private set; }

        public bool PlayedOnce { get; private set; }

        public EventInstance EventInstance {  get; private set; }

        private static readonly Dictionary<string, ChannelGroup> channelGroups = new();

        /// <summary>
        /// Plays the sound, optionally assigning it to a named channel group
        /// </summary>
        /// <param name="channelGroupName"></param>
        /// <returns>returns an FMOD <see cref="FMOD.Studio.EventInstance"/>> created from <see cref="EventReference"/></returns>
        public EventInstance Play(string channelGroupName = "master_channel_group")
        {
            if (PlayOnce && PlayedOnce)
                return default;

            PlayedOnce = true;

            EventInstance = RuntimeManager.CreateInstance(EventReference);
            EventInstance.setVolume(volume * globalVolumeMultiplier);

            if (!string.IsNullOrEmpty(channelGroupName))
                AttachInstanceToChannelGroup(channelGroupName);
            
            RuntimeManager.AttachInstanceToGameObject(EventInstance, FollowGameObject);

            EventInstance.setCallback(CleanupSound, EVENT_CALLBACK_TYPE.STOPPED);

            EventInstance.start();
            Playing = true;
            activeSounds.Add(this);

            return EventInstance;
        }

        /// <summary>
        /// stops the sound and releases <see cref="EventInstance"/>
        /// </summary>
        /// <param name="stopMode"></param>
        public void Stop(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
        {
            if (!Playing || !EventInstance.isValid())
                return;

            EventInstance.stop(stopMode);
            EventInstance.clearHandle();
            EventInstance.release();
            EventInstance = default;
            Playing = false;
        }

        /// <summary>
        /// Attaches the sound instance to a specified FMOD channel group.
        /// </summary>
        /// <param name="channelGroupName">custom channel name. Channel will be created if not already found</param>
        private void AttachInstanceToChannelGroup(string channelGroupName)
        {
            EventInstance.getChannelGroup(out ChannelGroup eventInstanceChannelGroup);

            if (channelGroups.ContainsKey(channelGroupName))
            {
                channelGroups[channelGroupName].addGroup(eventInstanceChannelGroup);
            }
            else
            {
                RuntimeManager.CoreSystem.createChannelGroup(channelGroupName, out ChannelGroup newChannelGroup);
                newChannelGroup.addGroup(eventInstanceChannelGroup);
                channelGroups.Add(channelGroupName, newChannelGroup);
            }

        }

        private static RESULT CleanupSound(EVENT_CALLBACK_TYPE type, IntPtr _event, IntPtr parameters)
        {
            activeSounds.RemoveAll(sound => sound.EventInstance.handle.Equals(_event));

            return RESULT.OK;
        }
    }
}