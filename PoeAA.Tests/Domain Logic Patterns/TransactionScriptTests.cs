using System;
using System.Collections.Generic;
using System.Text;
using PoeAA.Domain_Logic_Patterns;
using Xunit;

namespace PoeAA.Tests.Domain_Logic_Patterns
{
    public class TransactionScriptTests
    {
        private string connString =
            "Data Source=localhost;Initial Catalog=test;Integrated Security=True;MultipleActiveResultSets=True";
        [Fact]
        public void ExecuteInsertCommand_WithValidData_ShouldInsertEntityToDb()
        {
            var ts = new TransactionScript<TransactionScriptModel>();
            ts.SetCommand(new InsertCommand(connString));
            ts.ExecuteCommand(new TransactionScriptModel
            {
                Status = "Done",
                Id = Guid.NewGuid()
            });
        }
        [Fact]
        public void ExecuteInsertCommand_WithBadStatus_ShouldThrowException()
        {
            var ts = new TransactionScript<TransactionScriptModel>();
            ts.SetCommand(new InsertCommand(connString));
            Assert.Throws<Exception>(() => ts.ExecuteCommand(new TransactionScriptModel
            {
                Status = "Fake",
                Id = Guid.NewGuid()
            }));
        }
    }
}
