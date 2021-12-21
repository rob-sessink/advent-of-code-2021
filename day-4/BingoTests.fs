module BingoTests


open FsUnit
open Xunit

open Bingo

[<Fact>]
let ``Part 1: Rrrrrr, reading out the bingo-numbers-test.txt, winner winner, chicken dinner!`` () =
    play "bingo-numbers-test.txt" |> should equal 4512

[<Fact>]
let ``Part 1: Rrrrrr, reading out the bingo-numbers.txt, winner winner, chicken dinner!`` () =
    play "bingo-numbers.txt" |> should equal 10374
