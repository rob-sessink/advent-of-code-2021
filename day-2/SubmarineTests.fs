module SubmarineTests

open FsUnit.Xunit
open Xunit

open Submarine

[<Fact>]
let ``Part 1: Moving along the course using instructions-test.txt`` () =
    navigate "instructions-test.txt" |> should equal 150

[<Fact>]
let ``Part 1: Moving along the course using instructions.txt`` () =
    navigate "instructions.txt" |> should equal 1383564
