namespace Diku.Bot

module Program =

    open System.Threading.Tasks
    open System.IO
    open Microsoft.Extensions.Configuration
    open DSharpPlus
    open DSharpPlus.CommandsNext
    open Diku.Bot.Commands

    let appConfig =
        ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json", true, true)
            .Build()

    [<EntryPoint>]
    let main argv =
        printfn "Starting"

        let token = appConfig.["Token"]
        let config = DiscordConfiguration ()
        config.Token <- token
        config.TokenType <- TokenType.Bot

        let client = new DiscordClient(config)

        let commandsConfig = CommandsNextConfiguration ()
        commandsConfig.StringPrefixes <- ["/"]

        let commands = client.UseCommandsNext(commandsConfig)
        commands.RegisterCommands<Test>()

        client.ConnectAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously

        Task.Delay(-1)
        |> Async.AwaitTask
        |> Async.RunSynchronously

        1