namespace Tagger

open CommandLine;

module Programm =

    [<EntryPoint>]
    let main argv =
        let result = Parser.Default.ParseArguments<Parse.Verb, Set.Verb, Print.Verb> argv
        match result with
        | :? Parsed<obj> as command ->
            match command.Value with
            | :? Parse.Verb as opts -> Parse.Action opts
            | :? Set.Verb as opts -> Set.Action opts
            | :? Print.Verb as opts -> Print.Action opts
            | _ -> ()
        | :? NotParsed<obj> -> ()
        | _ ->()
        0