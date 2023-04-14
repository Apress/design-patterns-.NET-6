module ChainOfResponsibility

type Person = {
  Age : int
  IsCitizen : bool
}

[<EntryPoint>]
let main argv =
  let oldEnough person = person.Age >= 16
  let ageReasonable person = person.Age < 130
  let isCitizen person = person.IsCitizen

  let check(f : Person -> bool) (person, result) =
    if not result then (person, false)
    else (person, f(person))

  let validationChain = check(oldEnough) >> check(ageReasonable) >> check(isCitizen)

  let sam = { Age = 40; IsCitizen = true };
  printf "Can Sam vote? %b" ((validationChain(sam, true)) |> snd)
  // Can Sam vote? true

  0