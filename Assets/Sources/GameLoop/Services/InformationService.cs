using Sources.Architecture.Interfaces;
using Sources.Presenters.HelperViews;

namespace Sources.GameLoop.Services
{
    public class InformationService: IInformationService
    {
        private readonly InformationWindow _window;

        public InformationService(InformationWindow window)
        {
            _window = window;
        }
        public void ShowInfo(IVisualData data)
        {
            _window.gameObject.SetActive(true);
            _window.Show(data);
        }
    }
}