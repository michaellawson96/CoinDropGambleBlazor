using CoinDropGamble.Models.Enums;
using Microsoft.AspNetCore.Components;

namespace CoinDropGamble.Components.Pages
{
    public partial class Gameboard : ComponentBase
    {
    private PiggyBankDisplay? _piggyBankRef;
    private const int MAX_COINS_PLAYABLE = 3;
    private bool _isGameStarted = false;
    private bool _isRegularPiggyBank = true;
    private bool _hasPlayerShakenThisRound;
    private bool _hasOpponentShakenThisRound;
    private bool _hasPlayerActedThisTurn;
    private bool _hasOpponentActedThisTurn;
    private bool _hasOpponentRunOutOfGoodOptions;
    private bool _isPlayersTurn =true;
    private bool _isOpponentsTurn=false;
    private bool _isPlayerCoinDisabled;
    private bool _isOpponentCoinDisabled;
    private bool _isPlayerShakeDisabled;
    private bool _isOpponentShakeDisabled;
    private bool _isPlayerSpecialDisabled;
    private bool _isOpponentSpecialDisabled;
    private bool _isPlayerEndTurnDisabled;
    private bool _isOpponentEndTurnDisabled;
    private bool _isPlayerControlsDisabled;
    private int _piggyBankCoinCount = 0;
    private int _playerHealthCount = 3;
    private int _opponentHealthCount = 3;
    private int _currentGameMaxHealth = 3;
    private int _playerPocketMoneyCount = 0;
    private int _opponentPocketMoneyCount;
    private int _playerPlayedCoinsCount;
    private int _opponentPlayedCoinsCount;
    private int _playerPlayedSpecialCoinsCount;
    private int _opponentPlayedSpecialCoinsCount;
    private int _currentDialogIndex;
    private int _gamesWon = 0;
    private bool _isNewRoundStarting = false;
    private int opponentPiggyBankCapacityGuessLow;
    private int opponentPiggyBankCapacityGuessHigh;
    private int _roundCount = 0;
    private List<(string line1, string line2)> _dialogList = new();
    private (string line1, string line2) _dialog = (
        "It's your turn. Insert up to 3 coins or shake the swine", 
        "Once you have done all you desire, you may end your turn" 
        );
    private List<SpecialCoins> _playerSpecialCoins = new();
    private List<SpecialCoins> _opponentSpecialCoins = new();
    private List<SpecialCoins> _specialCoinShopItems = new();
    private List<OpponentAction> _opponentsActionsThisTurn = new();
    
    public void StartGameButtonPressed()
    {
        _isGameStarted = true;
        PrepareNewRound();
        DisableControls();
        IntroductionDialog();
        InvokeAsync(StateHasChanged);
    }

    public void PrepareNewRound()
    {
        _roundCount++;
        _hasOpponentShakenThisRound = false;
        _hasPlayerShakenThisRound = false;
        _isPlayersTurn = true;
        _isOpponentsTurn = false;
        _playerPlayedCoinsCount = 0;
        _opponentPlayedCoinsCount = 0;
        _piggyBankCoinCount = 0; //should happen before generating new capacity
        _piggyBankRef.GenerateCapacityAndRange();
        InvokeAsync(StateHasChanged);
    }

    public void IntroductionDialog()
    {
        _dialogList.Add(("Greetings!","And welcome to the game"));
        _dialogList.Add(("Survive!","For as long as you can"));
        _dialogList.Add(("Above the swine, a range","The swine's limit is within that range"));
        _dialogList.Add(("Fill the swine","And its fortune shall be yours"));
        _dialogList.Add(("You may shake the swine once","To discover its contents"));
        _dialogList.Add(("Shake, or provide","You must choose one"));
    }

    public void ProgressDialogStream(){
        _dialogList.RemoveAt(0);
        if(_isOpponentsTurn)
        {
            performNextOpponentAction();
        }
        else if(_isPlayersTurn && !_dialogList.Any())
        {
            EnableControls();
        }
        else if(!_isOpponentsTurn && !_isPlayersTurn && _isNewRoundStarting)
        {
            EnableControls();
            _isPlayersTurn = true;
        }
    }



    


    private void DisableControls() => _isPlayerControlsDisabled = true;
    private void EnableControls() => _isPlayerControlsDisabled = false;

    private void NewTurn() 
    {
        //In this condition, we are moving from the players turn to the OPPONENT'S TURN
        if(_isPlayersTurn)
        {
            _isOpponentsTurn = true;
            _isPlayersTurn = false;
            _opponentPlayedCoinsCount = 0;
            _hasPlayerActedThisTurn = true;
            DisableControls();
            RunOpponentTurn();
        }
        //In this condition, it is either the opponent's turn or nobody's turn so we are moving the PLAYER'S TURN
        else
        {
            _isOpponentsTurn = false;
            _isPlayersTurn = true;
            _playerPlayedCoinsCount = 0;
            _hasPlayerActedThisTurn = false;

            EnableControls();
        }
    }

private void RunOpponentTurn()
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
        _opponentsActionsThisTurn = offensiveActions;
    }
    else
    {
        _opponentsActionsThisTurn = defensiveActions;
    }
    performNextOpponentAction();
}

private int CalculateWinChance(out List<OpponentAction> offensiveActions)
{

    offensiveActions = new List<OpponentAction>{OpponentAction.InsertCoin,OpponentAction.InsertCoin,OpponentAction.InsertCoin,OpponentAction.EndTurn};
    int projectedCoinsInBank = _piggyBankCoinCount;
    //work out the difference in the piggy bank capacity's range
    int capDiff = (_piggyBankRef.GetPiggyBankCapacity()+_piggyBankRef.GetHigherDisparity())-_piggyBankCoinCount;
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
        if(projectedCoinsInBank >= (_piggyBankRef.GetPiggyBankCapacity()-_piggyBankRef.GetLowerDisparity()) && projectedCoinsInBank <= (_piggyBankRef.GetPiggyBankCapacity()+_piggyBankRef.GetHigherDisparity()))
        {
            opponentWinChance += chanceIncrement;
        }
    }

    return opponentWinChance;
}

private int CalculateLossChance(out List<OpponentAction> defensiveActions)
{
    defensiveActions = new();
    int opponentLossChance = 100; // Start with the maximum possible chance
    List<OpponentAction> bestDefensiveActions = new();

    // Simulate all valid combinations of defensive actions
    // 2. Insert 0 to 3 coins, then end the turn
    for (int coinsToInsert = 0; coinsToInsert <= 3; coinsToInsert++)
    {
        List<OpponentAction> insertActions = new List<OpponentAction>();
        int simulatedBankCount = _piggyBankCoinCount;

        for (int i = 0; i < coinsToInsert; i++)
        {
            insertActions.Add(OpponentAction.InsertCoin);
            simulatedBankCount++;
        }

        insertActions.Add(OpponentAction.EndTurn);

        int playerWinChance = SimulatePlayerWinChance(simulatedBankCount, !_hasPlayerShakenThisRound);
        if (playerWinChance < opponentLossChance)
        {
            opponentLossChance = playerWinChance;
            bestDefensiveActions = insertActions;
        }
    }

    // 1. Shake and end the turn (if shaking is allowed) This should be done as a last resort
    if (!_hasOpponentShakenThisRound)
    {
        List<OpponentAction> shakeActions = new List<OpponentAction>
        {
            OpponentAction.Shake,
            OpponentAction.EndTurn
        };

        int playerWinChance = SimulatePlayerWinChance(_piggyBankCoinCount, !_hasPlayerShakenThisRound);
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


private int SimulatePlayerWinChance(int simulatedBankCount, bool playerCanShake)
{
    int playerWinChance = 0;

    // Calculate the player's chances of winning based on the simulated state
    int capDiff = (_piggyBankRef.GetPiggyBankCapacity() + _piggyBankRef.GetHigherDisparity()) - simulatedBankCount;
    int chanceIncrement = 100 / Math.Max(capDiff + 1, 1);


    // Player can use up to 3 coins
    for (int i = 0; i < 3; i++)
    {
        simulatedBankCount++;
        if (simulatedBankCount >= (_piggyBankRef.GetPiggyBankCapacity() - _piggyBankRef.GetLowerDisparity()) && simulatedBankCount <= (_piggyBankRef.GetPiggyBankCapacity() + _piggyBankRef.GetHigherDisparity()))
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

private void performNextOpponentAction()
{
    if(_piggyBankCoinCount == _piggyBankRef.GetPiggyBankCapacity())
        {
            CrackPiggyBank();
        }
            else
            {
                switch(_opponentsActionsThisTurn[0])
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
                _opponentsActionsThisTurn.RemoveAt(0);
            }
        
}




    private void OpponentShakeAction()
    {
        _hasOpponentActedThisTurn = true;
        _hasOpponentShakenThisRound = true;
        //add equalizer to the opponent's preceived limit once expanding coins are added
        _dialogList.Add(("The opponent shook the swine","And now knows how many coins it holds"));
    }

    private void OpponentDropCoinAction()
    {
        _opponentPlayedCoinsCount++;
        _piggyBankCoinCount++;
        _hasOpponentActedThisTurn = true;
        _dialogList.Add(("The opponent inserts a coin into the swine",$"They have inserted {(_opponentPlayedCoinsCount + _opponentPlayedSpecialCoinsCount)} coins this turn"));
    }

    private void OpponentEndTurnAction() 
    { 
        NewTurn();
        _dialog = ("The opponent ends their turn","Please select an action");
    }



    private void PlayerDropCoinAction() 
    { 
        //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
        if(!(_isPlayerCoinDisabled || _isPlayerControlsDisabled || (_playerPlayedCoinsCount == MAX_COINS_PLAYABLE)))
        {
        //put the coin in
            if(_isPlayersTurn)
            {
                _playerPlayedCoinsCount++;
                _piggyBankCoinCount++;
                _hasPlayerActedThisTurn = true;
            }
            //if it has hit the limit. crack the piggy bank
            if(_piggyBankCoinCount == _piggyBankRef.GetPiggyBankCapacity())
            {
                CrackPiggyBank();
            }
            else
            {
                _dialog = (
                    "You insert a coin into the swine", 
                    $"You have inserted {(_playerPlayedCoinsCount + _playerPlayedSpecialCoinsCount)} coins this turn" 
                );
            }
        }    
    }

    public void CrackPiggyBank()
    {

        DisableControls();
        if(_isPlayersTurn)
        {
            _isPlayersTurn = false;
            _isOpponentsTurn = false;
            if(_isRegularPiggyBank)
            {
                _dialogList.Add(("You have filled the swine",$"It is smashed on your head and all {(_piggyBankCoinCount)} coins pour out. You collect them"));
                _playerPocketMoneyCount += _piggyBankCoinCount;
                _piggyBankCoinCount = 0;
                
                StartShop();
            }
            else
            {
                _playerHealthCount--;
                if(_playerHealthCount <= 0)
                {
                    StartDefeat();
                }
                else
                {
                    StartShop();
                }
                
            }
            
                
        }

        if(_isOpponentsTurn)
        {
            _isPlayersTurn = false;
            _isOpponentsTurn = false;
            if(_isRegularPiggyBank)
            {
                _dialogList.Add(("The opponent has filled the swine",$"It is smashed on their head and all {(_piggyBankCoinCount)} coins pour out. They collect them"));
                _opponentPocketMoneyCount += _piggyBankCoinCount;
                _piggyBankCoinCount = 0;
                StartShop();
            }
            else
            {
                _dialogList.Add(("The opponent has filled the swine",$"It is smashed on their head. You hear the sound of metal colliding with bone. The swine doesn't crack"));
                _opponentHealthCount--;
                if(_opponentHealthCount <= 0)
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
        _dialog = (
                    "The spiked piggy bank strikes one last time, its unyielding surface crushing hope along with bone", 
                    "As you draw your final breath, the coins within remain untouched, forever out of reach. GAME OVER" 
                );
        _isPlayerControlsDisabled = true;
    }

    public void StartVictory()
    {

    }

    public void StartShop()
    {
        //add shop logic. for now, a new round starts
        _dialog = ("A new round has begun", "Select an action");
        _isNewRoundStarting = true;
        PrepareNewRound();
        
    }

    private void PlayerShakeAction() 
    {
        //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
        if(!(_isPlayerShakeDisabled || _hasPlayerShakenThisRound || _isPlayerControlsDisabled)){
            
            _hasPlayerShakenThisRound = true;
            _hasPlayerActedThisTurn = true;
            _dialog = (
            "You Shake the swine", 
            $"Hmm... Sounds like it has {_piggyBankCoinCount} coins in it" 
            );
        }
    }
    private void SpecialAction() 
    { 
        //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
        if(!(_isPlayerSpecialDisabled || !_playerSpecialCoins.Any() || _isPlayerControlsDisabled))
        {
            
        }
        
    }
    private void EndTurnAction() 
    { 
        //Put the reverse of the disabled condition here too as crafty players might try to reenable the button using dev tools
        if(!(_isPlayerEndTurnDisabled || !_hasPlayerActedThisTurn || _isPlayerControlsDisabled ))
        {
            NewTurn();
        }
        
    }

    }
}