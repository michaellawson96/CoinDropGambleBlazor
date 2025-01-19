# Coin Drop Gamble

Coin Drop Gamble is a turn-based strategy game where players compete to outsmart their opponent and win the contents of a piggy bank by smashing it on themselves. The game is designed as a playable MVP (Minimum Viable Product) and will be refined over time.

## How It Works

### Overview
- **Players**: 1 Player vs. 1 Computer Opponent
- **Rounds**: The game is played in rounds, with the player always taking the first turn.
- **Objective**: Fill the piggy bank on your turn, causing it to smash on your head, and collect its contents.

### Game Rules
1. A piggy bank is shared between the player and the opponent each round.
2. The piggy bank has a **random maximum capacity** (between 3 and 20). Players cannot see this capacity but are given a range that contains it (e.g., "BETWEEN 3 AND 4").
3. During their turn, players can choose one of three actions:
   - **Drop Coin**: Insert 1 to 3 coins into the piggy bank.
     - If the number of coins in the piggy bank meets or exceeds its maximum capacity, the piggy bank smashes on the current player's head.
     - The current player collects all the coins.
   - **Shake Piggy Bank**: Reveal the exact number of coins in the piggy bank. 
     - Each player can only shake once per round.
     - Shaking can be a stall tactic to avoid dropping coins when it’s disadvantageous.
   - **End Turn**: Pass the turn to the opponent.
     - The player must perform at least one action (Drop Coin or Shake) before ending their turn.
4. The game continues until the piggy bank is smashed, and the contents are awarded to the player who smashed it.

### Key Limitations in the MVP
- The current amount of coins in the piggy bank is not visible, requiring players to rely on memory and logic.
- The range of possible piggy bank capacities is always 1 number apart.
- The opponent AI may not always make optimal moves.

## MVP Goals
The following tasks need to be completed before the MVP is considered acceptable:

### 1. File Structure
- Refactor the project into a proper file structure to separate concerns:
  - Components
  - Services
  - Models
  - Helpers
  - Tests

### 2. Code Cleanup
- Refactor and organize the code to improve readability and maintainability.
- Extract reusable logic into services or helper classes.

### 3. Unit Testing
- Add unit tests to cover:
  - Core game logic (e.g., win/loss conditions, piggy bank behavior).
  - Opponent AI decision-making.
- Validate edge cases (e.g., maximum capacity, early smashing).

### 4. Opponent AI Improvements
- Fix the opponent's logic to:
  - Utilize the Shake action when tactically beneficial.
  - Avoid handing the player easy wins.

### 5. Mobile-Friendly Design
- Adjust CSS to ensure the game is fully playable on mobile devices in both portrait and landscape orientations.
- Ensure all text and buttons are easily readable and tappable on smaller screens.

## Future Considerations
While not part of the MVP, future updates may include:
- Adding special coins with unique effects.
- Increasing the range of possible capacity values.
- Enhancing the opponent AI for more challenging gameplay.

## How to play (Online)
I host the game on coindropgamble.azurewebsites.net
Note: This will not always be the most up to date version and it may go down sometimes.

## How to Play (Locally)
1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```bash
   cd coin-drop-gamble
   ```
3. Run the application using your preferred development server (e.g., Blazor or .NET).
4. Play the game through your web browser.

## Contributing
Contributions are welcome! If you find a bug or have an idea for improvement, feel free to open an issue or submit a pull request.

---

Enjoy playing Coin Drop Gamble and may the odds be in your favor!
