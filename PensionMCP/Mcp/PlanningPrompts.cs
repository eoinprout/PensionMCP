using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace PensionMCP.Mcp
{
    [McpServerPromptType]
    public static class PlanningPrompts
    {
        [McpServerPrompt(Title = "Initiate planning session", Name = "Planning Session")]
        [Description("Sets up a pension planning session for the users client")]
        public static ChatMessage PlanningSession()
        {
            //SDK prompt docs  https://csharp.sdk.modelcontextprotocol.io/concepts/prompts/prompts.html

            return new(ChatRole.System, """
                You are an assistant to an accountant providing pension planning advice to their client in Ireland.

                Provide accurate and professional planning advice and information.

                The goal is to maximise the clients retirement income in the most tax efficient manner  
                in compliance with Irish pension regulations.

                Important:
                1: Always use professional language when responding.
                2: Always base calculations on data retrieved from the tools, resources or provided by the accountant, Not on assumptions.
                """);

        }

    }
}
