module WhalesTreacheryTests

open FsUnit.Xunit
open Xunit

open WhalesTreachery

[<Fact>]
let ``Part 1: Whales are coming, solve most efficient position for linear fuel usage for positions-test.txt`` () =
    solve "linear" "positions-test.txt" |> should equal (2L, 37L)

[<Fact>]    
let ``Part 1: Whales are coming, solve most efficient position for linear fuel usage for positions.txt`` () =
    solve "linear" "positions.txt" |> should equal (349L, 352331L)    

[<Fact>]
let ``Part 2: Whales are coming, solve most efficient position for cumulative fuel usage for positions-test.txt`` () =
    solve "cumulative" "positions-test.txt" |> should equal (5L, 168L)

[<Fact>]
let ``Part 2: Whales are coming, solve most efficient position for cumulative fuel usage  for positions.txt`` () =
    solve "cumulative" "positions.txt" |> should equal (488L, 99266250L)
