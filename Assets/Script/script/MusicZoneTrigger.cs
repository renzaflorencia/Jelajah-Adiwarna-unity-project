using UnityEngine;

public class MusicZoneTrigger : MonoBehaviour
{
    public enum ZoneType { ZoneA, ZoneB, ZoneC }
    public ZoneType zoneType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (zoneType)
            {
                case ZoneType.ZoneA:
                    SoundSystem.instance.ChangeMusicZone(SoundSystem.instance.zoneA_Music);
                    break;
                case ZoneType.ZoneB:
                    SoundSystem.instance.ChangeMusicZone(SoundSystem.instance.zoneB_Music);
                    break;
                case ZoneType.ZoneC:
                    SoundSystem.instance.ChangeMusicZone(SoundSystem.instance.zoneB_Music);
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundSystem.instance.StopZoneMusic();
        }
    }
}
