using Microsoft.Extensions.DependencyInjection;
using UniversalConverter.Client.ViewModels;

namespace UniversalConverter.Client.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage() : this(IPlatformApplication.Current?.Services.GetService<LoginViewModel>()
            ?? new LoginViewModel(new Services.RestConversionService(new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build())))
        {
        }

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}