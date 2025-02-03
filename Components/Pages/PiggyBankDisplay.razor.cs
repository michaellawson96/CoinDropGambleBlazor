using Microsoft.AspNetCore.Components;

namespace CoinDropGamble.Components.Pages
{
    public partial class PiggyBankDisplay : ComponentBase
    {
        private const string COFFIN_IMAGE_PATH = "images/coffin.jpeg";
        private const string COIN_BRIEFCASE_IMAGE_PATH = "images/coin-briefcase.jpeg";
        private const string PIGGY_BANK_IMAGE_PATH = "images/piggy-bank-towards-viewer.jpeg";
        private const string SPIKED_PIGGY_BANK_IMAGE_PATH = "images/spiked-piggy-bank-towards-viewer.jpeg";
        private const string BLOODED_SPIKED_PIGGY_BANK_IMAGE_PATH = "images/smash-spiked-piggy-bank-on-head.jpeg";
        private const string SMASHED_PIGGY_BANK_IMAGE_PATH = "images/smash-piggy-bank-on-head.jpeg";
        [Parameter]
        public int GamesWon { get; set; }
        private int _piggyBankCapacity;
        private int _higherDisparity;
        private int _lowerDisparity;
        private string _imagePath = PIGGY_BANK_IMAGE_PATH;
        public int GetPiggyBankLowerLimit() => _piggyBankCapacity - _lowerDisparity;
        public int GetPiggyBankHigherLimit() => _piggyBankCapacity - _higherDisparity;
        public int GetPiggyBankCapacity() => _piggyBankCapacity;
        public int GetHigherDisparity() => _higherDisparity;
        public int GetLowerDisparity() => _lowerDisparity;


        public void GenerateCapacityAndRange()
        {
            // Random number generator
            Random random = new Random();

            // Generate a random capacity between 3 and 20
            _piggyBankCapacity = random.Next(3, 21);

            // Determine the increment range based on games won
            int incrementMax;
            if (GamesWon <= 10)
                incrementMax = 1;
            else if (GamesWon <= 20)
                incrementMax = random.Next(1, 3); // Random between 1 and 2
            else if (GamesWon <= 30)
                incrementMax = random.Next(1, 4); // Random between 1 and 3
            else if (GamesWon <= 40)
                incrementMax = random.Next(1, 5); // Random between 1 and 4
            else
                incrementMax = random.Next(1, 6); // Random between 1 and 5

            // Reset disparities to 0
            _higherDisparity = 0;
            _lowerDisparity = 0;

            // Apply increments to disparities
            for (int i = 0; i < incrementMax; i++)
            {
                bool incrementHigherDisparity = true;

                // Boundary checks
                if (_piggyBankCapacity + _higherDisparity == 20)
                    incrementHigherDisparity = false; // Force increment to lower disparity
                else if (_piggyBankCapacity - _lowerDisparity == 3)
                    incrementHigherDisparity = true; // Force increment to higher disparity
                else
                    incrementHigherDisparity = random.Next(0, 2) == 0; // 50/50 chance otherwise

                if (incrementHigherDisparity)
                    _higherDisparity++;
                else
                    _lowerDisparity++;
            }
            InvokeAsync(StateHasChanged);
        }
    }
}