module Main

open Diagnostics

let usage () =
    printfn $"diagnostics <report>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 1 then
        usage ()
    else
        let diagnostics = args.[0]
        let consumption = calculatePowerConsumption diagnostics
        printfn $"running diagnostics: %s{diagnostics} yields power consumption: %i{consumption}"
        0
