module Main

open WhalesTreachery

let usage () =
    printfn $"whalestreachery <linear|cumulative> <positions>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 2 then
        usage ()
    else
        let strategy = args.[0]
        let positions = args.[1]
        let position = solve strategy positions
        printfn $"Whales are coming: align the crab pods on position: %i{snd position} consuming %i{fst position} fuel"
        0
