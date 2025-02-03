using CoinDropGamble.Models.DTOs;
using CoinDropGamble.Models.Enums;

namespace CoinDropGamble.Services
{
    internal class GameService : IGameService
    {
        public GameStateDTO GameState { get; } = new();
        public void DisableControls() => GameState.IsPlayerControlsDisabled = true;
        public void EnableControls() => GameState.IsPlayerControlsDisabled = false;
        public void GenerateCapacityAndRange()
        {
            // Random number generator
            Random random = new Random();

            // Generate a random capacity between 3 and 20
            GameState.PiggyBankCapacity = random.Next(3, 21);

            // Determine the increment range based on games won
            int incrementMax;
            if (GameState.GamesWon <= 10)
                incrementMax = 1;
            else if (GameState.GamesWon <= 20)
                incrementMax = random.Next(1, 3); // Random between 1 and 2
            else if (GameState.GamesWon <= 30)
                incrementMax = random.Next(1, 4); // Random between 1 and 3
            else if (GameState.GamesWon <= 40)
                incrementMax = random.Next(1, 5); // Random between 1 and 4
            else
                incrementMax = random.Next(1, 6); // Random between 1 and 5

            // Reset disparities to 0
            GameState.HigherDisparity = 0;
            GameState.LowerDisparity = 0;

            // Apply increments to disparities
            for (int i = 0; i < incrementMax; i++)
            {
                bool incrementHigherDisparity = true;

                // Boundary checks
                if (GameState.PiggyBankCapacity + GameState.HigherDisparity == 20)
                    incrementHigherDisparity = false; // Force increment to lower disparity
                else if (GameState.PiggyBankCapacity - GameState.LowerDisparity == 3)
                    incrementHigherDisparity = true; // Force increment to higher disparity
                else
                    incrementHigherDisparity = random.Next(0, 2) == 0; // 50/50 chance otherwise

                if (incrementHigherDisparity)
                    GameState.HigherDisparity++;
                else
                    GameState.LowerDisparity++;
            }
        }
        public void IntroductionDialog()
        {
            GameState.DialogList.Add(("Greetings!","And welcome to the game"));
            GameState.DialogList.Add(("Survive!","For as long as you can"));
            GameState.DialogList.Add(("Above the swine, a range","The swine's limit is within that range"));
            GameState.DialogList.Add(("Fill the swine","And its fortune shall be yours"));
            GameState.DialogList.Add(("You may shake the swine once","To discover its contents"));
            GameState.DialogList.Add(("Shake, or provide","You must choose one"));
        }
        public void ProgressDialogStream(){
            GameState.DialogList.RemoveAt(0);
            if(GameState.IsOpponentsTurn)
            {
                PerformNextOpponentAction();
            }
            else if(GameState.IsPlayersTurn && !GameState.DialogList.Any())
            {
                EnableControls();
            }
            else if(!GameState.IsOpponentsTurn && !GameState.IsPlayersTurn && GameState.IsNewRoundStarting)
            {
                EnableControls();
                GameState.IsPlayersTurn = true;
            }
        }
        public void PerformNextOpponentAction()
        {
            if(GameState.PiggyBankCoinCount == GameState.PiggyBankCapacity)
                {
                    CrackPiggyBank();
                }
                    else
                    {
                        switch(GameState.OpponentsActionsThisTurn[0])
                        {
                            case OpponentAction.InsertCoin:
                                OpponentDropCoinAction();
                                break;

                            case OpponentAction.Shake:
                                OpponentShakeAction();
                                break;

                            case OpponentAction.EndTurn:
                                OpponentEndTurnAction();
                                break;
                        }
                        GameState.OpponentsActionsThisTurn.RemoveAt(0);
                    }
                
        }
        public void StartGameButtonPressed()
        {
            GameState.IsGameStarted = true;
            PrepareNewRound();
            DisableControls();
            IntroductionDialog();
        }
        public void PrepareNewRound()
        {
            GameState.RoundCount++;
            GameState.HasOpponentShakenThisRound = false;
            GameState.HasPlayerShakenThisRound = false;
            GameState.IsPlayersTurn = true;
            GameState.IsOpponentsTurn = false;
            GameState.PlayerPlayedCoinsCount = 0;
            GameState.OpponentPlayedCoinsCount = 0;
            GameState.PiggyBankCoinCount = 0; //should happen before generating new capacity
            GenerateCapacityAndRange();
        }
        public void NewTurn() 
        {
            //In this condition, we are moving from the players turn to the OPPONENT'S TURN
            if(GameState.IsPlayersTurn)
            {
                GameState.IsOpponentsTurn = true;
                GameState.IsPlayersTurn = false;
                GameState.OpponentPlayedCoinsCount = 0;
                GameState.HasPlayerActedThisTurn = true;
                DisableControls();
                RunOpponentTurn();
            }
            //In this condition, it is either the opponent's turn or nobody's turn so we are moving the PLAYER'S TURN
            else
            {
                GameState.IsOpponentsTurn = false;
                GameState.IsPlayersTurn = true;
                GameState.PlayerPlayedCoinsCount = 0;
                GameState.HasPlayerActedThisTurn = false;

                EnableControls();
            }
        }
        public void RunOpponentTurn()
        {
            Console.WriteLine("Opponent turn executing");
            //Calculate Opponent's win chance this turn
            int opponentWinChance = CalculateWinChance(out List<OpponentAction> offensiveActions);

            //Calculate Opponent's loss chance this turn
            int opponentLossChance = CalculateLossChance(out List<OpponentAction> defensiveActions);

            //If the win chance is higher or equal
                //perform offensive strategy
            //else
                //perform defensive strategy
            if (opponentWinChance >= opponentLossChance)
            {
                GameState.OpponentsActionsThisTurn = offensiveActions;
            }
            else
            {
                GameState.OpponentsActionsThisTurn = defensiveActions;
            }
            PerformNextOpponentAction();
        }
        public int CalculateWinChance(out List<OpponentAction> offensiveActions)
        {

            offensiveActions = new List<OpponentAction>{OpponentAction.InsertCoin,OpponentAction.InsertCoin,OpponentAction.InsertCoin,OpponentAction.EndTurn};
            int projectedCoinsInBank = GameState.PiggyBankCoinCount;
            //work out the difference in the piggy bank capacity's range
            int capDiff = GameState.PiggyBankHigherLimit - GameState.PiggyBankCoinCount;
            //divide 100 by the cap diff +1. this will be the amount that the chance of winning will increase by each time you put in a coin to 
            //bring the current amount of coins in the piggy bank to a value withint the range
            int chanceIncrement = 100 / Math.Max(capDiff + 1, 1);
            int opponentWinChance = 0;
            //opponent has 3 coins to use. so do a for loop with i = 3
            for(int i = 0; i < 3 ; i++)
            {
                //add a coin
                projectedCoinsInBank++;
                //if the current coin count is within the range, increment the coin count
                if(projectedCoinsInBank >= GameState.PiggyBankLowerLimit && projectedCoinsInBank <= GameState.PiggyBankHigherLimit)
                {
                    opponentWinChance += chanceIncrement;
                }
            }

            return opponentWinChance;
        }
        public int CalculateLossChance(out List<OpponentAction> defensiveActions)
        {
            defensiveActions = new();
            int opponentLossChance = 100; // Start with the maximum possible chance
            List<OpponentAction> bestDefensiveActions = new();

            // Simulate all valid combinations of defensive actions
            // 2. Insert 0 to 3 coins, then end the turn
            for (int coinsToInsert = 0; coinsToInsert <= 3; coinsToInsert++)
            {
                List<OpponentAction> insertActions = new List<OpponentAction>();
                int simulatedBankCount = GameState.PiggyBankCoinCount;

                for (int i = 0; i < coinsToInsert; i++)
                {
                    insertActions.Add(OpponentAction.InsertCoin);
                    simulatedBankCount++;
                }

                insertActions.Add(OpponentAction.EndTurn);

                int playerWinChance = SimulatePlayerWinChance(simulatedBankCount, !GameState.HasPlayerShakenThisRound);
                if (playerWinChance < opponentLossChance)
                {
                    opponentLossChance = playerWinChance;
                    bestDefensiveActions = insertActions;
                }
            }

            // 1. Shake and end the turn (if shaking is allowed) This should be done as a last resort
            if (!GameState.HasOpponentShakenThisRound)
            {
                List<OpponentAction> shakeActions = new List<OpponentAction>
                {
                    OpponentAction.Shake,
                    OpponentAction.EndTurn
                };

                int playerWinChance = SimulatePlayerWinChance(GameState.PiggyBankCoinCount, ! GameState.HasPlayerShakenThisRound);
                if (playerWinChance < opponentLossChance)
                {
                    opponentLossChance = playerWinChance;
                    bestDefensiveActions = shakeActions;
                }
            }
            
            //DEBUG: Show current opponent's actions before going random
            foreach(var action in bestDefensiveActions)
            {
                Console.WriteLine(action.ToString());
            }


            // Assign the best defensive actions
            if (bestDefensiveActions != null)
                defensiveActions = bestDefensiveActions;

            if (defensiveActions.Count() == 1 && defensiveActions.Contains(OpponentAction.EndTurn))
                {
                    defensiveActions = new();
                    // Random number generator
                    Random random = new Random();

                    for(int i=0; i< random.Next(1, 4); i++)
                    {
                        defensiveActions.Add(OpponentAction.InsertCoin);
                    }
                    defensiveActions.Add(OpponentAction.EndTurn);
                }

            return opponentLossChance;
        }
        public int SimulatePlayerWinChance(int simulatedBankCount, bool playerCanShake)
    {
        int playerWinChance = 0;

        // Calculate the player's chances of winning based on the simulated state
        int capDiff = GameState.PiggyBankHigherLimit - simulatedBankCount;
        int chanceIncrement = 100 / Math.Max(capDiff + 1, 1);


        // Player can use up to 3 coins
        for (int i = 0; i < 3; i++)
        {
            simulatedBankCount++;
            if (simulatedBankCount >= GameState.PiggyBankLowerLimit && simulatedBankCount <= GameState.PiggyBankHigherLimit)
            {
                playerWinChance += chanceIncrement;
            }
        }

        // Adjust win chance based on whether the player can shake
        if (playerCanShake)
        {
            playerWinChance = Math.Min(playerWinChance + 10, 100); // Arbitrary bonus for shaking
        }

        return playerWinChance;
    }
        public void OpponentShakeAction()
        {
            GameState.HasOpponentActedThisTurn = true;
            GameState.HasOpponentShakenThisRound = true;
            //add equalizer to the opponent's preceived limit once expanding coins are added
            GameState.DialogList.Add(("The opponent shook the swine","And now knows how many coins it holds"));
        }
        public void OpponentDropCoinAction()
        {
            GameState.OpponentPlayedCoinsCount++;
            GameState.PiggyBankCoinCount++;
            GameState.HasOpponentActedThisTurn = true;
            GameState.DialogList.Add(("The opponent inserts a coin into the swine",$"They have inserted {(GameState.OpponentPlayedCoinsCount + GameState.OpponentPlayedSpecialCoinsCount)} coins this turn"));
        }
        public void OpponentEndTurnAction() 
        { 
            NewTurn();
            GameState.Dialog = ("The opponent ends their turn","Please select an action");
        }
        public void PlayerDropCoinAction() 
        { 
            //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
            if(!(GameState.IsPlayerCoinDisabled || GameState.IsPlayerControlsDisabled || (GameState.PlayerPlayedCoinsCount == GameStateDTO.MAX_COINS_PLAYABLE)))
            {
            //put the coin in
                if(GameState.IsPlayersTurn)
                {
                    GameState.PlayerPlayedCoinsCount++;
                    GameState.PiggyBankCoinCount++;
                    GameState.HasPlayerActedThisTurn = true;
                }
                //if it has hit the limit. crack the piggy bank
                if(GameState.PiggyBankCoinCount == GameState.PiggyBankCapacity)
                {
                    CrackPiggyBank();
                }
                else
                {
                    GameState.Dialog = (
                        "You insert a coin into the swine", 
                        $"You have inserted {(GameState.PlayerPlayedCoinsCount + GameState.PlayerPlayedSpecialCoinsCount)} coins this turn" 
                    );
                }
            }    
        }
        public void CrackPiggyBank()
        {

            DisableControls();
            if(GameState.IsPlayersTurn)
            {
                GameState.IsPlayersTurn = false;
                GameState.IsOpponentsTurn = false;
                if(GameState.IsRegularPiggyBank)
                {
                    GameState.DialogList.Add(("You have filled the swine",$"It is smashed on your head and all {(GameState.PiggyBankCoinCount)} coins pour out. You collect them"));
                    GameState.PlayerPocketMoneyCount += GameState.PiggyBankCoinCount;
                    GameState.PiggyBankCoinCount = 0;
                    
                    StartShop();
                }
                else
                {
                    GameState.PlayerHealthCount--;
                    if(GameState.PlayerHealthCount <= 0)
                    {
                        StartDefeat();
                    }
                    else
                    {
                        StartShop();
                    }
                    
                }
                
                    
            }

            if(GameState.IsOpponentsTurn)
            {
                GameState.IsPlayersTurn = false;
                GameState.IsOpponentsTurn = false;
                if(GameState.IsRegularPiggyBank)
                {
                    GameState.DialogList.Add(("The opponent has filled the swine",$"It is smashed on their head and all {(GameState.PiggyBankCoinCount)} coins pour out. They collect them"));
                    GameState.OpponentPocketMoneyCount += GameState.PiggyBankCoinCount;
                    GameState.PiggyBankCoinCount = 0;
                    StartShop();
                }
                else
                {
                    GameState.DialogList.Add(("The opponent has filled the swine",$"It is smashed on their head. You hear the sound of metal colliding with bone. The swine doesn't crack"));
                    GameState.OpponentHealthCount--;
                    if(GameState.OpponentHealthCount <= 0)
                    {
                        StartVictory();
                    }
                    else
                    {
                        StartShop();
                    }
                } 
            }     
        }
        public void StartDefeat()
        {
            //Display game over, the amount of games the player has won, and disable all controls
            GameState.Dialog = (
                        "The spiked piggy bank strikes one last time, its unyielding surface crushing hope along with bone", 
                        "As you draw your final breath, the coins within remain untouched, forever out of reach. GAME OVER" 
                    );
            GameState.IsPlayerControlsDisabled = true;
        }
        public void StartVictory()
        {
            
        }
        public void StartShop()
        {
            //add shop logic. for now, a new round starts
            GameState.Dialog = ("A new round has begun", "Select an action");
            GameState.IsNewRoundStarting = true;
            PrepareNewRound();
            
        }
        public void PlayerShakeAction() 
        {
            //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
            if(!(GameState.IsPlayerShakeDisabled || GameState.HasPlayerShakenThisRound || GameState.IsPlayerControlsDisabled)){
                
                GameState.HasPlayerShakenThisRound = true;
                GameState.HasPlayerActedThisTurn = true;
                GameState.Dialog = (
                "You Shake the swine", 
                $"Hmm... Sounds like it has {GameState.PiggyBankCoinCount} coins in it" 
                );
            }
        }
        public void SpecialAction() 
        { 
            //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
            if(!(GameState.IsPlayerSpecialDisabled || !GameState.PlayerSpecialCoins.Any() || GameState.IsPlayerControlsDisabled))
            {
                
            }
            
        }
        public void EndTurnAction() 
        { 
            //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
            if(!(GameState.IsPlayerEndTurnDisabled || !GameState.HasPlayerActedThisTurn || GameState.IsPlayerControlsDisabled ))
            {
                NewTurn();
            }
            
        }

    }
}