module Main

open HydroThermal

let usage () =
    printfn $"hydrothermal <report>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 1 then
        usage ()
    else
        let vents = args.[0]
        let overlapping = pathFinder all "lines.txt"
        printfn $"finding path through vents: %s{vents} yields power consumption: %i{overlapping}"
        0
