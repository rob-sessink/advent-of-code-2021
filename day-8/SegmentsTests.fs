module SegmentsTests

open FsUnit.Xunit
open Xunit

open Segments

[<Fact>]
let ``Part 1: Pattern matching galore with input-test.txt`` () =
    solve "unique" "input-test.txt" |> should equal 26

[<Fact>]
let ``Part 1: Pattern matching galore with input.txt`` () =
    solve "unique" "input.txt" |> should equal 383

[<Fact>]
let ``Part 2: Pattern matching galore counting all with input-test.txt`` () =
    solve "all" "input-test.txt" |> should equal 61229

[<Fact>]
let ``Part 2: Pattern matching galore counting all with input.txt`` () =
    solve "all" "input.txt" |> should equal 998900