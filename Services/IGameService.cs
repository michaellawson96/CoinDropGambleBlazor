using CoinDropGamble.Models.DTOs;
using CoinDropGamble.Models.Enums;

namespace CoinDropGamble.Services
{
    internal interface IGameService
    {
        // Property to expose the game state
        public GameStateDTO GameState { get; }

        // Methods for controlling the game state and flow
        public void DisableControls();
        public void EnableControls();
        public void GenerateCapacityAndRange();
        public void IntroductionDialog();
        public void ProgressDialogStream();
        public void PerformNextOpponentAction();
        public void StartGameButtonPressed();
        public void PrepareNewRound();
        public void NewTurn();
        public void RunOpponentTurn();
        int CalculateWinChance(out List<OpponentAction> offensiveActions);
        int CalculateLossChance(out List<OpponentAction> defensiveActions);
        int SimulatePlayerWinChance(int simulatedBankCount, bool playerCanShake);
        public void OpponentShakeAction();
        public void OpponentDropCoinAction();
        public void OpponentEndTurnAction();
        public void PlayerDropCoinAction();
        public void CrackPiggyBank();
        public void StartDefeat();
        public void StartVictory();
        public void StartShop();
        public void PlayerShakeAction();
        public void SpecialAction();
        public void EndTurnAction();
    }
}