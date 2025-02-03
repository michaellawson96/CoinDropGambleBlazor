using CoinDropGamble.Models.Enums;
using Microsoft.AspNetCore.Components;

namespace CoinDropGamble.Components.Pages
{
    public partial class Gameboard : ComponentBase
    {
        protected override void OnInitialized()
        {
            GameService.OnChange += HandleGameStateChanged;
        }

        private async void HandleGameStateChanged()
        {
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            GameService.OnChange -= HandleGameStateChanged;
        }
    }
}