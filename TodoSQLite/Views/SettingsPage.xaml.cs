using System.Collections.ObjectModel;
using TodoSQLite.Models;
using TodoSQLite.Data;

namespace TodoSQLite;

public partial class SettingsPage : ContentPage
{
    public ObservableCollection<Settings> Items { get; set; } = new();
    SettingsDatabase itemDatabase;
    Settings varSettings { get; set; } = new();
    public SettingsPage()
    {
        InitializeComponent();
        itemDatabase = new SettingsDatabase();
        BindingContext = this;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        varSettings = await itemDatabase.GetSettingAsync();
        if (varSettings == null)
        {
            varSettings = new Settings();
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            entry.Text = varSettings.Token;
        });
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        varSettings.Token = entry.Text;
        itemDatabase.SaveItemAsync(varSettings);
    }
}