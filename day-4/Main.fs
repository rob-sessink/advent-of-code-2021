module Main

open Bingo

let usage () =
    printfn $"bingo <cards>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 1 then
        usage ()
    else
        let file = args.[0]
        let points = play drawUntilWinner file
        points
