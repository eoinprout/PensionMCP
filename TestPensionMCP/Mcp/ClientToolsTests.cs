using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PensionMCP.Data;
using PensionMCP.Mcp;
using System.Text.Json;

namespace TestPensionMCP.Mcp
{
    public class ClientToolsTests
    {
        private SqliteConnection _connection;
        private PensionDbContext _context;

        [SetUp]
        public async Task SetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            await _connection.OpenAsync();

            var options = new DbContextOptionsBuilder<PensionDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new PensionDbContext(options);
            await _context.Database.EnsureCreatedAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
            _connection.Dispose();
        }

        [Test]
        public async Task GetAllClients_ReturnsThreeClients()
        {
            var tool = new ClientTools(_context);
            var json = await tool.GetAllClients();

            var clients = JsonSerializer.Deserialize<List<object>>(json);
            Assert.That(clients, Has.Count.EqualTo(3));
        }
    }
}
