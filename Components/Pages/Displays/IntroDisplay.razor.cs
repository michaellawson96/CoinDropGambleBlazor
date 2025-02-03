using Microsoft.AspNetCore.Components;

namespace CoinDropGamble.Components.Pages.Displays
{
    public partial class IntroDisplay : ComponentBase
    {
        [Parameter]
        public EventCallback OnStartGame { get; set; }
    }
}