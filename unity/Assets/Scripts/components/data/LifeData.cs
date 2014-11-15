using UnityEngine;

namespace Assets.Sources.components.data
{
    public class LifeData : MonoBehaviour {

        public float MaxHp;
        public float CurrentHp;
        public float Armor;
        public float MaxShield;
        public float CurrentShield;
        public float ShieldStrength;
        public float HpRegen; 	    // hp 			/ sec
        public float ShieldRegen;   // shield_regen / sec

        private float _hpRegenTime;
        private float _shieldRegenTime;
        public static float RegenerationTick = 1f;

        public void Start()
        {
            _hpRegenTime = 0;
            _shieldRegenTime = 0;
        }

        public void Update()
        {
            // hp regeneration by the amount of hpregen each second
            if (CurrentHp < MaxHp)
            {
                _hpRegenTime += Time.deltaTime;
                if (_hpRegenTime >= RegenerationTick)
                {
                    _hpRegenTime -= RegenerationTick;
                    CurrentHp += HpRegen;
                }
            }

            // shield regeneration by the amount of shieldregen each second
            if (CurrentShield < MaxShield)
            {
                _shieldRegenTime += Time.deltaTime;
                if (_shieldRegenTime >= RegenerationTick)
                {
                    _shieldRegenTime -= RegenerationTick;
                    CurrentShield += ShieldRegen;
                }
            }
        }
    }
}