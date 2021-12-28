module Latternfish

open System.IO
open Microsoft.FSharp.Collections

let spawn (count: int64) (generation: Map<int, int64>) =
    let elders =
        Option.defaultValue 0L (Map.tryFind 6 generation)

    generation
    |> Map.add 6 (count + elders)
    |> Map.add 8 count

let cycle age count generation = generation |> Map.add (age - 1) count

let grow (population: Map<int, int64>) =
    (population, Map.empty)
    ||> Map.foldBack
            (fun age count generation ->
                match age with
                | 0 -> spawn count generation
                | age when age > 0 -> cycle age count generation
                | age -> failwith $"Invalid fish age: {age}")

let evolveSchool days (seed: Map<int, int64>) =
    let duration = seq { 1 .. days }

    (seed, duration)
    ||> Seq.fold (fun population _ -> (grow population))

let readSeed file =
    File.ReadAllLines file
    |> Array.collect (fun line -> line.Split(',') |> Array.map int)
    |> Array.groupBy id
    |> Array.map (fun (age, count) -> (age, (int64 count.Length)))
    |> Map

let evolve days file =
    let seed = readSeed file

    (0L, (evolveSchool days seed))
    ||> Map.fold (fun s _ count -> s + count)
