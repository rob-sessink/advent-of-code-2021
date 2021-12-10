module SonarSweep

open System.IO

type Variation =
    | Increase
    | Decrease
    | None

let determine m1 m2 =
    match m1, m2 with
    | a, b when a < b -> Increase
    | a, b when a > b -> Decrease
    | _ -> None

let deltasFor window measurements =
    measurements
    |> List.windowed window
    |> List.map List.sum
    |> List.pairwise
    |> List.map (fun (m1, m2) -> determine m1 m2)

let countBy (var: Variation) variations =
    variations
    |> List.filter (fun v -> v = var)
    |> List.length

let toList file =
    File.ReadLines(file) |> Seq.map int |> Seq.toList

let sweepFor window file =
    toList file
    |> deltasFor window
    |> countBy Increase
