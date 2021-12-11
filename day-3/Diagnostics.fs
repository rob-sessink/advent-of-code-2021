module Diagnostics

open System
open System.IO

let calculateRate diagnostics func =
    diagnostics
    |> List.map (List.countBy id)
    |> List.map (func snd)
    |> List.fold (fun s (c, _) -> (s + string c)) ""

let toDecimal rate = Convert.ToInt32(rate, 2)

let calculateGamma diagnostics = calculateRate diagnostics List.maxBy

let calculateEpsilon diagnostics = calculateRate diagnostics List.minBy

let toDiagnostics report =
    report |> List.map Seq.toList |> List.transpose

let readReport file = File.ReadLines file |> Seq.toList

let calculatePowerConsumption file =
    let diagnostics = readReport file |> toDiagnostics

    [ calculateGamma; calculateEpsilon ]
    |> List.map (fun f -> f diagnostics |> toDecimal)
    |> List.reduce (*)
