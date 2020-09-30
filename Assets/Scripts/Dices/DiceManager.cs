using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Dices
{
    public static class DiceManager
    {
        public static Dice FourEdges = "1d4";
        public static Dice SixEdges = "1d6";
        public static Dice EightEdges = "1d8";
        public static Dice TenEdges = "1d10";
        public static Dice TwelveEdges = "1d12";
        public static Dice TwentyEdges = "1d20";

        public static Random Random = new Random();

        public static Dice[] CreateDicesFromString(string dices)
        {
            int countOfDices = int.Parse(dices.Split('d')[0]);
            Dice[] dicesArray = new Dice[countOfDices];

            for (int i = 0; i < countOfDices; i++)
                dicesArray[i] = dices;

            return dicesArray;
        }

        public static int RollDice(Dice dice) => dice.Roll(Random);

        public static int RollUndSum(this IEnumerable<Dice> source) => source.Sum(dice => dice.Roll(Random));

        public static int RollUndSumFromString(this string source) => CreateDicesFromString(source).RollUndSum();
    }
}