// File: Models/Enums/SpecialCoins.cs

using System.ComponentModel;

namespace CoinDropGamble.Models.Enums
{
    public enum SpecialCoins
    {
        [Description("x2 Coin - Splits into 2 coins inside the piggy bank")]
        TimesTwoCoin,

        [Description("x3 Coin - Splits into 3 coins inside the piggy bank")]
        TimesThreeCoin,

        [Description("Ejector Coin - Ejects a random amount of coins from the piggy bank")]
        EjectorCoin,

        [Description("Poach Coin - Lets you steal a special coin from your opponent")]
        PoachCoin,

        [Description("Scry Coin - Lets you peer into the future to make better choices")]
        ScryCoin,

        [Description("Extra Coin - Doesn't count towards your max 3 coins per turn")]
        ExtraCoin,

        [Description("Skip Coin - When your turn ends, your opponent's next turn will be skipped")]
        SkipCoin,

        [Description("Mirror Coin - Has the same effect as the last special coin played")]
        MirrorCoin,

        [Description("Shield Coin - Your head will be protected for this turn")]
        ShieldCoin,

        [Description("Ghost Coin - Disappears after it is inserted")]
        GhostCoin
    }
}
