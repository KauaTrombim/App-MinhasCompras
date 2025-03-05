using MinhasCompras.Models;
using SQLite;

namespace MinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _connect;
        public SQLiteDatabaseHelper(string path) 
        {
            _connect = new SQLiteAsyncConnection(path);
            _connect.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p) 
        {
            return _connect.InsertAsync(p);
        }
        public Task<List <Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade =?, Preco =? WHERE Id =?";
            return _connect.QueryAsync<Produto>(
                   sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }
        public Task<int> Delete(int id) 
        {
            return _connect.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll() 
        {
            return _connect.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * Produto WHERE descricao LIKE '%" + q + "%'";
            return _connect.QueryAsync<Produto>(sql);
        }
    }
}
