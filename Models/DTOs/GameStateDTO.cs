using CoinDropGamble.Constants;
using CoinDropGamble.Models.Enums;

namespace CoinDropGamble.Models.DTOs
{
    internal class GameStateDTO
    {
        internal const int MAX_COINS_PLAYABLE = 3;
        internal bool IsOpponentsTurn { get; set; }
        internal bool IsPlayersTurn { get; set; }
        internal bool IsNewRoundStarting { get; set; }
        internal bool IsPlayerControlsDisabled { get; set; }
        internal bool IsGameStarted { get; set; }
        internal bool IsRegularPiggyBank { get; set; }
        internal bool HasPlayerShakenThisRound { get; set; }
        internal bool HasOpponentShakenThisRound { get; set; }
        internal bool HasPlayerActedThisTurn { get; set; }
        internal bool HasOpponentActedThisTurn { get; set; }
        internal bool HasOpponentRunOutOfGoodOptions { get; set; }
        internal bool IsPlayerCoinDisabled { get; set; }
        internal bool IsOpponentCoinDisabled { get; set; }
        internal bool IsPlayerShakeDisabled { get; set; }
        internal bool IsOpponentShakeDisabled { get; set; }
        internal bool IsPlayerSpecialDisabled { get; set; }
        internal bool IsOpponentSpecialDisabled { get; set; }
        internal bool IsPlayerEndTurnDisabled { get; set; }
        internal bool IsOpponentEndTurnDisabled { get; set; }
        internal int PiggyBankCoinCount { get; set; }
        internal int GamesWon { get; set; }
        internal int PiggyBankCapacity { get; set; }
        internal int HigherDisparity { get; set; }
        internal int LowerDisparity { get; set; }
        internal int PiggyBankLowerLimit => PiggyBankCapacity - LowerDisparity;
        internal int PiggyBankHigherLimit => PiggyBankCapacity + HigherDisparity;
        internal int PlayerHealthCount { get; set; }
        internal int OpponentHealthCount { get; set; }
        internal int CurrentGameMaxHealth { get; set; }
        internal int PlayerPocketMoneyCount { get; set; }
        internal int OpponentPocketMoneyCount { get; set; }
        internal int PlayerPlayedCoinsCount { get; set; }
        internal int OpponentPlayedCoinsCount { get; set; }
        internal int PlayerPlayedSpecialCoinsCount { get; set; }
        internal int OpponentPlayedSpecialCoinsCount { get; set; }
        internal int CurrentDialogIndex { get; set; }
        internal int RoundCount { get; set; }
        internal int OpponentPiggyBankCapacityGuessLow { get; set; }
        internal int OpponentPiggyBankCapacityGuessHigh { get; set; }
        internal string ImagePath { get; set; }
        internal (string line1, string line2) Dialog { get; set; }
        internal List<(string line1, string line2)> DialogList { get; set; }
        internal List<OpponentAction> OpponentsActionsThisTurn { get; set; }
        internal List<SpecialCoins> PlayerSpecialCoins { get; set; }
        internal List<SpecialCoins> OpponentSpecialCoins { get; set; }
        internal List<SpecialCoins> SpecialCoinShopItems { get; set; }

        internal GameStateDTO()
        {
            IsOpponentsTurn = false;
            IsPlayersTurn = true;
            IsNewRoundStarting = false;
            PiggyBankCoinCount = 0;
            GamesWon = 0;
            PiggyBankCapacity = 0;
            HigherDisparity = 0;
            LowerDisparity = 0;
            ImagePath = PathConstants.PIGGY_BANK_IMAGE_PATH;
            DialogList = new List<(string, string)>();
            Dialog = ("It's your turn. Insert up to 3 coins or shake the swine", "Once you have done all you desire, you may end your turn");
            OpponentsActionsThisTurn = new List<OpponentAction>();
            IsGameStarted = false;
            IsRegularPiggyBank = true;
            HasPlayerShakenThisRound = false;
            HasOpponentShakenThisRound = false;
            HasPlayerActedThisTurn = false;
            HasOpponentActedThisTurn = false;
            HasOpponentRunOutOfGoodOptions = false;
            IsPlayerCoinDisabled = false;
            IsOpponentCoinDisabled = false;
            IsPlayerShakeDisabled = false;
            IsOpponentShakeDisabled = false;
            IsPlayerSpecialDisabled = false;
            IsOpponentSpecialDisabled = false;
            IsPlayerEndTurnDisabled = false;
            IsOpponentEndTurnDisabled = false;
            PlayerHealthCount = 3;
            OpponentHealthCount = 3;
            CurrentGameMaxHealth = 3;
            PlayerPocketMoneyCount = 0;
            OpponentPocketMoneyCount = 0;
            PlayerPlayedCoinsCount = 0;
            OpponentPlayedCoinsCount = 0;
            PlayerPlayedSpecialCoinsCount = 0;
            OpponentPlayedSpecialCoinsCount = 0;
            CurrentDialogIndex = 0;
            RoundCount = 0;
            OpponentPiggyBankCapacityGuessLow = 0;
            OpponentPiggyBankCapacityGuessHigh = 0;
            PlayerSpecialCoins = new List<SpecialCoins>();
            OpponentSpecialCoins = new List<SpecialCoins>();
            SpecialCoinShopItems = new List<SpecialCoins>();
        }
    }
}
