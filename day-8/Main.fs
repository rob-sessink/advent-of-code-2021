module Main

open Segments

let usage () =
    printfn $"segments <unique|all> <input>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 2 then
        usage ()
    else
        let strategy = args.[0]
        let input = args.[1]
        let s = solve strategy input
        printfn $"Sensors indicating an exit at: %i{s}"
        0
