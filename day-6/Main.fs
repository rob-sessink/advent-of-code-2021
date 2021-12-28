module Main

open Latternfish

let usage () =
    printfn $"latternfish <days> <state>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 2 then
        usage ()
    else
        let days = args.[0] |> int
        let state = args.[1]
        let population = evolve days state
        printfn $"Feeding fishies for: %i{days} grows a school population of: %i{population}"
        0
