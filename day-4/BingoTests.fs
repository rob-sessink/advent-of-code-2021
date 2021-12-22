module BingoTests


open FsUnit
open Xunit

open Bingo

[<Fact>]
let ``Part 1: Rrrrrr, reading out the bingo-numbers-test.txt, winner winner, chicken dinner!`` () =
    play drawUntilWinner "bingo-numbers-test.txt" |> should equal 4512

[<Fact>]
let ``Part 1: Rrrrrr, reading out the bingo-numbers.txt, winner winner, chicken dinner!`` () =
    play drawUntilWinner "bingo-numbers.txt" |> should equal 10374

[<Fact>]
let ``Part 2: Rrrrrr, reading out the bingo-numbers-test.txt, last winner pays the dinner!`` () =
    play drawLastWinner "bingo-numbers-test.txt" |> should equal 1924

[<Fact>]
let ``Part 2: Rrrrrr, reading out the bingo-numbers.txt, last winner pays the dinner!`` () =
    play drawLastWinner "bingo-numbers.txt" |> should equal 24742
