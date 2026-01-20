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
            public static CMSEntityPfb AddBeesToTheRoom => Resources.Load<CMSEntityPfb>("CMS/Tasks/AddBeesToTheRoom");
            public static CMSEntityPfb DrinkAllSleepPils => Resources.Load<CMSEntityPfb>("CMS/Tasks/DrinkAllSleepPils");
            public static CMSEntityPfb EnableAC => Resources.Load<CMSEntityPfb>("CMS/Tasks/EnableAC");
            public static CMSEntityPfb FriendsCall => Resources.Load<CMSEntityPfb>("CMS/Tasks/FriendsCall");
            public static CMSEntityPfb GiftSleepPils => Resources.Load<CMSEntityPfb>("CMS/Tasks/GiftSleepPils");
            public static CMSEntityPfb GiveEatTask => Resources.Load<CMSEntityPfb>("CMS/Tasks/GiveEatTask");
            public static CMSEntityPfb MusicOnNight => Resources.Load<CMSEntityPfb>("CMS/Tasks/MusicOnNight");
            public static CMSEntityPfb PlayMovie => Resources.Load<CMSEntityPfb>("CMS/Tasks/PlayMovie");
            public static CMSEntityPfb TakeShower => Resources.Load<CMSEntityPfb>("CMS/Tasks/TakeShower");
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
