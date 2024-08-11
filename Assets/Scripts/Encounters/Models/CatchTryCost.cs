using System;

namespace Encounters
{
    [Serializable]
    public class CatchTryCost
    {
        public int Cost;
        public Resource Currency;

        public CatchTryCost(int cost, Resource currency)
        {
            Cost = cost;
            Currency = currency;
        }
    }
}