module Segments

open System
open System.IO

type Digit =
    { Segments: string
      Digit: int option }

    static member Create(s) = { Segments = s; Digit = None }

    static member Create(s, d) = { Segments = s; Digit = Some d }

    member private this.FromChar(s: char seq) = s |> String.Concat |> Digit.Create

    member this.Combine(p: string) =
        this.Segments + p |> Seq.distinct |> this.FromChar

    member this.Remove(d: Digit) =
        this.Segments
        |> Seq.filter (fun c -> not (d.Segments.Contains(c)))
        |> this.FromChar

    member this.Remove(s: string) =
        this.Segments
        |> Seq.filter (fun c -> not (s.Contains(c)))
        |> this.FromChar

let (|Length|_|) (l: int) (signal: string) =
    if signal.Length = l then
        Some l
    else
        None

let (|EqualLength|_|) (d: Digit option) (signal: string) =
    let eql d (s: string) = d.Segments.Length = s.Length

    match (d, signal) with
    | Some d, s when eql d s -> Some signal.Length
    | _ -> None

let (|Inclusive|_|) (d: Digit option) (signal: string) =
    let containsAll (signal: string) (d: Digit) =
        d.Segments
        |> Seq.forall (fun c -> signal.Contains(c, StringComparison.OrdinalIgnoreCase))

    match (d, signal) with
    | Some da, s when containsAll s da -> Some signal
    | _ -> None

let (|NonInclusive|_|) (d: Digit option) (signal: string) =
    match signal with
    | Inclusive d _ -> None
    | p -> Some p

let (|Equal|_|) (d: Digit option) (signal: string) =
    match signal with
    | Inclusive d _ & EqualLength d _ -> d
    | _ -> None

let (|EqualWhenAdded|_|) (additive: Digit option) (d: Digit option) (signal: string) =
    let com =
        additive
        |> Option.map (fun x -> x.Combine(signal))

    let isEqual segment d =
        match segment with
        | Equal d _ -> Some signal
        | _ -> None

    isEqual com.Value.Segments d

let matches actpat pattern =
    match actpat pattern with
    | Some p -> Some p
    | _ -> None

let findDigit mf signals =
    let digits = signals |> Array.choose (matches mf)

    match digits with
    | d when d.Length = 1 -> Some d.[0]
    | _ -> invalidArg "digits" $"multiple digits identified: {digits}"

let (|One|_|) (signal: string) =
    match signal with
    | s when s.Length = 2 -> Some(Digit.Create(s, 1))
    | _ -> None

let (|Two|_|) (four: Digit option) (eight: Digit option) (signal: string) =
    match signal with
    | Length 5 _ & EqualWhenAdded four eight s -> Some(Digit.Create(s, 2))
    | _ -> None

let (|Three|_|) (one: Digit option) (signal: string) =
    match signal with
    | Length 5 _ & Inclusive one s -> Some(Digit.Create(s, 3))
    | _ -> None

let (|Four|_|) (signal: string) =
    match signal with
    | s when s.Length = 4 -> Some(Digit.Create(s, 4))
    | _ -> None

let (|Five|_|) (nine: Digit option) (one: Digit option) (signal: string) =
    match signal with
    | Length 5 _ & EqualWhenAdded nine one s -> Some(Digit.Create(s, 5))
    | _ -> None

let (|Six|_|) (one: Digit option) (signal: string) =
    match signal with
    | Length 6 _ & NonInclusive one s -> Some(Digit.Create(s, 6))
    | _ -> None

let (|Seven|_|) (signal: string) =
    match signal with
    | s when s.Length = 3 -> Some(Digit.Create(s, 7))
    | _ -> None

let (|Eight|_|) (signal: string) =
    match signal with
    | s when s.Length = 7 -> Some(Digit.Create(s, 8))
    | _ -> None

let (|Nine|_|) (four: Digit option) (signal: string) =
    match signal with
    | Length 6 _ & Inclusive four s -> Some(Digit.Create(s, 9))
    | _ -> None

let (|Zero|_|) (six: Digit option) (nine: Digit option) (signal: string) =
    match signal with
    | Length 6 _ & NonInclusive six _ & NonInclusive nine s -> Some(Digit.Create(s, 0))
    | _ -> None

/// Deduction rules for segments and numbers
///
/// As = 7[s] - 1[s]
/// Gs = 9[s] - 4[s] - As     -> 9 = len(6) and (?[s] incl 4[s])
/// Ds = 3[s] - 7[s] - Gs     -> 3 = len(5) and (?[s] incl 1[s])
/// Bs = 4[s] - 1[s] - Ds
/// Es = 8[s] - 9[s]
/// Cs = 8[s] - 6[s]          -> 6 = len(6) and (?[s] not incl 7[s])
/// Fs = 1[s] - Cs
let deduct (signals: string []) =
    let one = findDigit (|One|_|) signals
    let four = findDigit (|Four|_|) signals
    let seven = findDigit (|Seven|_|) signals
    let eight = findDigit (|Eight|_|) signals
    let nine = findDigit ((|Nine|_|) four) signals
    let three = findDigit ((|Three|_|) one) signals
    let six = findDigit ((|Six|_|) one) signals
    let zero = findDigit ((|Zero|_|) six nine) signals
    let two = findDigit ((|Two|_|) four eight) signals
    let five = findDigit ((|Five|_|) one nine) signals

    [ one
      two
      three
      four
      five
      six
      seven
      eight
      nine
      zero ]

let matchUnique (outputPattern: string) =
    match outputPattern with
    | One n -> Some n
    | Four n -> Some n
    | Seven n -> Some n
    | Eight n -> Some n
    | _ -> None

let matchAll (numbers: Digit option list) (outputPattern: string) =
    numbers
    |> List.pick
        (fun n ->
            match outputPattern with
            | Equal n _ -> Some n
            | _ -> None)

let nonEmpty s = s <> ""

let parseSignal (signals: string) =
    signals.Split(' ') |> Array.filter nonEmpty

let parseOutput (outputs: string) =
    outputs.Split(' ') |> Array.filter nonEmpty

let parseInput file =
    File.ReadAllLines file
    |> Array.map (fun l -> l.Split('|'))
    |> Array.map (fun s -> ((parseSignal s.[0]), (parseOutput s.[1])))

let deductUnique inputs =
    inputs
    |> Array.collect (fun (_, outputs) -> outputs |> Array.choose matchUnique)
    |> Array.length

let deductAll inputs =
    let combineAsNumber digits =
        digits
        |> Array.fold (fun s (j: Digit) -> s + string j.Digit.Value) ""
        |> Int32.Parse

    inputs
    |> Array.map
        (fun (segments, outputs) ->
            let numbers = deduct segments
            outputs |> Array.choose (matchAll numbers))
    |> Array.map combineAsNumber
    |> Array.reduce (+)

let solve strategy file =
    let inputs = parseInput file

    match strategy with
    | "unique" -> deductUnique inputs
    | "all" -> deductAll inputs
    | _ -> invalidArg "strategy" $"Unknown strategy: {strategy}"
