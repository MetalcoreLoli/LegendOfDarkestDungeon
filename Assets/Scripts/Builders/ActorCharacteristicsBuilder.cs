using Assets.Scripts.Stats;

namespace Assets.Scripts.Builders
{
    public class ActorCharacteristicsBuilder
    {
        protected ActorCharacteristics characteristics;

        public ActorCharacteristicsBuilder()
        {
            characteristics = new ActorCharacteristics();
        }

        public ActorCharacteristicsBuilder WithHp(int hp)
        {
            characteristics.Hp = hp;
            return this;
        }

        public ActorCharacteristicsBuilder WithMaxHp(int hp)
        {
            characteristics.MaxHp = hp;
            return this;
        }

        public ActorCharacteristicsBuilder WithMp(int mp)
        {
            characteristics.Mp = mp;
            return this;
        }

        public ActorCharacteristicsBuilder WithMaxMp(int mp)
        {
            characteristics.MaxMp = mp;
            return this;
        }

        public ActorCharacteristics Build() => characteristics;

        public ActorCharacteristicsModificatorBuilder Modifactor => new ActorCharacteristicsModificatorBuilder(characteristics);
        public ActorCharacteristicsStatsBuilder Characteristic => new ActorCharacteristicsStatsBuilder(characteristics);
    }

    public class ActorCharacteristicsStatsBuilder : ActorCharacteristicsBuilder
    {
        public ActorCharacteristicsStatsBuilder(ActorCharacteristics characteristics)
        {
            this.characteristics = characteristics;
        }

        public ActorCharacteristicsStatsBuilder IntelligenceWithValue(int intelligence)
        {
            characteristics.Intelligence = intelligence;
            return this;
        }

        public ActorCharacteristicsStatsBuilder CharismaWithValue(int charisma)
        {
            characteristics.Charisma = charisma;
            return this;
        }

        public ActorCharacteristicsStatsBuilder DexterityWithValue(int dexterity)
        {
            characteristics.Dexterity = dexterity;
            return this;
        }

        public ActorCharacteristicsStatsBuilder StrengthWithValue(int strength)
        {
            characteristics.Strength = strength;
            return this;
        }

        public ActorCharacteristicsStatsBuilder LuckyWithValue(int lucky)
        {
            characteristics.Lucky = lucky;
            return this;
        }
    }

    public class ActorCharacteristicsModificatorBuilder : ActorCharacteristicsBuilder
    {
        public ActorCharacteristicsModificatorBuilder(ActorCharacteristics characteristics)
        {
            this.characteristics = characteristics;
        }

        public ActorCharacteristicsModificatorBuilder CalculateIntelligence()
        {
            characteristics.IntelligenceMod = CalculateModificators(characteristics.Intelligence);
            return this;
        }

        public ActorCharacteristicsModificatorBuilder CalculateCharisma()
        {
            characteristics.CharismaMod = CalculateModificators(characteristics.Dexterity);
            return this;
        }

        public ActorCharacteristicsModificatorBuilder CalculateDexterity()
        {
            characteristics.DexterityMod = CalculateModificators(characteristics.Dexterity);
            return this;
        }

        public ActorCharacteristicsModificatorBuilder CalculateStrength()
        {
            characteristics.StrengthMod = CalculateModificators(characteristics.Strength);
            return this;
        }

        public ActorCharacteristicsModificatorBuilder CalculateLucky()
        {
            characteristics.LuckyMod = CalculateModificators(characteristics.Lucky);
            return this;
        }

        protected virtual int CalculateModificators(int value)
             => (value - 10 != 0) ? (value - 10) / 2 : 0;
    }
}