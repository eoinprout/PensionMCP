using Microsoft.EntityFrameworkCore;
using ModelContextProtocol.Server;
using PensionMCP.Data;
using System.ComponentModel;
using System.Text.Json;

namespace PensionMCP.Mcp
{
    [McpServerToolType]
    public class ClientTools(PensionDbContext context)
    {
        [McpServerTool(Name = "get_all_clients")]
        [Description("Returns all client records as JSON")]
        public async Task<string> GetAllClients()
        {
            var clients = await context.Clients.ToListAsync();
            return JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
