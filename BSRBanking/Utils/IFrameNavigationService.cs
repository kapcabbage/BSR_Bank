using GalaSoft.MvvmLight.Views;


namespace BSRBanking.Utils
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
