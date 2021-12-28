module LatternfishTests

open FsUnit
open Xunit

open Latternfish

[<Fact>]
let ``Part 1: Don't feed the Latternfish, using state-test.txt`` () =
    evolve 18 "state-test.txt" |> should equal 26L
    evolve 80 "state-test.txt" |> should equal 5934L
    
[<Fact>] 
let ``Part 1: Don't feed the Latternfish, using state.txt`` () =
    evolve 80 "state.txt" |> should equal 373378L

[<Fact>]
let ``Part 2: Don't feed the Latternfish, using state-test.txt`` () =
    evolve 256 "state-test.txt" |> should equal 26984457539L

[<Fact>]
let ``Part 2: Don't feed the Latternfish, using state.txt`` () =
    evolve 256 "state.txt" |> should equal 1682576647495L
    