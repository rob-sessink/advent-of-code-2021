module Main

open System
open SonarSweep

let usage () =
    printfn $"sonarsweep <window> <report>"
    -1

[<EntryPoint>]
let main args =
    if args = [||] || args.Length < 2 then
        usage ()
    else
        let window = Int32.Parse args.[0]
        let report = args.[1]
        let measurement = sweepFor window report
        printfn $"sonar sweeping report: %s{report} and measurement: %i{measurement}"
        0
