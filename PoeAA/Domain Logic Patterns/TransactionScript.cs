using System;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace PoeAA.Domain_Logic_Patterns
{
    public class TransactionScript<T>
    {
        private ICommand<T> _command;

        public void SetCommand(ICommand<T> command)
        {
            _command = command;
        }

        public void ExecuteCommand(T model)
        {
            _command.Execute(model);
        }
    }

    public interface ICommand<in T>
    {
        void Execute(T model);
    }

    public class InsertCommand : ICommand<TransactionScriptModel>
    {
        private readonly string _connectionString;

        public InsertCommand(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Execute(TransactionScriptModel model)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                db.Open();
                using (IDbTransaction transaction = db.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        db.CreateCommand().Transaction = transaction;
                        var sqlQuery = "INSERT INTO TransactionScript (Id,Status) VALUES(@Id, @Status)";
                        db.Execute(sqlQuery, model, transaction);
                        DoStuff(model);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void DoStuff(TransactionScriptModel model)
        {
            if (model == null || model.Status == "Fake")
            {
                throw new Exception("Bad data");
            }
        }
    }

    public class TransactionScriptModel
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
