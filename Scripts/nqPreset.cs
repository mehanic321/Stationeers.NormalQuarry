using UnityEngine;
using System.Collections;

namespace nqPlugin.Preset
{
    public struct nqPreset
    {
        public const int REPEAT_RATE = 120;
        public const int MIN_COUNT_SPAWN_ORE = 1;
        public const int MAX_COUNT_SPAWN_ORE = 50;
        public const float USED_POWER = 1000f;

        public const float X_OFFSET_FOR_TEXT = .7f;
        public const float Y_OFFSET_FOR_TEXT = 2;

        public const bool TURBO_IS_ACTIVATE = false;
        public const int TURBO_REPEAT_RATE = 60;
        public const int TURBO_MIN_COUNT_SPAWN_ORE = 1;
        public const int TURBO_MAX_COUNT_SPAWN_ORE = 50;
        public const float TURBO_USED_POWER = 5000f;
    }
}