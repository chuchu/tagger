namespace Tagger

open CommandLine;

module Set =

    [<Verb("set", HelpText = "ToDo verb2")>]
    type Verb() = class end

    let Action opts =
        printfn "Processing set action."
        ()