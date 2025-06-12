using UnityEngine;

namespace XMod
{
    public class SimpleSoundPlayer : MonoBehaviour
    {
        [field: SerializeField]
        public bool PlayOnStart { get; private set; }

        [field: SerializeField]
        public string ChannelGroupName { get; private set; } = "master_channel_group";

        [field: SerializeField]
        public Sound Sound { get; private set; }

        private void Start()
        {
            if (Sound == null)
            {
                Debug.LogWarning($"No sound assigned to {gameObject.name}");
                return;
            }

            if (PlayOnStart)
                Sound.Play(ChannelGroupName);
        }
    }
}
