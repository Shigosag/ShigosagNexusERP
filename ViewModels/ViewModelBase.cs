using CommunityToolkit.Mvvm.ComponentModel;

namespace ShigosagNexusERP.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _statusMessage = string.Empty;
}
