module DiagnosticsTests


open FsUnit
open Xunit

open Diagnostics

[<Fact>]
let ``Part 1: Captain, reading out the diagnositics-test.txt, we don't have power enough!`` () =
    calculatePowerConsumption "diagnostics-test.txt"
    |> should equal 198

[<Fact>]
let ``Part 1: Captain, reading out the diagnositics.txt, we don't have power enough!`` () =
    calculatePowerConsumption "diagnostics.txt"
    |> should equal 3847100
