module WhalesTreachery

open System
open System.IO

let alignments positions =
    [| Array.min positions .. Array.max positions |]

let alignmentAverage positions =
    let avg =
        Array.averageBy float positions
        |> Math.Ceiling
        |> int64

    [| Array.min positions .. avg |]

let weightLinear alignment position = abs (alignment - position)

let weightCumulative alignment position =
    let n = abs (alignment - position)

    seq { 0L .. (n - 1L) }
    |> Seq.sumBy (fun n -> n + 1L)

let costs weight alignment positions =
    let cost =
        positions
        |> Array.map (weight alignment)
        |> Array.sum

    (alignment, cost)

let costPerAlignment weight positions alignments =
    Array.map (fun alignment -> costs weight alignment positions) alignments

let mostEfficient cpa =
    Array.minBy (fun (_, cost: int64) -> cost) cpa

let solve' weight positions =
    positions
    |> alignments
    |> costPerAlignment weight positions
    |> mostEfficient

let toArray file =
    File.ReadLines(file)
    |> Seq.collect (fun l -> l.Split(",") |> Seq.map int64)
    |> Seq.toArray

let solve strategy file =
    match strategy with
    | "linear" -> (toArray file) |> solve' weightLinear
    | "cumulative" -> (toArray file) |> solve' weightCumulative
    | _ -> invalidArg "strategy" $"Unknown strategy: {strategy}"
