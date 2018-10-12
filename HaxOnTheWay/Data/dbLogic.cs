using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace HaxOnTheWay
{
    public class dbLogic
    {
        readonly SQLiteAsyncConnection database;

        public dbLogic(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            //Se crean las tablas
            database.CreateTableAsync<Models.Drivers>().Wait();
            database.CreateTableAsync<Models.Commands>().Wait();
            database.CreateTableAsync<Models.Tracings>().Wait();
            database.CreateTableAsync<Models.Estatus>().Wait();
            database.CreateTableAsync<Models.Coord>().Wait();
        }

        public void dropTables() {
            database.ExecuteAsync("DELETE FROM Drivers");
            database.ExecuteAsync("DELETE FROM Commands");
            database.ExecuteAsync("DELETE FROM Tracings");
            database.ExecuteAsync("DELETE FROM Estatus");
            database.ExecuteAsync("DELETE FROM Coord");
        }

        public void dropTablesCoord()
        {
            database.ExecuteAsync("DELETE FROM Coord");
        }

        public void dropTablesCommands()
        {
            database.ExecuteAsync("DELETE FROM Commands");
        }

        public Task<int> SaveDriverAsync(Models.Drivers item)
        {
            return database.InsertAsync(item);
        }

        public Task<List<Models.Drivers>> GetAllDriversAsync()
        {
            return database.Table<Models.Drivers>().ToListAsync();
        }

        public Task<List<Models.Estatus>> GetAllEstatusAsync()
        {
            return database.Table<Models.Estatus>().ToListAsync();
        }

        public Task<List<Models.Coord>> GetAllCoordAsync()
        {
            return database.Table<Models.Coord>().ToListAsync();
        }

        public Task<int> DeleteDriverAsync(Models.Drivers item)
        {
            return database.DeleteAsync(item);
        }

        public Task<List<Models.Commands>> GetAllCommandsAsync()
        {
            return database.Table<Models.Commands>().ToListAsync();
        }

        public Task<Models.Commands> GetCommandAsync(int id)
        {
            return database.Table<Models.Commands>().Where(i => i.iCommand == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveCommandAsync(Models.Commands item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> SaveEstatusAsync(Models.Estatus item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> SaveCoordAsync(Models.Coord item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> UpdateCommandAsync(Models.Commands item)
        {
            return database.UpdateAsync(item);   
        }

        public Task<int> DeleteCommandAsync(Models.Commands item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> SaveTracingAsync(Models.Tracings item)
        {
            return database.InsertAsync(item);
        }
    }
}
