using Microsoft.Extensions.DependencyInjection;
using UniversalConverter.Client.ViewModels;

namespace UniversalConverter.Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage() : this(IPlatformApplication.Current?.Services.GetService<MainViewModel>()
            ?? new MainViewModel(new Services.RestConversionService(new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build())))
        {
        }

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}