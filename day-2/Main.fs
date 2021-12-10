module Main

open Submarine

let usage () =
    printfn $"submarine <instructions>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 1 then
        usage ()
    else
        let instructions = args.[0]
        let coordinates = navigate instructions
        printfn $"navigating submarine by instruction: %s{instructions} to coordinates: %i{coordinates}"
        0
