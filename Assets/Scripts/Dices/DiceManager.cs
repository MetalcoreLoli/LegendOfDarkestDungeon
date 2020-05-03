using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Dices
{
    public static class DiceManager
    {
        public static Dice FourEdges    = "1d4";
        public static Dice SixEdges     = "1d6";
        public static Dice EightEdges   = "1d8";
        public static Dice TenEdges     = "1d10";
        public static Dice TwelveEdges  = "1d12";
        public static Dice TwentyEdges  = "1d20";

        public static Dice[] CreateDicesFromString(string dices) 
        {
            int countOfDices    = int.Parse(dices.Split('d')[0]);
            Dice[] dicesArray   = new Dice[countOfDices];
            
            for (int i = 0; i < countOfDices; i++)
                dicesArray[i] = dices;
            
            return dicesArray;
        }

        public static int RollUndSum(this IEnumerable<Dice> source) => source.Sum(dice => dice.RolledValue);
        public static int RollUndSumFromString(this string source) => CreateDicesFromString(source).RollUndSum();
    }
}
