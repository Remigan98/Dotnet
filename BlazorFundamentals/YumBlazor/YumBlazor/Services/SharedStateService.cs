namespace YumBlazor.Services
{
    public class SharedStateService
    {
        public event Action OnChange;

        int totalCartCount;

        public int TotalCartCount
        {
            get => totalCartCount;
            set
            {
                totalCartCount = value;
                NotifyStateChanged();
            }
        }

        void NotifyStateChanged() => OnChange?.Invoke();
    }
}
