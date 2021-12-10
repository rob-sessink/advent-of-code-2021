module SonarSweepTests

open FsUnit.Xunit
open Xunit

open SonarSweep

[<Fact>]
let ``Part 1: Sweeping the surface, scanning report-test.txt`` () =
    sweepFor 1 "report-test.txt" |> should equal 7

[<Fact>]
let ``Part 1: Sweeping the surface, scanning report.txt`` () =
    sweepFor 1 "report.txt" |> should equal 1446

[<Fact>]
let ``Part 2: Sweeping the surface, scanning report-test.txt with sliding window 3`` () =
    sweepFor 3 "report-test.txt" |> should equal 5

[<Fact>]
let ``Part 2: Sweeping the surface, scanning report.txt with sliding window 3`` () =
    sweepFor 3 "report-test.txt" |> should equal 5
