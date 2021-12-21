module Bingo

open System
open System.IO
open System.Text.RegularExpressions

type Number =
    | Number of number: int * drawn: bool
    static member Create(number) = Number(number, false)
    member this.Drawn() = this |> fun (Number (_, d)) -> d

    member this.Point() =
        this
        |> fun (Number (n, d)) -> if d = false then n else 0

    member this.Get() = this |> fun (Number (n, d)) -> (n, d)

    member this.Mark(drawn) =
        match this.Get() with
        | n, _ when n = drawn -> Number(n, true)
        | n, d -> Number(n, d)


let initializer (ci: int [] []) x y = Number(ci.[x].[y], false)

type BingoCard =
    { Size: int
      mutable Card: Number [,] }

    static member Create(size, arr) =
        { Size = size - 1
          Card = Array2D.init size size (initializer arr) }

    member private this.isBingoForSlice(slice: Number array) =
        let marked =
            slice
            |> Array.fold (fun s n -> if n.Drawn() then s + 1 else s) 0

        match marked with
        | d when d = slice.Length -> Some slice
        | _ -> None

    member private this.column(c: int) = this.Card.[c, *]

    member private this.row(r: int) = this.Card.[*, r]

    member private this.isBingoInDirection(f) =
        seq {
            for c in 0 .. this.Size do
                let slice = f c
                this.isBingoForSlice slice
        }
        |> Seq.choose id

    member private this.CountSlice(slice: Number array) =
        slice |> Array.fold (fun s n -> s + n.Point()) 0

    member private this.CountSlices(slices: seq<Number array>) =
        (slices |> Seq.map this.CountSlice) |> Seq.sum

    member private this.Points() =
        seq {
            for c in 0 .. this.Size do
                this.CountSlice this.Card.[c, *]
        }
        |> Seq.sum

    member this.UpdateFor(drawn) =
        this.Card <- this.Card |> Array2D.map (fun x -> x.Mark(drawn))

    member this.isBingo() =
        let columns = this.isBingoInDirection this.column
        let rows = this.isBingoInDirection this.row

        match (columns, rows) with
        | c, _ when Seq.length c > 0 -> Some(this, c)
        | _, r when Seq.length r > 0 -> Some(this, r)
        | _ -> None

    member this.Points(number) = number * this.Points()

let hasBingo number (cards: BingoCard list) =
    cards |> List.iter (fun c -> c.UpdateFor(number))

    let winners =
        cards
        |> List.map (fun c -> c.isBingo ())
        |> List.choose id
        |> List.map (fun (c, _) -> (c, c.Points(number)))

    match winners with
    | [] -> None
    | _ -> Some winners.Head

let readDrawnNumbers (lines: (int * string) []) =
    lines
    |> Array.take 1
    |> Array.collect (fun (_, s) -> s.Split(",") |> Array.map Int32.Parse)

let createBingoCard (chunk: (int * string) []) =
    let arr =
        chunk
        |> Array.map
            (fun (_, s) ->
                Regex.Split(s, "\s+")
                |> Array.filter (fun s -> s <> "")
                |> Array.map Int32.Parse)

    BingoCard.Create(5, arr)

let readBingoCards (lines: (int * string) []) =
    lines
    |> Array.skip 1
    |> Array.filter (fun (_, s) -> s <> "")
    |> Array.chunkBySize 5
    |> Array.map createBingoCard
    |> Array.toList

let drawUntilWinner numbers cards =
    Array.tryPick (fun num -> hasBingo num cards) numbers

let play file =
    let lines =
        File.ReadAllLines file
        |> Array.mapi (fun i line -> (i, line))

    let numbers = readDrawnNumbers lines
    let cards = readBingoCards lines
    let firstWinner = drawUntilWinner numbers cards

    match firstWinner with
    | None -> 0
    | Some x -> snd x
