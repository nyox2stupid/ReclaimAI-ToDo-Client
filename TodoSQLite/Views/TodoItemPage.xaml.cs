using TodoSQLite.Data;
using TodoSQLite.Models;

namespace TodoSQLite.Views;

[QueryProperty("Item", "Item")]
public partial class TodoItemPage : ContentPage
{
    TodoItem item;
    public TodoItem Item
    {
        get => BindingContext as TodoItem;
        set => BindingContext = value;
    }
    TodoItemDatabase database;
    public TodoItemPage(TodoItemDatabase todoItemDatabase)
    {
        InitializeComponent();
        database = todoItemDatabase;
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Item.Title))
        {
            await DisplayAlert("Name Required", "Please enter a name for the todo item.", "OK");
            return;
        }
        await database.SaveItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }
    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (Item.TaskID == 0)
            return;
        await database.DeleteItemAsync(Item);
        await Shell.Current.GoToAsync("..");
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void TimePlus(object sender, EventArgs e)
    {
        // Split timepick text and remove h or m
        string[] splits = timepick.Text.Split(' ');
        splits[0] = splits[0].Remove(splits[0].Length - 1);
        splits[1] = splits[1].Remove(splits[1].Length - 1);

        // If minutes are 55, reset to 0 and increase hours by 1
        if (splits[1] == "55")
        {
            splits[1] = "0";
            splits[0] = "" + (int.Parse(splits[0]) + 1);
        }

        // Otherwise, increase minutes by 5
        else
        {
            splits[1] = "" + (int.Parse(splits[1]) + 5);
        }

        // Update timepick text with new values
        timepick.Text = splits[0] + "h " + splits[1] + "m";
    }
    private void TimeMinus(object sender, EventArgs e)
    {
        // If time is 0h 0m, do nothing
        if (timepick.Text == "0h 0m") return;

        // Split timepick text and remove h or m
        string[] splits = timepick.Text.Split(' ');
        splits[0] = splits[0].Remove(splits[0].Length - 1);
        splits[1] = splits[1].Remove(splits[1].Length - 1);

        // If minutes are 0, reset to 55 and decrease hours by 1
        if (splits[1] == "0")
        {
            splits[1] = "55";
            splits[0] = "" + (int.Parse(splits[0]) - 1);
        }

        // Otherwise, decrease minutes by 5
        else
        {
            splits[1] = "" + (int.Parse(splits[1]) - 5);
        }

        // Update timepick text with new values
        timepick.Text = splits[0] + "h " + splits[1] + "m";
    }
    private async void TimeCustom(object sender, EventArgs e)
    {
        // Split timepick text && remove h and m
        string[] splits = timepick.Text.Split(' ');
        splits[0] = splits[0].Remove(splits[0].Length - 1);
        splits[1] = splits[1].Remove(splits[1].Length - 1);

        // Prompt user for hours -> store in timeh
        string timeh = await DisplayPromptAsync("set time", "How many hours?", placeholder: splits[0], keyboard: Keyboard.Numeric);
        int hour;
        // Use previous value if input is empty or invalid
        if (timeh == "" || !int.TryParse(timeh, out hour)) timeh = splits[0];

        // Prompt user or minutes -> store in timem
        string timem = await DisplayPromptAsync("set time", "How many minutes?", placeholder: splits[1], keyboard: Keyboard.Numeric);
        int minute;
        // Use previous value if input is empty or invalid
        if (timem == "" || !int.TryParse(timem, out minute)) timem = splits[1];

        // Adjust hours and minutes if input is more than 59
        else if (int.Parse(timem) > 59)
        {
            timeh = "" + (int.Parse(timeh) + Math.Floor(double.Parse(timem) / 60));
            timem = "" + (int.Parse(timem) % 60);
        }

        // Round timem up to next 5 minute step
        if ((int.Parse(timem) % 5) > 0)
            timem = "" + (int.Parse(timem) + (5 - (int.Parse(timem) % 5)));

        // Update timepick text with new values
        timepick.Text = timeh + "h " + timem + "m";
    }
}