module Diagnostics

open System
open System.IO

let calculateRate (diagnostics: char list list) func =
    diagnostics
    |> List.transpose
    |> List.map (List.countBy id)
    |> List.map (func snd)
    |> List.fold (fun s (c, _) -> c :: s) []
    |> List.rev

let toDecimalString (rate: char list) = Convert.ToInt32(String.Concat rate, 2)

let maxByOrWhenEqual value1 value2 : (char * int) =
    match Operators.compare (snd value1) (snd value2) with
    | 1 -> value1
    | -1 -> value2
    | 0 -> ('1', 1)
    | a -> invalidArg "compare values" $"Unknown comparison values: {a}"

let minByOrWhenEqual value1 value2 : (char * int) =
    match Operators.compare (snd value1) (snd value2) with
    | 1 -> value2
    | -1 -> value1
    | 0 -> ('0', 0)
    | a -> invalidArg "compare values" $"Unknown comparison values: {a}"

let cmpMax _ list = list |> List.reduce maxByOrWhenEqual

let cmpMin _ list = list |> List.reduce minByOrWhenEqual

let rec calculateRating cmp bit (diagnostics: char list list) =
    let tps = List.transpose diagnostics

    match diagnostics with
    | d when d.Length = 1 -> diagnostics |> List.concat
    | _ ->
        let m = tps.[bit] |> List.countBy id |> cmp snd

        let diagnostics' =
            diagnostics
            |> List.filter (fun d -> d.[bit] = fst m)

        calculateRating cmp (bit + 1) diagnostics'

let calculateGamma diagnostics = calculateRate diagnostics List.maxBy

let calculateEpsilon diagnostics = calculateRate diagnostics List.minBy

let calculateOxygenGenerator diagnostics = diagnostics |> calculateRating cmpMax 0

let calculateC02Scrubber diagnostics = diagnostics |> calculateRating cmpMin 0

let toDiagnostics report = report |> List.map Seq.toList

let readReport file = File.ReadLines file |> Seq.toList

let calculateByCombinedRateFunctions file rateFuncs =
    let diagnostics = readReport file |> toDiagnostics

    rateFuncs
    |> List.map (fun f -> f diagnostics |> toDecimalString)
    |> List.reduce (*)

let calculatePowerConsumption file =
    [ calculateGamma; calculateEpsilon ]
    |> calculateByCombinedRateFunctions file

let calculateLifeSupportRating file =
    [ calculateOxygenGenerator
      calculateC02Scrubber ]
    |> calculateByCombinedRateFunctions file
