using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ModelContextProtocol;
using PensionMCP.Common;
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
            // We are using an in memory DB for testing, one the nice features of SQLite.
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

        [TestCase("Kirk", 1)]
        [TestCase("KIRK", 1)]
        [TestCase("o", 2)]
        [TestCase("Khan", 0)]
        public async Task SearchClients_ReturnsExpectedCount(string searchTerm, int expectedCount)
        {
            var tool = new ClientTools(_context);

            var json = await tool.SearchClients(searchTerm);
            var clients = JsonSerializer.Deserialize<List<object>>(json);

            Assert.That(clients, Has.Count.EqualTo(expectedCount));
        }

        [Test]
        public async Task AddClient_ReturnsClientJson()
        {
            var tool = new ClientTools(_context);
            string name = "Montgomery Scott";
            string dob = "1970-05-06";

            var json = await tool.AddClient(name, dob);
            var client = JsonSerializer.Deserialize<JsonElement>(json);

            Assert.That(client.GetProperty("Name").GetString(), Is.EqualTo(name));
            Assert.That(client.GetProperty("DateOfBirth").GetString(), Is.EqualTo(dob));
            Assert.That(client.GetProperty("NetRelevantIncome").GetDecimal(), Is.EqualTo(Constants.AmountNotSet));
        }

        [Test]
        public async Task AddClient_PersistsClientToDatabase()
        {
            var tool = new ClientTools(_context);
            string name = "Montgomery Scott";
            string dob = "1970-05-06";

            await tool.AddClient(name, dob);

            var dbClient = await _context.Clients.FirstOrDefaultAsync(c => c.Name == name);
            Assert.That(dbClient, Is.Not.Null);
            Assert.That(dbClient.DateOfBirth, Is.EqualTo(new DateOnly(1970, 5, 6)));
            Assert.That(dbClient.NetRelevantIncome, Is.EqualTo(Constants.AmountNotSet));
        }

        [Test]
        public async Task AddClient_DuplicateThrowsMcpException()
        {
            var tool = new ClientTools(_context);
            string name = "Hikaru Sulu";
            string dob = "1984-05-12";

            await tool.AddClient(name, dob);

            var ex = Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.AddClient(name, dob);
            });

            Assert.That(ex.Message, Does.Contain("already exists"));
        }

        [Test]
        public void AddClient_InvalidDobFormatThrowsException()
        {
            var tool = new ClientTools(_context);

            var ex = Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.AddClient("Pavel Chekov", "AN INVALID DOB");
            });
            Assert.That(ex.Message, Does.Contain("Invalid dateOfBirth"));
        }

        [Test]
        public void AddClient_BlankName_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            var ex = Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.AddClient("", "1970-07-07");
            });
            Assert.That(ex.Message, Does.Contain("name"));
        }

        [Test]
        public void AddClient_WhitespaceName_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            var ex = Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.AddClient("   ", "1970-07-07");
            });
            Assert.That(ex.Message, Does.Contain("name"));
        }

        [Test]
        public async Task DeleteClient_RemovesClientFromDatabase()
        {
            var tool = new ClientTools(_context);

            await tool.DeleteClient(1);

            var dbClient = await _context.Clients.FindAsync(1);
            Assert.That(dbClient, Is.Null);
        }

        [Test]
        public async Task DeleteClient_ReturnsDeletedClientJson()
        {
            var tool = new ClientTools(_context);

            var json = await tool.DeleteClient(1);
            var deleted = JsonSerializer.Deserialize<JsonElement>(json);

            Assert.That(deleted.GetProperty("Id").GetInt32(), Is.EqualTo(1));
            Assert.That(deleted.GetProperty("Name").GetString(), Is.EqualTo("James Kirk"));
        }

        [Test]
        public void DeleteClient_InvalidId_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            var ex = Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.DeleteClient(9999);
            });
            Assert.That(ex.Message, Does.Contain("9999"));
        }

        [Test]
        public async Task UpdateClientName_UpdatesName()
        {
            var tool = new ClientTools(_context);

            await tool.UpdateClientName(1, "James T. Kirk");

            var dbClient = await _context.Clients.FindAsync(1);
            Assert.That(dbClient, Is.Not.Null);
            Assert.That(dbClient.Name, Is.EqualTo("James T. Kirk"));
        }

        [Test]
        public void UpdateClientName_InvalidId_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.UpdateClientName(3000, "James T. Kirk");
            });
        }

        [Test]
        public void UpdateClientName_BlankName_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.UpdateClientName(1, "");
            });
        }

        [Test]
        public async Task UpdateClientDateOfBirth_UpdatesDateOfBirth()
        {
            var tool = new ClientTools(_context);

            var json = await tool.UpdateClientDateOfBirth(1, "1974-12-25");
            var client = JsonSerializer.Deserialize<JsonElement>(json);

            Assert.That(client.GetProperty("DateOfBirth").GetString(), Is.EqualTo("1974-12-25"));
        }

        [Test]
        public void UpdateClientDateOfBirth_InvalidFormat_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            var ex = Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.UpdateClientDateOfBirth(1, "25/12/1974");
            });
            Assert.That(ex.Message, Does.Contain("dateOfBirth"));
        }

        [Test]
        public async Task UpdateClientNetRelevantIncome_UpdatesIncome()
        {
            var tool = new ClientTools(_context);

            var json = await tool.UpdateClientNetRelevantIncome(1, 55000m);
            var client = JsonSerializer.Deserialize<JsonElement>(json);

            Assert.That(client.GetProperty("NetRelevantIncome").GetDecimal(), Is.EqualTo(55000m));
        }

        [Test]
        public void UpdateClientNetRelevantIncome_ThrowsMcpException()
        {
            var tool = new ClientTools(_context);

            Assert.ThrowsAsync<McpException>(async () =>
            {
                await tool.UpdateClientNetRelevantIncome(100, 55000m);
            });
        }
    }
}
