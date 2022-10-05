namespace Diku.Bot.Commands


open System
open DSharpPlus.CommandsNext
open DSharpPlus.CommandsNext.Attributes
open DSharpPlus.Entities
open FSharp.Control.Tasks

type Test () =

    inherit BaseCommandModule ()

    [<Command "inspire">]
    [<Description "Cast bardic inspiration on someone!">]
    member this.Inspiration (ctx: CommandContext, [<Description "Who do you want to inspire?">] user: DiscordMember) =
        unitTask {
            do!
                ctx.TriggerTypingAsync()

            let emoji = DiscordEmoji.FromName(ctx.Client, ":drum:").Name
            let roll = Random().Next(1, 7)
            let userName = user.Mention

            let! _ =
                sprintf "%s Bardic Inspiration! %s, add %i (1d6) to your next ability check, attack, or saving throw." emoji userName roll
                |> ctx.RespondAsync

            return ()
            }