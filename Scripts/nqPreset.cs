using UnityEngine;
using System.Collections;

namespace nqPlugin.Preset
{
    public struct nqPreset
    {

        //����� ������� ������ �������� ���� / how many seconds does it take for ore to spawn
        public const int REPEAT_RATE = 240;

        //����������� ���������� ���� ������� �������� / the minimum amount of ore that will spawn
        public const int MIN_COUNT_SPAWN_ORE = 1;

        //������������ ���������� ���� ������� �������� / the maximum amount of ore that will spawn
        public const int MAX_COUNT_SPAWN_ORE = 50;

        //��������� � ������� ������ ������������ ���� / distance at which a quarry can detect uranium
        public const float DISTANCE_FOR_URANIUM = 100;

        //�������� �� ������� ������ 245 ������ / check for upgrade is 245 seconds
        public const float TIMER_FOR_FACTOR_UPGRADE = 245;

        //����� ����� ����� ������� ��������� ���� �� ��� ������� / we divide the time after how much the ore spawns by these factors
        public const float SPEED_FACTOR_X0 = 1;
        public const float SPEED_FACTOR_X1 = 2;
        public const float SPEED_FACTOR_X2 = 2.5f;
        public const float SPEED_FACTOR_X3 = 4;

        //������� ����� ����� ��� �������� / how much uranium is needed to upgrade
        public const int NEED_URANIUM_X1 = 1000;
        public const int NEED_URANIUM_X2 = 3000;
        public const int NEED_URANIUM_X3 = 5000;

        //������� ����� ������������� �� ������ ������� �������� / how much electricity is needed at different upgrade points
        public const float USED_POWER_X0 = 200f;
        public const float USED_POWER_X1 = 2000f;
        public const float USED_POWER_X2 = 5000f;
        public const float USED_POWER_X3 = 10000f;
       
        //�������� ������ � ������� �� � / quarry text offset by X
        public const float X_OFFSET_FOR_TEXT = .7f;

        //�������� ������ � ������� �� Y / quarry text offset by Y
        public const float Y_OFFSET_FOR_TEXT = 2;
    }
}