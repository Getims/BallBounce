using Main.Scripts.Data.Core;

namespace Main.Scripts.Data.Services
{
    public class DataService
    {
        private readonly IDatabase _database;

        protected DataService(IDatabase database) =>
            _database = database;

        protected void TryToSave(bool autoSave)
        {
            if (!autoSave)
                return;

            _database.SaveData();
        }
    }
}