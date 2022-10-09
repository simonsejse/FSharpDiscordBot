namespace Diku.Bot

module Program =
    open Discord.WebSocket
    open Microsoft.Extensions.Configuration
    open System.IO
    open System.Threading.Tasks
    open Discord
    open System
    
    type DiscordSlashCommandData = {
        Name : string
        Description : string
    }

    let appConfig: IConfigurationRoot =
            ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", true, true)
                .Build()

    let mutable client: DiscordSocketClient = null
  
    let doTestCommand (interaction: SocketSlashCommand) = task {
        do! interaction.RespondAsync("pong bro") :> Task
    } 

    let interactionCreate (data:SocketInteraction): Task = task {
        let isSlashCommand = data.Type = InteractionType.ApplicationCommand
        if isSlashCommand then
            let commandData = data :?> SocketSlashCommand
            match commandData.Data.Name with
            | "test" -> do! doTestCommand commandData 
            | _ -> ()
    }


    let createSlashCommand (commandData:DiscordSlashCommandData) =
        let builder = SlashCommandBuilder()
        builder.Name <- commandData.Name
        builder.Description <- commandData.Description
        builder.Build()
    
    let (commands:ApplicationCommandProperties array) = 
        [|
            {
                Name = "test"
                Description = "Ping pong little ding dong"
            };
        |] 
        |> Array.map createSlashCommand 
        |> Array.map(fun x -> x :> ApplicationCommandProperties)



    let onReady (): Task = task {
        printfn "Ready"
        do! client.BulkOverwriteGlobalApplicationCommandsAsync(commands) :> Task
    } 



    [<EntryPoint>]
    let main (argv: string[]) =
        printfn "Starting"

        client <- new DiscordSocketClient();

        client.add_InteractionCreated(Func<_, _>(interactionCreate))
        client.add_Ready(Func<_>(onReady))

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        let discordToken: string = appConfig.["Token"]

        client.LoginAsync (TokenType.Bot, discordToken) 
        |> Async.AwaitTask
        |> Async.RunSynchronously

        client.StartAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously

        Task.Delay(-1)
        |> Async.AwaitTask
        |> Async.RunSynchronously

        1