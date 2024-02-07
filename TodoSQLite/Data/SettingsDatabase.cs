using Microsoft.VisualBasic;
using SQLite;
using TodoSQLite.Models;

namespace TodoSQLite.Data;

public class SettingsDatabase
{
    SQLiteAsyncConnection Database;

    public SettingsDatabase()
    {
    }

    public static Settings getSettings() 
    {

        //ToDo:Klappt noch nicht muss ich mal ausprobieren.
        SettingsDatabase settingsDatabase = new SettingsDatabase();
        var test = settingsDatabase.GetSettingAsync();
        Settings settings = new Settings();
        Task.Run(async () =>
        {
            settings = await settingsDatabase.GetSettingAsync();

        });
   
            
        if (settings == null)
        {
            settings = new Settings();
        }

        return settings; 
    }
    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await Database.CreateTableAsync<Settings>();
    }
    public async Task<Settings> GetSettingAsync()
    {
        await Init();
        return await Database.Table<Settings>().Where(i => i.ID != 0).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(Settings item)
    {
        await Init();
        
        if (item.ID != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }
}

