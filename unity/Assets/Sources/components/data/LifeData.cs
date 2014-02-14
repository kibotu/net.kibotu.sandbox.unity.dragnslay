using UnityEngine;

namespace Assets.Sources.components.data
{
    public class LifeData : MonoBehaviour {
        public int MaxHp;
        public int CurrentHp;
        public int Armor;
        public int MaxShield;
        public int CurrentShield;
        public float HpRegen; 	    // hp 			/ sec
        public float ShieldRegen;   // shield_regen / sec
    }
}

