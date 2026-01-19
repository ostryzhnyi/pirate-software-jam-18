using jam.CodeBase.Audio;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// This file is auto-generated. Do not modify manually.

public static class GameResources
{
    public static class CMS
    {
        public static class Characters
        {
            public static CMSEntityPfb Man1CharacterEntity => Resources.Load<CMSEntityPfb>("CMS/Characters/Man1CharacterEntity");
            public static CMSEntityPfb Man2CharacterEntity => Resources.Load<CMSEntityPfb>("CMS/Characters/Man2CharacterEntity");
        }
        public static class Chat
        {
            public static CMSEntityPfb Day1Messages => Resources.Load<CMSEntityPfb>("CMS/Chat/Day1Messages");
            public static CMSEntityPfb ReactionMessages => Resources.Load<CMSEntityPfb>("CMS/Chat/ReactionMessages");
        }
        public static class Tasks
        {
            public static CMSEntityPfb GiveEatTask => Resources.Load<CMSEntityPfb>("CMS/Tasks/GiveEatTask");
            public static CMSEntityPfb MusicOnNight => Resources.Load<CMSEntityPfb>("CMS/Tasks/MusicOnNight");
        }
        public static class Templates
        {
        }
        public static CMSEntityPfb BaseEconomy => Resources.Load<CMSEntityPfb>("CMS/BaseEconomy");
        public static CMSEntityPfb CMSEntity => Resources.Load<CMSEntityPfb>("CMS/CMSEntity");
    }
    public static AudioController AudioController => Resources.Load<AudioController>("AudioController");
    public static CanvasScaler Canvas => Resources.Load<CanvasScaler>("Canvas");
}
