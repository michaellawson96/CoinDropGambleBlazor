using Microsoft.AspNetCore.Components;

namespace CoinDropGamble.Components.Pages
{
    public partial class IntroCard : ComponentBase
    {
        [Parameter]
        public EventCallback OnStartGame { get; set; }
    }
}