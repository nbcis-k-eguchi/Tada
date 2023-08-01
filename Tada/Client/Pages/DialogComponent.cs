using Microsoft.AspNetCore.Components;

namespace Tada.Client.Pages
{
    /// <summary>
    /// ダイアログ表示と終了の共通処理
    /// TODO : ダイアログコンポーネントとして共通化する。個別にbodyを実装する
    /// </summary>
    public abstract class DialogComponent : ComponentBase
    {
        [Parameter]
        public string DisplayStyle { get; set; } = "none;";

        [Parameter]
        public string ShowClass { get; set; } = "";

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public virtual async Task ShowDialog()
        {
            DisplayStyle = "block";
            await Task.Delay(50);
            ShowClass = "show";
            StateHasChanged();
        }

        public virtual async Task HideDialog()
        {
            ShowClass = "";
            await Task.Delay(200);
            DisplayStyle = "none";
            StateHasChanged();
        }
    }
}
