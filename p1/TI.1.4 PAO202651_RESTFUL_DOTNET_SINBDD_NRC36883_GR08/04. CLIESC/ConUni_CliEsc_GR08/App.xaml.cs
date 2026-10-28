using System;
using System.Windows;
using ConUni_CliEsc_GR08.Services;

namespace ConUni_CliEsc_GR08;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        if (e.Args.Length > 0 && !string.IsNullOrWhiteSpace(e.Args[0]))
        {
            RestClient.SetServerHost(e.Args[0]);
        }
        else
        {
            string? envHost = Environment.GetEnvironmentVariable("REST_HOST");
            if (!string.IsNullOrWhiteSpace(envHost))
            {
                RestClient.SetServerHost(envHost);
            }
        }
    }
}
