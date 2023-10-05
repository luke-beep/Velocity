using Microsoft.UI.Xaml.Controls;

namespace Velocity.Contracts.Services;

public interface IIntroductionService
{
    void CloseTip(TeachingTip sender, object args);
    void CloseTipNoNav(TeachingTip sender, object args);
    void HandleTipBackButtonClick(TeachingTip sender, object args);
    void HandleTipBackButtonClickNoNav(TeachingTip sender, object args);
    void HandleTipActionButtonClick(TeachingTip sender, object args);
    void HandleTipActionButtonClickNoNav(TeachingTip sender, object args);
    void ShowTipForPage(string? page);
    void Initialize(Dictionary<string, TeachingTip> tipsDictionary);
}